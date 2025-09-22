using Microsoft.EntityFrameworkCore;
using MyBots.Core.Models;
using Quartz;

namespace MyBots.Scheduling;

public class QuartzScheduleManager : IScheduleManager
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly Repository.BotDbContext _dbContext;

    public QuartzScheduleManager(ISchedulerFactory schedulerFactory, Repository.BotDbContext dbContext)
    {
        _schedulerFactory = schedulerFactory;
        _dbContext = dbContext;
    }

    public async Task ScheduleMessageAsync(long userId, string message, DateTimeOffset scheduledTime)
    {
        var scheduledMessage = new ScheduledMessage
        {
            UserId = userId,
            Message = message,
            ScheduledTime = scheduledTime,
            IsSent = false,
            IsBroadcast = false
        };
        
        _dbContext.Set<ScheduledMessage>().Add(scheduledMessage);
        await _dbContext.SaveChangesAsync();

        var scheduler = await _schedulerFactory.GetScheduler();
        var jobId = $"message_{scheduledMessage.Id}";
        
        var job = JobBuilder.Create<Jobs.SendMessageJob>()
            .WithIdentity(jobId)
            .UsingJobData("MessageId", scheduledMessage.Id)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger_{jobId}")
            .StartAt(scheduledTime.UtcDateTime)
            .Build();

        scheduledMessage.JobId = jobId;
        await _dbContext.SaveChangesAsync();
        
        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task ScheduleNotificationAsync(long[] userIds, string message, DateTimeOffset scheduledTime)
    {
        foreach (var userId in userIds)
        {
            await ScheduleMessageAsync(userId, message, scheduledTime);
        }
    }

    public async Task CancelScheduledMessageAsync(long messageId)
    {
        var message = await _dbContext.Set<ScheduledMessage>()
            .FirstOrDefaultAsync(m => m.Id == messageId && !m.IsSent);
            
        if (message?.JobId == null) return;

        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.DeleteJob(new JobKey(message.JobId));
        
        _dbContext.Set<ScheduledMessage>().Remove(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CancelAllUserScheduledMessagesAsync(long userId)
    {
        var messages = await _dbContext.Set<ScheduledMessage>()
            .Where(m => m.UserId == userId && !m.IsSent)
            .ToListAsync();
            
        var scheduler = await _schedulerFactory.GetScheduler();
        
        foreach (var message in messages)
        {
            if (message.JobId != null)
            {
                await scheduler.DeleteJob(new JobKey(message.JobId));
            }
        }
        
        _dbContext.Set<ScheduledMessage>().RemoveRange(messages);
        await _dbContext.SaveChangesAsync();
    }
}
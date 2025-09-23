using Microsoft.EntityFrameworkCore;
using MyBots.Core.Models;
using MyBots.Persistence;
using Quartz;

namespace MyBots.Scheduling;

/// <summary>
/// Implements <see cref="IScheduleManager"/> using Quartz.NET for job scheduling.
/// </summary>
public class QuartzScheduleManager : IScheduleManager
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly Repository.BotDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuartzScheduleManager"/> class.
    /// </summary>
    /// <param name="schedulerFactory">The factory for creating Quartz schedulers.</param>
    /// <param name="dbContext">The database context for persisting scheduled messages.</param>
    public QuartzScheduleManager(ISchedulerFactory schedulerFactory, Repository.BotDbContext dbContext)
    {
        _schedulerFactory = schedulerFactory;
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task ScheduleMessageAsync(long userId, string message, DateTimeOffset scheduledTime)
    {
        // Create and persist the message
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

        // Schedule the job with Quartz
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

        // Update the message with the job ID and save
        scheduledMessage.JobId = jobId;
        await _dbContext.SaveChangesAsync();
        
        await scheduler.ScheduleJob(job, trigger);
    }

    /// <inheritdoc />
    public async Task ScheduleNotificationAsync(long[] userIds, string message, DateTimeOffset scheduledTime)
    {
        // Schedule the same message for multiple users
        foreach (var userId in userIds)
        {
            await ScheduleMessageAsync(userId, message, scheduledTime);
        }
    }

    /// <inheritdoc />
    public async Task CancelScheduledMessageAsync(long messageId)
    {
        // Find the message if it hasn't been sent yet
        var message = await _dbContext.Set<ScheduledMessage>()
            .FirstOrDefaultAsync(m => m.Id == messageId && !m.IsSent);
            
        if (message?.JobId == null) return;

        // Delete the job from Quartz
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.DeleteJob(new JobKey(message.JobId));
        
        // Remove the message from the database
        _dbContext.Set<ScheduledMessage>().Remove(message);
        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task CancelAllUserScheduledMessagesAsync(long userId)
    {
        // Find all unsent messages for the user
        var messages = await _dbContext.Set<ScheduledMessage>()
            .Where(m => m.UserId == userId && !m.IsSent)
            .ToListAsync();
            
        var scheduler = await _schedulerFactory.GetScheduler();
        
        // Delete each job from Quartz
        foreach (var message in messages)
        {
            if (message.JobId != null)
            {
                await scheduler.DeleteJob(new JobKey(message.JobId));
            }
        }
        
        // Remove all messages from the database
        _dbContext.Set<ScheduledMessage>().RemoveRange(messages);
        await _dbContext.SaveChangesAsync();
    }
}
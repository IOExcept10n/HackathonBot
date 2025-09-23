using Microsoft.Extensions.DependencyInjection;
using MyBots.Core.Models;
using Quartz;
using Telegram.Bot;

namespace MyBots.Scheduling.Jobs;

/// <summary>
/// A Quartz job that sends scheduled messages to users through Telegram.
/// </summary>
[DisallowConcurrentExecution]
public class SendMessageJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendMessageJob"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public SendMessageJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the job by sending a scheduled message to a user.
    /// </summary>
    /// <param name="context">The context in which the job is executed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Execute(IJobExecutionContext context)
    {
        var messageId = context.JobDetail.JobDataMap.GetLong("MessageId");
        
        // Create a new service scope for database and bot client
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Repository.BotDbContext>();
        var message = await dbContext.Set<ScheduledMessage>().FindAsync(messageId);
        
        // Skip if message is not found or already sent
        if (message == null || message.IsSent) return;

        var bot = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        
        try
        {
            // Send the message through Telegram
            await bot.SendMessage(
                chatId: message.UserId,
                text: message.Message
            );
            
            // Mark the message as sent and save
            message.IsSent = true;
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            // TODO: Add proper error logging
        }
    }
}
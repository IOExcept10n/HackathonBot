using Microsoft.Extensions.DependencyInjection;
using MyBots.Core.Models;
using Quartz;
using Telegram.Bot;

namespace MyBots.Scheduling.Jobs;

[DisallowConcurrentExecution]
public class SendMessageJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public SendMessageJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messageId = context.JobDetail.JobDataMap.GetLong("MessageId");
        
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Repository.BotDbContext>();
        var message = await dbContext.Set<ScheduledMessage>().FindAsync(messageId);
        
        if (message == null || message.IsSent) return;

        var bot = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        
        try
        {
            await bot.SendMessage(
                chatId: message.UserId,
                text: message.Message
            );
            
            message.IsSent = true;
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            // Логирование ошибки
        }
    }
}
namespace MyBots.Scheduling;

public interface IScheduleManager
{
    Task ScheduleMessageAsync(long userId, string message, DateTimeOffset scheduledTime);
    Task ScheduleNotificationAsync(long[] userIds, string message, DateTimeOffset scheduledTime);
    Task CancelScheduledMessageAsync(long messageId);
    Task CancelAllUserScheduledMessagesAsync(long userId);
}
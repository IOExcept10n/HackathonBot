namespace MyBots.Scheduling;

/// <summary>
/// Provides functionality for scheduling and managing delayed messages in the bot system.
/// </summary>
public interface IScheduleManager
{
    /// <summary>
    /// Schedules a message to be sent to a specific user at a later time.
    /// </summary>
    /// <param name="userId">The ID of the user who will receive the message.</param>
    /// <param name="message">The text content of the message.</param>
    /// <param name="scheduledTime">The time when the message should be sent.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ScheduleMessageAsync(long userId, string message, DateTimeOffset scheduledTime);

    /// <summary>
    /// Schedules a notification to be sent to multiple users at a later time.
    /// </summary>
    /// <param name="userIds">The IDs of users who will receive the notification.</param>
    /// <param name="message">The text content of the notification.</param>
    /// <param name="scheduledTime">The time when the notification should be sent.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ScheduleNotificationAsync(long[] userIds, string message, DateTimeOffset scheduledTime);

    /// <summary>
    /// Cancels a previously scheduled message.
    /// </summary>
    /// <param name="messageId">The ID of the scheduled message to cancel.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CancelScheduledMessageAsync(long messageId);

    /// <summary>
    /// Cancels all scheduled messages for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose messages should be canceled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CancelAllUserScheduledMessagesAsync(long userId);
}
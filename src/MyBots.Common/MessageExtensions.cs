using System;
using MyBots.Core.Fsm.States;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyBots.Core;

public static class MessageExtensions
{
    public static MessageContent GetContent(this Message message) => message.Type switch
    {
        MessageType.Text => new TextMessageContent(message.Text!),
        MessageType.Document => new FileMessageContent(message.Document!.FileId),
        MessageType.Photo => new FileMessageContent(message.Photo!.Last().FileId),
        MessageType.Audio => new FileMessageContent(message.Audio!.FileId),
        MessageType.Video => new FileMessageContent(message.Video!.FileId),
        _ => MessageContent.Unknown,
    };
}

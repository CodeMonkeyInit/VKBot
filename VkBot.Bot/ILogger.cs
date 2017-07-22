using System;

namespace VkBot.Bot
{
    public interface ILogger
    {
        void LogException(Exception e);
    }
}
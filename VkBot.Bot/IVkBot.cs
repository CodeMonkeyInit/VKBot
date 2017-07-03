using System;

namespace VkBot.Bot
{
    public interface IVkBot
    {
        bool IsAuthorized { get; }

        bool BotWorking { get; }

        void RegisterTaskHandler(BotTaskHandler taskHandler);
        void SendResponse(BotResponse response);
        void Start(TimeSpan timeBetweenChecks);
        void RegisterOnUnknownCommandHandler(BotTaskHandler taskHandler);
        void RegisterGreetingTaskHandler(BotTaskHandler taskHandler);
        void Stop();
    }
}
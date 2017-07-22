using System;
using VkBot.BotApi;

namespace VkBot.Bot
{
    public interface IVkBot
    {
        bool IsAuthorized { get; }

        bool BotWorking { get; }

        void SetAccessToken(string accessToken);
        void RegisterTaskHandler(BotTaskHandler taskHandler);
        void SendResponse(BotResponse response);
        void Start(TimeSpan timeBetweenChecks);
        void RegisterOnUnknownCommandHandler(BotTaskHandler taskHandler);
        void RegisterGreetingTaskHandler(BotTaskHandler taskHandler);
        void Install(IBotFunctionsInstaller installer);
        void Stop();
    }
}
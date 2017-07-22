using VkBot.BotApi;
using VkBot.IocContainer;

namespace VkBot.Bot
{
    public class BotContainerInstaller : IContainerInstaller
    {
        public void Install(IContainer container)
        {
            container.Register<IVkApi, VkApi>();
            container.Register<ILogger, BotLogger>();
        }
    }
}
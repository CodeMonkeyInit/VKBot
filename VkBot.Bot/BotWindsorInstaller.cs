using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using VkBot.BotApi;

namespace VkBot.Bot
{
    public class BotWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IVkApi>()
                    .ImplementedBy<VkApi>()
            );

            container.Register(
                Component
                    .For<IBotFunctionsInstaller>()
                    .ImplementedBy<BotFunctionsInstaller>()
            );
        }
    }
}
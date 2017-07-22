using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using VkBot.Bot;
using VkBot.BotApi;
using VkBot.Functions;
using VkBot.Functions.UrbanDictionaryApi;
using VkBot.SettingsManager;

namespace VkBot.UI
{
    public class UIWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IVkBot>().ImplementedBy<Bot.VkBot>()
            );

            container.Register(
                Component.For<IVkSettingsManager>().ImplementedBy<VkSettingsManager>()
            );

            container.Register(
                Component.For<IBotFunctionsInstaller>().ImplementedBy<BotFunctionsInstaller>()
            );
        }
    }
}
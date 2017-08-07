using Microsoft.Practices.Unity;
using VkBot.Bot;
using VkBot.Functions;
using VkBot.SettingsManager;

namespace VkBot.UI.Container
{    
    public class Container
    {
        public static IUnityContainer GetUIContainer()
        {
            var unityContainer = new UnityContainer();

            unityContainer.RegisterType<IVkBot, Bot.VkBot>();
            unityContainer.RegisterType<IVkSettingsManager, VkSettingsManager>();
            unityContainer.RegisterType<IBotFunctionsInstaller, BotFunctionsInstaller>();

            return unityContainer;
        }
        
    }
}
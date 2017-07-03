using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace VkBot.BotApi
{
    public class VkApiWindsorIntaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<VkNet.VkApi>());

            container.Register(
                Component.For<IVkApi>().ImplementedBy<VkApi>()
            );
        }
    }
}

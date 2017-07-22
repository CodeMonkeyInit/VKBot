//using Castle.MicroKernel.Registration;
//using Castle.MicroKernel.SubSystems.Configuration;
//using Castle.Windsor;
using VkBot.IocContainer;

namespace VkBot.BotApi
{
    public class VkApiContainerIntaller : IContainerInstaller
    {
//        public void Install(IWindsorContainer container, IConfigurationStore store)
//        {
//            container.Register(Component.For<VkNet.VkApi>());
//
//            container.Register(
//                Component.For<IVkApi>().ImplementedBy<VkApi>()
//            );
//        }

        public void Install(IContainer container)
        {
            container.RegisterInstance(new VkNet.VkApi());

            container.Register<IVkApi, VkApi>();
        }
    }
}

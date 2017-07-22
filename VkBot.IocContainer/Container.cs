using System;
using Microsoft.Practices.Unity;

namespace VkBot.IocContainer
{
    public class Container : IContainer
    {
        private readonly IUnityContainer _container;

        public Container()
        {
            _container = new UnityContainer();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public void Register<TAbstract, TRealization>() where TRealization : TAbstract
        {
            _container.RegisterType<TAbstract, TRealization>();
        }

        public void RegisterInstance<TRealization>(TRealization instance)
            where TRealization : class
        {
            _container.RegisterInstance(instance);
        }

        public IContainer Install(IContainerInstaller installer)
        {
            installer.Install(this);

            return this;
        }
    }
}
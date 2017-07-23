namespace VkBot.IocContainer
{
    public interface IContainer
    {
        T Resolve<T>();

        void Register<TAbstract, TRealization>() where TRealization : TAbstract;

        void RegisterInstance<TRealization>(TRealization instance) where TRealization : class;

        IContainer Install(IContainerInstaller installer);
    }
}
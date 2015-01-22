namespace SystemDot.EventSourcing.Synchronisation.Server.Bootstrapping
{
    using System.Web.Http.Controllers;
    using SystemDot.Ioc;

    public static class IocContainerExtensions
    {
        public static void RegisterControllers(this IIocContainer container)
        {
            container.RegisterMultipleTypes()
                .FromAssemblyOf<SynchronisationController>()
                .ThatImplementType<IHttpController>()
                .ByClass(DependencyLifecycle.InstancePerDependency);
        }
    }
}
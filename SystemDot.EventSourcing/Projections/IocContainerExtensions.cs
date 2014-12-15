namespace SystemDot.EventSourcing.Projections
{
    using SystemDot.Ioc;

    public static class IocContainerExtensions
    {
        public static void RegisterProjectionsFromAssemblyOf<T>(this IIocContainer container)
        {
            container
                .RegisterMultipleTypes()
                .FromAssemblyOf<T>()
                .ThatImplementOpenType(typeof(IProjection<>))
                .ByClass();
        }
    }
}
using SystemDot.Ioc;

namespace SystemDot.EventSourcing.Sql.Windows.Bootstrapping
{
    using SystemDot.EventSourcing.Sessions;

    public static class IocContainerExtensions
    {
        internal static void RegisterSqlWindowsEventSourcing(this IIocContainer container)
        {
            container.RegisterInstance<IEventSessionFactory, EventStoreEventSessionFactory>();
        }
    }
}

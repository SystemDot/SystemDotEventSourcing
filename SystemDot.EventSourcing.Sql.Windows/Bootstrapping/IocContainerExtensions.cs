using SystemDot.EventSourcing.Sql.Windows.Lookups;
using SystemDot.Ioc;

namespace SystemDot.EventSourcing.Sql.Windows.Bootstrapping
{
    using SystemDot.EventSourcing.Sessions;

    public static class IocContainerExtensions
    {
        internal static void RegisterSqlWindowsEventSourcing(this IIocContainer container)
        {
            container.RegisterInstance<IAggregateLookup, AggregateLookup>();
            container.RegisterInstance<IAggregateLookupDataEngine, SqlEventStoreAggregateLookupDataEngine>();
            container.RegisterInstance<ILookupIdCache, LookupIdCache>();
            container.RegisterInstance<IEventSessionFactory, EventStoreEventSessionFactory>();
        }
    }
}

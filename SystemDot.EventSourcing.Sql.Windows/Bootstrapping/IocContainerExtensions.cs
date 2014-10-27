using SystemDot.EventSourcing.Sql.Windows.Lookups;
using SystemDot.Ioc;

namespace SystemDot.EventSourcing.Sql.Windows.Bootstrapping
{
    public static class IocContainerExtensions
    {
        internal static void RegisterSqlWindowsEventSourcing(this IIocContainer container)
        {
            container.RegisterInstance<ILookupIdCache, LookupIdCache>();
        }
    }
}

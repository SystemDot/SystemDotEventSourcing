using SystemDot.Ioc;

namespace SystemDot.EventSourcing.Esent.Windows.Configuration
{
    internal static class IocContainerExtensions
    {
        internal static void RegisterInMemoryEventSourcing(this IIocContainer container)
        {
            container.RegisterFromAssemblyOf<EsentEventSession>();
        }
    }
}

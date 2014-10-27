using SystemDot.EventSourcing.Sessions;
using SystemDot.Ioc;

namespace SystemDot.EventSourcing.InMemory.Bootstrapping
{
    internal static class IocContainerExtensions
    {
        internal static void RegisterInMemoryEventSourcing(this IIocContainer container)
        {
            container.RegisterInstance<IEventSessionFactory, InMemoryEventSessionFactory>();
        }
    }
}

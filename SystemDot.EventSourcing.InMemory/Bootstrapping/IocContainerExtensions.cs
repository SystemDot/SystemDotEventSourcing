using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;
using SystemDot.Ioc;

namespace SystemDot.EventSourcing.InMemory.Bootstrapping
{
    internal static class IocContainerExtensions
    {
        internal static void RegisterInMemoryEventSourcing(this IIocContainer container)
        {
            container.RegisterInstance<IEventSessionFactory, EventSessionFactory>();
            container.RegisterInstance<IEventStore, InMemoryEventStore>();
        }
    }
}

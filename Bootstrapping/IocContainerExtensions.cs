using System.Collections;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Projections;
using SystemDot.Ioc;

namespace SystemDot.EventSourcing.Bootstrapping
{
    using SystemDot.Messaging.Handling.Configuration;

    internal static class IocContainerExtensions
    {
        public static void RegisterEventSourcing(this IIocContainer container)
        {
            container.RegisterInstance<IDomainRepository, DomainRepository>();
        }

        public static async Task HydrateInMemoryProjections(this IIocContainer container)
        {
            await container.Resolve<ProjectionHydrater>().HydrateAsync(container.ResolveInMemoryProjections());
        }

        static IEnumerable ResolveInMemoryProjections(this IIocContainer container)
        {
            return container
                .ResolveMutipleTypes()
                .ThatImplementOpenType(typeof(IProjection<>))
                .ThatHaveAttribute<HydrateProjectionAtStartupAttribute>();
        }

        public static void RegisterProjectionsWithMessenger(this IIocContainer container)
        {
            container.RegisterOpenTypeHandlersWithMessenger(typeof(IProjection<>));
        }
    }
}

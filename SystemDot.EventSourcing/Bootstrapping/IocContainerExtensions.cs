using System.Collections;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Projections;
using SystemDot.Ioc;

namespace SystemDot.EventSourcing.Bootstrapping
{
    internal static class IocContainerExtensions
    {
        public static void RegisterEventSourcing(this IIocContainer container)
        {
            container.RegisterInstance<IDomainRepository, DomainRepository>();
        }

        public static async Task BuildReadModel(this IIocContainer container)
        {
            await container.Resolve<ProjectionBuilder>().BuildAsync(container.ResolveProjections());
        }

        static IEnumerable ResolveProjections(this IIocContainer container)
        {
            return container
                .ResolveMutipleTypes()
                .ThatImplementOpenType(typeof(IProjection<>));
        }
    }
}

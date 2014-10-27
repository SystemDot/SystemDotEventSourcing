using System.Collections;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Projections;
using SystemDot.EventSourcing.Projections.Mapping;
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
            await container.Resolve<ReadModelBuilder>().BuildAsync(container.ResolveMappers());
        }

        static IEnumerable ResolveMappers(this IIocContainer container)
        {
            return container
                .ResolveMutipleTypes()
                .ThatImplementOpenType(typeof(IReadModelMapper<>));
        }
    }
}

using System.Threading.Tasks;
using SystemDot.EventSourcing.Projections;

namespace SystemDot.EventSourcing.Specifications
{
    [HydrateProjectionAtStartup]
    public class TestHydrateAtStartupProjection : IProjection<TestAggregateRootCreatedEvent>
    {
        public string IdFromEvent { get; private set; }

        public Task Handle(TestAggregateRootCreatedEvent message)
        {
            IdFromEvent = message.Id;
            return Task.FromResult(false);
        }
    }
}
namespace SystemDot.EventSourcing.Specifications
{
    using System.Threading.Tasks;
    using SystemDot.EventSourcing.Projections;

    [HydrateProjectionAtStartup]
    public class TestHydrateAtStartupProjection : IProjection<TestAggregateRootCreatedEvent>
    {
        public string IdFromEvent { get; set; }

        public Task Handle(TestAggregateRootCreatedEvent message)
        {
            IdFromEvent = message.Id;
            return Task.FromResult(false);
        }
    }
}
namespace SystemDot.EventSourcing.Specifications
{
    using System.Threading.Tasks;
    using SystemDot.EventSourcing.Projections;

    public class TestProjection : IProjection<TestAggregateRootCreatedEvent>
    {
        public string IdFromEvent { get; set; }

        public Task Handle(TestAggregateRootCreatedEvent message)
        {
            IdFromEvent = message.Id;
            return Task.FromResult(false);
        }
    }
}
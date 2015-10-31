using SystemDot.EventSourcing.Aggregation;

namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Domain;

    public class TestAggregateRoot : AggregateRoot
    {
        public bool State { private set; get; }
        public string Id { get; private set; }
        public string SiteId { get; private set; }

        public static TestAggregateRoot Create(TestAggregateRootId id)
        {
            return new TestAggregateRoot(id);
        }

        TestAggregateRoot(MultiSiteId multiSiteId) : base(multiSiteId)
        {
            Then<TestAggregateRootCreatedEvent>(c =>
            {
                c.Id = multiSiteId.Id;
                c.SiteId = multiSiteId.SiteId;
            });
        }

        public TestAggregateRoot()
        {
        }

        public void SetSomeMoreStateResultingInEvent()
        {
            Then(new TestAggregateRootStateEvent());
        }

        void ApplyEvent(TestAggregateRootCreatedEvent @event)
        {
            Id = @event.Id;
            SiteId = @event.SiteId;
        }

        void ApplyEvent(TestAggregateRootStateEvent @event)
        {
            State = true;
        }
    }
}
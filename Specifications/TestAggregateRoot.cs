using SystemDot.EventSourcing.Aggregation;

namespace SystemDot.EventSourcing.Specifications
{
    public class TestAggregateRoot : AggregateRoot
    {
        public bool State { private set; get; }
        public string Id { get; private set; }
        public string BucketId { get; private set; }

        public static TestAggregateRoot Create(TestAggregateRootId id)
        {
            return new TestAggregateRoot(id);
        }

        TestAggregateRoot(TestAggregateRootId id)
            : base(id)
        {
            AddEvent<TestAggregateRootCreatedEvent>(c =>
            {
                c.Id = id.Id;
                c.BucketId = id.BucketId;
            });
        }

        public TestAggregateRoot()
        {
        }

        public void SetSomeMoreStateResultingInEvent()
        {
            AddEvent(new TestAggregateRootStateEvent());
        }

        void ApplyEvent(TestAggregateRootCreatedEvent @event)
        {
            Id = @event.Id;
            BucketId = @event.BucketId;
        }

        void ApplyEvent(TestAggregateRootStateEvent @event)
        {
            State = true;
        }
    }
}
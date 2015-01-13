using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Specifications
{
    using System.Security.Principal;

    public class TestAggregateRoot : AggregateRoot
    {
        public bool State { private set; get; }

        public static TestAggregateRoot Create(string id)
        {
            return new TestAggregateRoot(id);
        }

        TestAggregateRoot(string id) : base(id)
        {
            AddEvent<TestAggregateRootCreatedEvent>(c => c.Id = id);
        }

        public TestAggregateRoot()
        {
        }

        public void SetSomeMoreStateResultingInEvent()
        {
            AddEvent(new TestAggregateRootStateEvent());
        }

        void ApplyEvent(TestAggregateRootStateEvent @event)
        {
            State = true;
        }
    }
}
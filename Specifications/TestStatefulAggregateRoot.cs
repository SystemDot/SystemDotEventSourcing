namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Domain;
    using SystemDot.EventSourcing.Aggregation;

    
    public class TestStatefulAggregateRoot : AggregateRoot<TestStatefulAggregateRoot.TestStatefulAggregateRootState>
    {
        public class TestStatefulAggregateRootState
        {
            public bool SomeState { private set; get; }

            void ApplyEvent(TestAggregateRootStateEvent @event)
            {
                SomeState = true;
            }
        }
        
        protected override TestStatefulAggregateRootState CreateState()
        {
            return new TestStatefulAggregateRootState();
        }

        public bool SomeState { get { return State.SomeState; }}

        public static TestStatefulAggregateRoot Create(TestAggregateRootId id)
        {
            return new TestStatefulAggregateRoot(id);
        }

        TestStatefulAggregateRoot(MultiSiteId multiSiteId) : base(multiSiteId)
        {
            Then<TestAggregateRootCreatedEvent>(c =>
            {
                c.Id = multiSiteId.Id;
                c.SiteId = multiSiteId.SiteId;
            });
        }

        public TestStatefulAggregateRoot()
        {
        }

        public void SetSomeMoreStateResultingInEvent()
        {
            Then(new TestAggregateRootStateEvent());
        }

    }
}
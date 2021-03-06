namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.EventSourcing.Aggregation;

    class TestCommandInvokedAggregateRoot : 
        AggregateRoot, 
        IInvokeAggregate<TestCommand>
    {
        public void When(TestCommand command)
        {
            Then<TestAggregateRootCreatedEvent>(e =>
            {
                e.Id = command.Id;
                e.SiteId = command.BucketId;
            });
        }
    }
}
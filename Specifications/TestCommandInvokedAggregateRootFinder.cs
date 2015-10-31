namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Domain;
    using SystemDot.EventSourcing.Aggregation;

    class TestCommandInvokedAggregateRootFinder : IFindAggregates<TestCommand>
    {
        public MultiSiteId GetIdFromCommandToFindAggregateWith(TestCommand message)
        {
            return new TestAggregateRootId(message.Id, message.BucketId);
        }
    }
}
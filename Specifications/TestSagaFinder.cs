namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Domain;
    using SystemDot.EventSourcing.Sagas;

    class TestSagaFinder : 
        IFindSagas<TestStartSagaEvent>, 
        IFindSagas<TestSagaEvent>
    {
        public MultiSiteId GetIdFromEventToFindSagaWith(TestStartSagaEvent message)
        {
            return new TestAggregateRootId(message.Id, message.SiteId);
        }

        public MultiSiteId GetIdFromEventToFindSagaWith(TestSagaEvent message)
        {
            return new TestAggregateRootId(message.Id, message.SiteId);
        }
    }
}
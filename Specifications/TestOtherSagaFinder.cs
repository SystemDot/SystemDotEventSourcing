namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Domain;
    using SystemDot.EventSourcing.Sagas;

    class TestOtherSagaFinder : IFindSagas<TestStartSagaEvent>
    {
        public MultiSiteId GetIdFromEventToFindSagaWith(TestStartSagaEvent message)
        {
            return new TestAggregateRootId(message.Id, message.SiteId);
        }
    }
}
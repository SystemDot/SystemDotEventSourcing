namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Domain;

    public class TestAggregateRootId : MultiSiteId
    {
        public TestAggregateRootId(string id, string siteId) : base(id, siteId)
        {
        }
    }
}
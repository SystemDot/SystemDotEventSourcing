namespace SystemDot.EventSourcing
{
    using SystemDot.Domain;
    using SystemDot.EventSourcing.Streams;

    public static class MultiSiteIdExtensions
    {
        public static EventStreamId ToEventStreamId(this MultiSiteId multiSiteId)
        {
            return new EventStreamId(multiSiteId.Id, multiSiteId.SiteId);
        }
    }
}
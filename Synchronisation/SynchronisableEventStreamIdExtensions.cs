namespace SystemDot.EventSourcing.Synchronisation
{
    using SystemDot.EventSourcing.Streams;

    public static class SynchronisableEventStreamIdExtensions
    {
        public static EventStreamId ToEventStreamId(this SynchronisableEventStreamId id)
        {
            return new EventStreamId(id.Id, id.ClientId);
        }
    }
}
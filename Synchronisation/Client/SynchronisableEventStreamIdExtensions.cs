using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public static class SynchronisableEventStreamIdExtensions
    {
        public static EventStreamId ToEventStreamId(this SynchronisableEventStreamId id)
        {
            return new EventStreamId(id.Id, id.ClientId);
        }
    }
}
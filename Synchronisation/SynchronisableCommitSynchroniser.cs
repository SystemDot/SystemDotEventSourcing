namespace SystemDot.EventSourcing.Synchronisation
{
    using SystemDot.Core.Collections;
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.EventSourcing.Streams;

    public class SynchronisableCommitSynchroniser
    {
        readonly IEventSessionFactory eventSessionFactory;

        public SynchronisableCommitSynchroniser(IEventSessionFactory eventSessionFactory)
        {
            this.eventSessionFactory = eventSessionFactory;
        }

        public void SynchroniseCommit(SynchronisableCommit toSynchronise)
        {
            using (var eventSession = eventSessionFactory.Create())
            {
                toSynchronise.Events.ForEach(e => eventSession.StoreEvent(e.ToSourcedEvent(), toSynchronise.StreamId.ToEventStreamId()));
                toSynchronise.Headers.ForEach(h => eventSession.StoreHeader(toSynchronise.StreamId.ToEventStreamId(), h.Key, h.Value.Deserialise()));
                eventSession.CommitWithoutDispatching(toSynchronise.CommitId);
            }
        }
    }
}
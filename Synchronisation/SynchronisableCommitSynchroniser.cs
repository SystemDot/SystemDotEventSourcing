namespace SystemDot.EventSourcing.Synchronisation
{
    using SystemDot.Core.Collections;
    using SystemDot.EventSourcing.Sessions;

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
                eventSession.Commit(toSynchronise.CommitId);
            }
        }
    }
}
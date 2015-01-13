using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class CommitSynchroniser
    {
        readonly CommitRetrievalClient commitRetrievalClient;
        readonly IEventSessionFactory eventSessionFactory;

        public CommitSynchroniser(
            CommitRetrievalClient commitRetrievalClient, 
            IEventSessionFactory eventSessionFactory)
        {
            this.commitRetrievalClient = commitRetrievalClient;
            this.eventSessionFactory = eventSessionFactory;
        }

        public async Task Synchronise(Uri serverUri)
        {
            IEnumerable<SynchronisableCommit> commits = await commitRetrievalClient.GetCommitsAsync(serverUri);
            commits.ForEach(SynchroniseCommit);
        }

        void SynchroniseCommit(SynchronisableCommit toSynchronise)
        {
            using (var eventSession = eventSessionFactory.Create())
            {
                toSynchronise.Events.ForEach(e => eventSession.StoreEvent(e.ToSourcedEvent(), toSynchronise.StreamId));
                eventSession.Commit(toSynchronise.CommitId);
            }
        }
    }
}
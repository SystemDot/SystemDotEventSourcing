using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class CommitSynchroniser
    {
        readonly ICommitRetrievalClient commitRetrievalClient;
        readonly IEventSessionFactory eventSessionFactory;

        public CommitSynchroniser(
            ICommitRetrievalClient commitRetrievalClient, 
            IEventSessionFactory eventSessionFactory)
        {
            this.commitRetrievalClient = commitRetrievalClient;
            this.eventSessionFactory = eventSessionFactory;
        }

        public async Task<DateTime> Synchronise(Uri serverUri, DateTime commitsStartFrom)
        {
            DateTime lastCommitDate = DateTime.MinValue;

            IEnumerable<SynchronisableCommit> commits = await commitRetrievalClient.GetCommitsAsync(serverUri, commitsStartFrom);

            commits.ForEach(c =>
            {
                lastCommitDate = c.CreatedOn;
                SynchroniseCommit(c);
            });

            return lastCommitDate;
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
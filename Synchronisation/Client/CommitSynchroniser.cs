using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public async Task Synchronise(Uri serverUri, DateTime getCommitsFrom, Action<SynchronisableCommit> onProcessingCommit, Action onError)
        {
            HttpResponseMessage response = await commitRetrievalClient.GetCommitsAsync(serverUri, getCommitsFrom.Ticks);

            if (!response.IsSuccessStatusCode)
            {
                onError();
                return;
            }

            IEnumerable<SynchronisableCommit> commits = await response.ReadContentAsSynchronisableCommitsAsync();

            commits.ForEach(commit =>
            {
                onProcessingCommit(commit);
                SynchroniseCommit(commit);
            });
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
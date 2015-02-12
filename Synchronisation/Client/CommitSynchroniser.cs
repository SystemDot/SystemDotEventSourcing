using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Synchronisation.Client.Http;
using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    using System.Net;

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

        public async Task Synchronise(CommitRetrievalCriteria criteria, Action<DateTime> onComplete, Action onError)
        {
            HttpResponseMessage response;
            try
            {
                response = await commitRetrievalClient.GetCommitsAsync(criteria.ServerUri, criteria.ClientId, criteria.GetCommitsFrom.Ticks);
            }
            catch (WebException)
            {
                onError();
                return;
            }
            
            if (!response.IsSuccessStatusCode)
            {
                onError();
                return;
            }

            IEnumerable<SynchronisableCommit> commits = await response.ReadContentAsSynchronisableCommitsAsync();

            DateTime lastCommitDate = criteria.GetCommitsFrom;
            
            commits.ForEach(commit =>
            {
                SynchroniseCommit(commit);
                lastCommitDate = commit.CreatedOn;
            });

            onComplete(lastCommitDate);
        }

        void SynchroniseCommit(SynchronisableCommit toSynchronise)
        {
            using (var eventSession = eventSessionFactory.Create())
            {
                toSynchronise.Events.ForEach(e => eventSession.StoreEvent(e.ToSourcedEvent(), toSynchronise.StreamId.ToEventStreamId()));
                eventSession.Commit(toSynchronise.CommitId);
            }
        }
    }
}
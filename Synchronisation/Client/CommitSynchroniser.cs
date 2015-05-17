

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using SystemDot.Core.Collections;
    using SystemDot.EventSourcing.Synchronisation.Client.Http;
    using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;
    using System.Linq;
    using System.Net;
    using SystemDot.Environment;

    public class CommitSynchroniser
    {
        readonly ISynchronisationHttpClient synchronisationHttpClient;
        readonly SynchronisationServerUriProvider serverUriProvider;
        readonly SynchronisableCommitSynchroniser synchronisableCommitSynchroniser;
        readonly SynchronisableCommitBuilder synchronisableCommitBuilder;
        readonly ILocalMachine localMachine;

        public CommitSynchroniser(
            ISynchronisationHttpClient synchronisationHttpClient, 
            SynchronisationServerUriProvider serverUriProvider, 
            SynchronisableCommitSynchroniser synchronisableCommitSynchroniser,
            SynchronisableCommitBuilder synchronisableCommitBuilder, 
            ILocalMachine localMachine)
        {
            this.synchronisationHttpClient = synchronisationHttpClient;
            this.serverUriProvider = serverUriProvider;

            this.synchronisableCommitSynchroniser = synchronisableCommitSynchroniser;
            this.synchronisableCommitBuilder = synchronisableCommitBuilder;
            this.localMachine = localMachine;
        }

        public async Task PullAsync(CommitRetrievalCriteria criteria, Action<DateTime> onComplete, Action onError)
        {
            HttpResponseMessage response;
            try
            {
                response = await synchronisationHttpClient.GetCommitsAsync(serverUriProvider.ServerUri, criteria.ClientId, criteria.GetCommitsFrom.Ticks);
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
                synchronisableCommitSynchroniser.SynchroniseCommit(commit);
                lastCommitDate = commit.CreatedOn;
            });

            onComplete(lastCommitDate);
        }

        public async Task PushAsync(CommitRetrievalCriteria criteria, Action<DateTime> onComplete, Action onError)
        {
            HttpResponseMessage response;
            DateTime lastCommitDate = criteria.GetCommitsFrom;
            
            try
            {
                IEnumerable<SynchronisableCommit> commits = synchronisableCommitBuilder.Build(
                    criteria.ClientId, 
                    criteria.GetCommitsFrom, 
                    commit => commit.OriginatesOnMachineNamed(localMachine.GetName()));

                if (commits.Any())
                {
                    lastCommitDate = commits.Last().CreatedOn;
                }

                response = await synchronisationHttpClient.PostCommitsAsync(serverUriProvider.ServerUri, commits.SerialiseToHttpContent());
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

            onComplete(lastCommitDate);
        }
    }
}


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

        public async Task SynchroniseAsync(CommitSynchronisationCriteria criteria, Action<CommitSynchronisationResult> onComplete, Action onError)
        {
            HttpResponseMessage response;
            try
            {
                response = await synchronisationHttpClient.GetCommitsAsync(serverUriProvider.ServerUri, criteria.ClientId, criteria.PullCommitsFrom.Ticks);
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

            IEnumerable<SynchronisableCommit> pullCommits = await response.ReadContentAsSynchronisableCommitsAsync();

            DateTime lastPullCommitDate = criteria.PullCommitsFrom;
            
            pullCommits.ForEach(commit =>
            {
                synchronisableCommitSynchroniser.SynchroniseCommit(commit);
                lastPullCommitDate = commit.CreatedOn;
            });

            DateTime lastPushCommitDate = criteria.PullCommitsFrom;

            try
            {
                IEnumerable<SynchronisableCommit> pushCommits = synchronisableCommitBuilder.Build(
                    criteria.ClientId,
                    criteria.PullCommitsFrom,
                    commit => commit.OriginatesOnMachineNamed(localMachine.GetName()));

                if (pushCommits.Any())
                {
                    lastPushCommitDate = pushCommits.Last().CreatedOn;
                }

                response = await synchronisationHttpClient.PostCommitsAsync(serverUriProvider.ServerUri, pushCommits.SerialiseToHttpContent());
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

            onComplete(new CommitSynchronisationResult(lastPullCommitDate, lastPushCommitDate));
        }
    }
}
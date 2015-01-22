namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SystemDot.Core;
    using SystemDot.EventSourcing.Synchronisation;
    using SystemDot.EventSourcing.Synchronisation.Client;

    public class TestCommitRetrievalClient : ICommitRetrievalClient
    {
        readonly List<SynchronisableCommit> commits = new List<SynchronisableCommit>();

        public Uri LastGetCommitsAsyncCallServerUri { get; private set; }
        
        public DateTime LastGetCommitsAsyncCallFrom { get; private set; }

        public Task<IEnumerable<SynchronisableCommit>> GetCommitsAsync(Uri serverUri, DateTime from)
        {
            LastGetCommitsAsyncCallServerUri = serverUri;
            LastGetCommitsAsyncCallFrom = from;

            return Task.FromResult(commits.As<IEnumerable<SynchronisableCommit>>());
        }
        
        public void Add(SynchronisableCommit toAdd)
        {
            commits.Add(toAdd);
        }
    }
}
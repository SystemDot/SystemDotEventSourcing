using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.Core;
using SystemDot.EventSourcing.Synchronisation.Client;

namespace SystemDot.EventSourcing.Synchronisation.Testing
{
    public class TestCommitRetrievalClient : ICommitRetrievalClient
    {
        readonly List<SynchronisableCommit> commits = new List<SynchronisableCommit>();

        public Uri LastGetCommitsAsyncCallServerUri { get; set; }

        public Task<IEnumerable<SynchronisableCommit>> GetCommitsAsync(Uri serverUri)
        {
            return Task.FromResult(commits.As<IEnumerable<SynchronisableCommit>>());
        }
        
        public void Add(SynchronisableCommit toAdd)
        {
            commits.Add(toAdd);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public interface ICommitRetrievalClient
    {
        Task<IEnumerable<SynchronisableCommit>> GetCommitsAsync(Uri serverUri, DateTime from);
    }
}
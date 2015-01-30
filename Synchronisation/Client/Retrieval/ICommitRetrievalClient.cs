using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Synchronisation.Client.Retrieval
{
    public interface ICommitRetrievalClient
    {
        Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, string clientId, long @fromCommitInTicks);
    }
}
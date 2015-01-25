using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public interface ICommitRetrievalClient
    {
        Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, DateTime from);
    }
}
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Synchronisation.Client.Retrieval
{
    public class CommitRetrievalClient : ICommitRetrievalClient
    {
        readonly HttpClient httpClient;

        public CommitRetrievalClient()
        {
            httpClient = new HttpClient(); 
        }

        public async Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, string clientId, long @fromCommitInTicks)
        {
            return await httpClient.GetAsync(SynchronisationUri.Parse(serverUri, clientId, @fromCommitInTicks));
        }
    }
}
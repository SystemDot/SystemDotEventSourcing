using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Synchronisation.Client.Retrieval
{
    using SystemDot.EventSourcing.Synchronisation.Client.Http;

    public class CommitRetrievalClient : ICommitRetrievalClient
    {
        readonly IHttpClientFactory httpClientFactory;

        public CommitRetrievalClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, string clientId, long @fromCommitInTicks)
        {
            return await httpClientFactory.Create().GetAsync(SynchronisationUri.Parse(serverUri, clientId, @fromCommitInTicks));
        }
    }
}
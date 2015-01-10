using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Synchronisation.Client.Http;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class CommitRetrievalClient
    {
        readonly IHttpClientFactory httpClientFactory;

        public CommitRetrievalClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<SynchronisableCommit>> GetCommitsAsync(Uri serverUri)
        {
            HttpResponseMessage response = await httpClientFactory
                .Create()
                .GetAsync(new Uri(serverUri, "Synchronisation"));
           
            return await response
                .Content
                .ReadAsAsync<IEnumerable<SynchronisableCommit>>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class CommitRetrievalClient : ICommitRetrievalClient
    {
        readonly HttpClient httpClient;

        public CommitRetrievalClient()
        {
            httpClient = new HttpClient(); 
        }

        public async Task<IEnumerable<SynchronisableCommit>> GetCommitsAsync(Uri serverUri, DateTime @from)
        {
            HttpResponseMessage response = await httpClient
                .GetAsync(new Uri(serverUri, string.Format("Synchronisation/{0}", @from)));

            return await response
                .Content
                .ReadAsAsync<IEnumerable<SynchronisableCommit>>();
        }
    }
}
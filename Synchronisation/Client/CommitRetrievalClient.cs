using System;
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

        public async Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, DateTime @from)
        {
           return await httpClient.GetAsync(SynchronisationUri.Parse(serverUri, @from));
        }
    }
}
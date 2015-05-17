namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class SynchronisationHttpClient : ISynchronisationHttpClient
    {
        readonly IHttpClientFactory httpClientFactory;

        public SynchronisationHttpClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, string clientId, long @fromCommitInTicks)
        {
            return await httpClientFactory.Create().GetAsync(GetEventsSynchronisationUri.Parse(serverUri, clientId, @fromCommitInTicks));
        }

        public async Task<HttpResponseMessage> PostCommitsAsync(Uri serverUri, HttpContent content)
        {
            return await httpClientFactory.Create().PostAsync(PostEventsSynchronisationUri.Parse(serverUri), content);
        }
    }
}
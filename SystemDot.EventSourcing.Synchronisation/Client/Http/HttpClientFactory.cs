using System.Net.Http;

namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}
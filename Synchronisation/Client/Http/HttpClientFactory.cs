namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    using System.Net.Http;

    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}
using System.Net.Http;
using SystemDot.EventSourcing.Synchronisation.Client.Http;
using Microsoft.Owin.Testing;

namespace SystemDot.EventSourcing.Synchronisation.Testing
{
    public class TestHttpClientFactory : IHttpClientFactory
    {
        public TestServer TestServer { get; set; }

        public HttpClient Create()
        {
            return TestServer.HttpClient;
        }
    }
}
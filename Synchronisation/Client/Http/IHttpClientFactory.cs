using System.Net.Http;

namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
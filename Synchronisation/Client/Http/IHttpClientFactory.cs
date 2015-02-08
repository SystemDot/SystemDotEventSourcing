namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    using System.Net.Http;

    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
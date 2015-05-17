namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface ISynchronisationHttpClient
    {
        Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, string clientId, long @fromCommitInTicks);
        Task<HttpResponseMessage> PostCommitsAsync(Uri serverUri, HttpContent content);
    }
}
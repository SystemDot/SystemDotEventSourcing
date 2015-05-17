namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Server
{
    using System.Collections.Generic;
    using System.Net.Http;
    using SystemDot.EventSourcing.Synchronisation;
    using Newtonsoft.Json;

    public static class HttpContentExtensions
    {
        public static IEnumerable<SynchronisableCommit> DeserialiseCommits(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<IEnumerable<SynchronisableCommit>>(content.ReadAsStringAsync().Result);
        }
    }
}
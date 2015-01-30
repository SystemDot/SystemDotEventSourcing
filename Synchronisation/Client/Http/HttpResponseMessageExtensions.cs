using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<IEnumerable<SynchronisableCommit>> ReadContentAsSynchronisableCommitsAsync(
            this HttpResponseMessage message)
        {
            return await message.Content.ReadAsAsync<IEnumerable<SynchronisableCommit>>();
        }
    }
}
using System.Net.Http;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<CommitSynchronisation> ReadContentAsCommitSynchronisationAsync(
            this HttpResponseMessage message)
        {
            return await message.Content.ReadAsAsync<CommitSynchronisation>();
        }
    }
}
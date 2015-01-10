using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            var resultString = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(resultString);
        }
    }
}
namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    using System.Net.Http;
    using Newtonsoft.Json;

    public static class ObjectExtensions
    {
        public static StringContent SerialiseToHttpContent(this object toSerialise)
        {
            return new StringContent(JsonConvert.SerializeObject(toSerialise));
        }
    }
}
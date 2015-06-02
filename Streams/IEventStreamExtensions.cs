namespace SystemDot.EventSourcing.Streams
{
    using System.Collections.Generic;

    public static class IEventStreamExtensions
    {
        public static void AddIfNotPresent(this IDictionary<string, object> dictionary, string key, object value)
        {
            if (dictionary.ContainsKey(key))
            {
                return;
            }
            dictionary.Add(key, value);
        }
    }
}
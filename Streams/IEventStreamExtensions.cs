namespace SystemDot.EventSourcing.Streams
{
    public static class IEventStreamExtensions
    {
        public static void AddHeaderIfNotPresent(this IEventStream stream, string key, object value)
        {
            if (stream.UncommittedHeaders.ContainsKey(key))
            {
                return;
            }
            stream.UncommittedHeaders.Add(key, value);
        }
    }
}
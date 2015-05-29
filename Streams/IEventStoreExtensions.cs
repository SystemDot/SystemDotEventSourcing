namespace SystemDot.EventSourcing.Streams
{
    using System.Linq;

    public static class IEventStoreExtensions
    {
        public static string Describe(this IEventStore store)
        {
            return store.GetCommits().Select(c => c.ToString()).Aggregate((previous, next) => previous + ", " + next);
        }
    }
}
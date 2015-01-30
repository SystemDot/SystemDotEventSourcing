using System.Linq;
using SystemDot.EventSourcing.Commits;
using NEventStore;

namespace SystemDot.EventSourcing.Sql.Windows
{
    public static class CommitExtensions
    {
        public static Commit CreateCommit(this ICommit commit)
        {
            return new Commit(
                commit.CommitId, 
                commit.BucketId, 
                commit.StreamId, 
                commit.Events.Select(e => EventMessageExtensions.CreateSourcedEvent(e)).ToList(), 
                commit.Headers);
        }
    }
}
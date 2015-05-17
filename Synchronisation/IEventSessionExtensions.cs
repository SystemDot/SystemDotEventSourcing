namespace SystemDot.EventSourcing.Synchronisation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Sessions;

    public static class IEventSessionExtensions
    {
        public static IEnumerable<SynchronisableCommit> GetSynchronisableCommits(
            this IEventSession session, 
            string bucketId, 
            DateTime @from, 
            Func<Commit, bool> commitsWhereClause)
        {
            return session
                .AllCommitsFrom(bucketId, @from)
                .Where(commitsWhereClause)
                .Select(commit => new SynchronisableCommit
                {
                    CommitId = commit.CommitId,
                    StreamId = new SynchronisableEventStreamId { ClientId = commit.BucketId, Id = commit.StreamId },
                    CreatedOn = commit.CreatedOn,
                    Headers = commit.GetCommitHeaders(),
                    Events = commit.GetSynchronisableEvents()
                });
        }
    }
} 
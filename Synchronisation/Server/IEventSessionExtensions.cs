using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Synchronisation.Server
{
    public static class IEventSessionExtensions
    {
        public static IEnumerable<SynchronisableCommit> GetSynchronisableCommits(this IEventSession session, string bucketId, DateTime @from)
        {
            return session
                .AllCommitsFrom(bucketId, @from)
                .Select(commit => new SynchronisableCommit
                {
                    CommitId = commit.CommitId,
                    StreamId = new SynchronisableEventStreamId { ClientId = commit.BucketId, Id = commit.StreamId },
                    CreatedOn = commit.CreatedOn,
                    Events = commit.GetSynchronisableEvents()
                });
        }
    }
}
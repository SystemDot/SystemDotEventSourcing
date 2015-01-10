using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Synchronisation.Server
{
    public static class IEventSessionExtensions
    {
        public static IEnumerable<SynchronisableCommit> GetSynchronisableCommits(this IEventSession session)
        {
            return session
                .AllCommitsFrom(DateTime.MinValue)
                .Select(commit => new SynchronisableCommit
                {
                    CommitId = commit.CommitId,
                    StreamId = commit.StreamId,
                    CreatedOn = commit.CreatedOn,
                    Events = commit.GetSynchronisableEvents()
                });
        }
    }
}
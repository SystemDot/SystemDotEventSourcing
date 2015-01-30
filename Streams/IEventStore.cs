using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Commits;

namespace SystemDot.EventSourcing.Streams
{
    public interface IEventStore
    {
        IEventStream OpenStream(EventStreamId streamId);
        IEnumerable<Commit> GetCommitsFrom(string bucketId, DateTime @from);
        IEnumerable<Commit> GetCommits();
    }
}
using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Commits;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Sessions
{
    public interface IEventSession : IDisposable
    {
        IEnumerable<SourcedEvent> GetEvents(EventStreamId streamId);

        void StoreEvent(SourcedEvent @event, EventStreamId id);

        void Commit(Guid commitId);

        void CommitWithoutDispatching(Guid commitId);
        
        IEnumerable<Commit> AllCommits();

        IEnumerable<Commit> AllCommitsFrom(string bucketId, DateTime from);

        void StoreHeader(EventStreamId id, string key, object value);
    }
}
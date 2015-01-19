using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Commits;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Sessions
{
    public interface IEventSession : IDisposable
    {
        IEnumerable<SourcedEvent> GetEvents(string streamId);

        void StoreEvent(SourcedEvent @event, string id);

        void Commit(Guid commitId);

        IEnumerable<Commit> AllCommitsFrom(DateTime from);

        void StoreHeader(string id, string key, object value);
    }
}
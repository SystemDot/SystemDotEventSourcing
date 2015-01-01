using System;
using System.Collections.Generic;

namespace SystemDot.EventSourcing.Streams
{
    public interface IEventStream : IDisposable
    {
        IEnumerable<SourcedEvent> CommittedEvents { get; }
        IEnumerable<SourcedEvent> UncommittedEvents { get; }
        void Add(SourcedEvent @event);
        void CommitChanges(Guid commitId);
        void ClearChanges();
    }
}
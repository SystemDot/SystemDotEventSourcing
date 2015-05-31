using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.EventSourcing.Commits;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.InMemory
{
    public class InMemoryEventStream : IEventStream
    {
        readonly EventStreamId streamId;
        readonly List<SourcedEvent> uncommittedEvents;
        readonly List<SourcedEvent> committedEvents;
        readonly Action<Commit> addCommit;
        readonly IDictionary<string, object> uncommittedHeaders;

        public InMemoryEventStream(EventStreamId streamId, List<SourcedEvent> committedEvents, Action<Commit> addCommit)
        {
            this.streamId = streamId;
            this.committedEvents = committedEvents;
            this.addCommit = addCommit;
            uncommittedEvents = new List<SourcedEvent>();
            uncommittedHeaders = new Dictionary<string, object>();
        }

        public void Dispose()
        {
        }

        public IEnumerable<SourcedEvent> CommittedEvents
        {
            get { return committedEvents; }
        }

        public IEnumerable<SourcedEvent> UncommittedEvents
        {
            get { return uncommittedEvents; }
        }

        public IDictionary<string, object> UncommittedHeaders
        {
            get { return uncommittedHeaders; }
        }

        public void Add(SourcedEvent @event)
        {
            uncommittedEvents.Add(@event);
        }

        public void CommitChanges(Guid commitId)
        {
            addCommit(new Commit(commitId, streamId.BucketId, streamId.Id, uncommittedEvents.ToList(), uncommittedHeaders, DateTime.Now));
            committedEvents.AddRange(uncommittedEvents);
            ClearChanges();
        }

        public void ClearChanges()
        {
            uncommittedEvents.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core.Collections;
using SystemDot.Domain.Events.Dispatching;
using SystemDot.EventSourcing.Commits;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        readonly IEventDispatcher eventDispatcher;
        readonly List<Commit> commits;
        readonly Dictionary<string, IEventStream> streams;

        public InMemoryEventStore(IEventDispatcher eventDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
            streams = new Dictionary<string, IEventStream>();
            commits = new List<Commit>();
        }

        public IEventStream OpenStream(string streamId)
        {
            if (!streams.ContainsKey(streamId))
            {
                streams.Add(streamId, new InMemoryEventStream(streamId, GetEvents(streamId), Commit));
            }
            return streams[streamId];
        }

        void Commit(Commit commit)
        {
             commits.Add(commit);
             commit.Events.ForEach(e => eventDispatcher.Dispatch(e.Body));
        }

        List<SourcedEvent> GetEvents(string streamId)
        {
            return commits
                .Where(c => c.StreamId == streamId)
                .SelectMany(c => c.Events).ToList();
        }

        public IEnumerable<Commit> GetCommitsFrom(DateTime @from)
        {
            return commits.Where(c => c.CreatedOn >= @from);
        }
    }
}
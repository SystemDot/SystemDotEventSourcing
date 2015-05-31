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
        readonly Dictionary<EventStreamId, IEventStream> streams;

        public InMemoryEventStore(IEventDispatcher eventDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
            streams = new Dictionary<EventStreamId, IEventStream>();
            commits = new List<Commit>();
        }

        public IEventStream OpenStream(EventStreamId streamId)
        {
            if (!streams.ContainsKey(streamId))
            {
                streams.Add(streamId, new InMemoryEventStream(streamId, GetEvents(streamId), Commit));
            }
            return streams[streamId];
        }

        public IEnumerable<Commit> GetCommitsFrom(string bucketId, DateTime @from)
        {
            return commits.Where(c => c.BucketId == bucketId && c.CreatedOn >= @from);
        }

        public IEnumerable<Commit> GetCommits()
        {
            return commits;
        }

        void Commit(Commit commit)
        {
            if (commits.Any(c => c.CommitId == commit.CommitId))
            {
                throw new DuplicateCommitException();
            }

            commits.Add(commit);
            
            if (commit.Headers.ContainsKey(PreventCommitDispatchHeader.Key))
            {
                return;
            }

            commit.Events.ForEach(e => eventDispatcher.Dispatch(e.Body));
        }

        List<SourcedEvent> GetEvents(EventStreamId streamId)
        {
            return commits
                .Where(c => c.StreamId == streamId.Id && c.BucketId == streamId.BucketId)
                .SelectMany(c => c.Events).ToList();
        }
    }
}
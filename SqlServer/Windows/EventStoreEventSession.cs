using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;
using SystemDot.EventSourcing.Commits;
using NEventStore;

namespace SystemDot.EventSourcing.Sql.Windows
{
    public class EventStoreEventSession : Disposable, IEventSession
    {
        readonly IStoreEvents eventStore;
        readonly Dictionary<EventStreamId, NEventStore.IEventStream> streams;

        public EventStoreEventSession(IStoreEvents eventStore)
        {
            this.eventStore = eventStore;
            streams = new Dictionary<EventStreamId, NEventStore.IEventStream>();
        }

        public IEnumerable<Commit> AllCommits()
        {
             return eventStore.Advanced.GetFrom().Select(c => c.CreateCommit());
        }

        public IEnumerable<Commit> AllCommitsFrom(string bucketId, DateTime @from)
        {
            return eventStore.Advanced.GetFrom(bucketId, @from).Select(c => c.CreateCommit());
        }

        public void StoreHeader(EventStreamId id, string key, object value)
        {
            GetStream(id).UncommittedHeaders[key] = value;
        }

        public IEnumerable<SourcedEvent> GetEvents(EventStreamId streamId)
        {
            NEventStore.IEventStream stream = GetStream(streamId);

            return stream.CommittedEvents
                .Select(e => e.CreateSourcedEvent())
                .Concat(stream.UncommittedEvents
                .Select(e => e.CreateSourcedEvent()));
        }

        public void StoreEvent(SourcedEvent @event, EventStreamId aggregateRootId)
        {
            var uncommittedEvent = new EventMessage
            {
                Body = @event.Body
            };

            @event.Headers.ForEach(h => uncommittedEvent.Headers.Add(h.Key, h.Value));

            GetStream(aggregateRootId).Add(uncommittedEvent);
        }

        public void Commit(Guid commitId)
        {
            foreach (var stream in streams)
            {
                CommitStream(commitId, stream.Value);
            }
        }

        public void CommitWithoutDispatching(Guid commitId)
        {
            foreach (var stream in streams)
            {
                stream.Value.UncommittedHeaders.AddIfNotPresent(PreventCommitDispatchHeader.Key, new PreventCommitDispatchHeader());
                CommitStream(commitId, stream.Value);
            }
        }

        NEventStore.IEventStream GetStream(EventStreamId streamId)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
            NEventStore.IEventStream stream;

            if (!streams.TryGetValue(streamId, out stream)) streams[streamId] = stream = eventStore.OpenStream(streamId.BucketId, streamId.Id, 0, int.MaxValue);

            return stream;
        }

        void CommitStream(Guid commitId, NEventStore.IEventStream toCommit)
        {
            try
            {
                toCommit.CommitChanges(commitId);
            }
            catch (NEventStore.DuplicateCommitException)
            {
                toCommit.ClearChanges();
            }
            catch (NEventStore.ConcurrencyException)
            {
                toCommit.ClearChanges();
                throw;
            }
        }

        protected override void DisposeOfManagedResources()
        {
            streams.ForEach(s => s.Value.Dispose());
            streams.Clear();
            base.DisposeOfManagedResources();
        }
    }
}
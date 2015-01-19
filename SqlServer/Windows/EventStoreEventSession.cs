using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;
using SystemDot.EventSourcing.Commits;

namespace SystemDot.EventSourcing.Sql.Windows
{
    public class EventStoreEventSession : Disposable, IEventSession
    {
        readonly NEventStore.IStoreEvents eventStore;
        readonly Dictionary<string, NEventStore.IEventStream> streams;

        public EventStoreEventSession(NEventStore.IStoreEvents eventStore)
        {
            this.eventStore = eventStore;
            streams = new Dictionary<string, NEventStore.IEventStream>();
        }

        public IEnumerable<Commit> AllCommitsFrom(DateTime @from)
        {
            return eventStore.Advanced
                .GetFrom("default", @from)
                .Select(c => new Commit(c.CommitId, c.StreamId, c.Events.Select(CreateSourcedEvent).ToList(), c.Headers));
        }

        public void StoreHeader(string id, string key, object value)
        {
            GetStream(id).UncommittedHeaders[key] = value;
        }

        public IEnumerable<SourcedEvent> GetEvents(string streamId)
        {
            NEventStore.IEventStream stream = GetStream(streamId);

            return stream.CommittedEvents
                .Select(CreateSourcedEvent)
                .Concat(stream.UncommittedEvents
                .Select(CreateSourcedEvent));
        }

        SourcedEvent CreateSourcedEvent(NEventStore.EventMessage @from)
        {
            var @event = new SourcedEvent
            {
                Body = @from.Body
            };

            @from.Headers.ForEach(h => @event.Headers.Add(h.Key, h.Value));

            return @event;
        }

        public void StoreEvent(SourcedEvent @event, string aggregateRootId)
        {
            var uncommittedEvent = new NEventStore.EventMessage
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

        NEventStore.IEventStream GetStream(string aggregateRootId)
        {
            NEventStore.IEventStream stream;

            if (!streams.TryGetValue(aggregateRootId, out stream)) streams[aggregateRootId] = stream = eventStore.OpenStream("default", aggregateRootId, 0, int.MaxValue);

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
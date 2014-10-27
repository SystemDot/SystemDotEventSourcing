using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Sql.Windows.Lookups;
using EventStore;

namespace SystemDot.EventSourcing.Sql.Windows
{
    public class EventStoreEventSession : Disposable, IEventSession
    {
        readonly IStoreEvents eventStore;
        readonly IAggregateLookup lookup;
        readonly Dictionary<Guid, IEventStream> streams;

        public EventStoreEventSession(IStoreEvents eventStore, IAggregateLookup lookup)
        {
            this.eventStore = eventStore;
            this.lookup = lookup;
            streams = new Dictionary<Guid, IEventStream>();
        }

        public async Task<IEnumerable<SourcedEvent>> AllEventsAsync()
        {
            return await Task.Run(() =>
                eventStore
                    .Advanced
                    .GetFrom(DateTime.MinValue)
                    .SelectMany(e => e.Events)
                    .Select(CreateSourcedEvent));
        }

        public async Task<IEnumerable<SourcedEvent>> GetEventsAsync(string streamId)
        {
            IEventStream stream = GetStream(GetInternalAggregateRootId(streamId));

            return await Task.Run(() =>
                stream.CommittedEvents
                    .Select(CreateSourcedEvent)
                    .Concat(stream.UncommittedEvents
                    .Select(CreateSourcedEvent)));
        }

        SourcedEvent CreateSourcedEvent(EventMessage @from)
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
            var uncommittedEvent = new EventMessage
            {
                Body = @event.Body
            };

            @event.Headers.ForEach(h => uncommittedEvent.Headers.Add(h.Key, h.Value));

            GetStream(GetInternalAggregateRootId(aggregateRootId)).Add(uncommittedEvent);
        }

        Guid GetInternalAggregateRootId(string aggregateRootId)
        {
            return lookup.LookupId<string>(aggregateRootId);
        }

        public async Task CommitAsync()
        {
            foreach (var stream in streams)
            {
                await CommitStreamAsync(Guid.NewGuid(), stream.Value);
            }
        }

        IEventStream GetStream(Guid aggregateRootId)
        {
            IEventStream stream;

            if (!streams.TryGetValue(aggregateRootId, out stream)) streams[aggregateRootId] = stream = eventStore.OpenStream(aggregateRootId, 0, int.MaxValue);

            return stream;
        }

        async Task CommitStreamAsync(Guid commandId, IEventStream toCommit)
        {
            try
            {
                await Task.Run(() => toCommit.CommitChanges(commandId));
            }
            catch (DuplicateCommitException)
            {
                toCommit.ClearChanges();
            }
            catch (ConcurrencyException)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Sessions;
using NEventStore;

namespace SystemDot.EventSourcing.Sql.Windows
{
    using NEventStore.Persistence;

    public class EventStoreEventSession : Disposable, IEventSession
    {
        readonly IStoreEvents eventStore;
        readonly Dictionary<string, IEventStream> streams;

        public EventStoreEventSession(IStoreEvents eventStore)
        {
            this.eventStore = eventStore;
            streams = new Dictionary<string, IEventStream>();
        }

        public async Task<IEnumerable<SourcedEvent>> AllEventsAsync()
        {
            return await Task.FromResult(
                eventStore
                    .Advanced
                    .GetFromStart()
                    .SelectMany(e => e.Events)
                    .Select(CreateSourcedEvent));
        }

        public async Task<IEnumerable<SourcedEvent>> GetEventsAsync(string streamId)
        {
            IEventStream stream = GetStream(streamId);

            return await Task.FromResult(
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

            GetStream(aggregateRootId).Add(uncommittedEvent);
        }

        public async Task CommitAsync()
        {
            foreach (var stream in streams)
            {
                await CommitStreamAsync(Guid.NewGuid(), stream.Value);
            }
        }

        IEventStream GetStream(string aggregateRootId)
        {
            IEventStream stream;

            if (!streams.TryGetValue(aggregateRootId, out stream)) streams[aggregateRootId] = stream = eventStore.OpenStream(aggregateRootId, 0);

            return stream;
        }

        async Task CommitStreamAsync(Guid commandId, IEventStream toCommit)
        {
            try
            {
                toCommit.CommitChanges(commandId);
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
            await Task.FromResult(false);
        }

        protected override void DisposeOfManagedResources()
        {
            streams.ForEach(s => s.Value.Dispose());
            streams.Clear();

            base.DisposeOfManagedResources();
        }
    }
}
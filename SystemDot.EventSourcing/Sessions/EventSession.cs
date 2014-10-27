using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.Domain.Events.Dispatching;

namespace SystemDot.EventSourcing.Sessions
{
    public abstract class EventSession : Disposable, IEventSession
    {
        readonly IEventDispatcher eventDispatcher;
        readonly List<EventContainer> eventsToCommit;

        protected EventSession(IEventDispatcher eventDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
            eventsToCommit = new List<EventContainer>();
        }

        public abstract Task<IEnumerable<SourcedEvent>> GetEventsAsync(string streamId);

        public void StoreEvent(SourcedEvent @event, string aggregateRootId)
        {
            eventsToCommit.Add(new EventContainer(aggregateRootId, @event));
        }

        public async Task CommitAsync()
        {
            eventsToCommit.ForEach(CommitEvent);
                OnEventsCommitted();
                eventsToCommit.Clear();

            await Task.FromResult(false);
        }

        void CommitEvent(EventContainer @event)
        {
            eventDispatcher.Dispatch(@event.Event.Body);
            OnEventCommitting(@event);
        }

        protected abstract void OnEventsCommitted();

        protected abstract void OnEventCommitting(EventContainer eventContainer);

        public abstract Task<IEnumerable<SourcedEvent>> AllEventsAsync();
    }
}
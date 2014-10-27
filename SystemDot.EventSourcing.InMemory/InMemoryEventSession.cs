using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.Domain.Events.Dispatching;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.InMemory
{
    public class InMemoryEventSession : EventSession
    {
        readonly List<EventContainer> events;

        public InMemoryEventSession(IEventDispatcher eventDispatcher) : base(eventDispatcher)
        {
            events = new List<EventContainer>();
        }

        public override async Task<IEnumerable<SourcedEvent>> GetEventsAsync(string streamId)
        {
            return await Task.FromResult(
                events
                    .Where(e => e.AggregateRootId == streamId)
                    .Select(e => e.Event));
        }

        protected override void OnEventsCommitted()
        {
        }

        protected override void OnEventCommitting(EventContainer eventContainer)
        {
            events.Add(eventContainer);
        }

        public override async Task<IEnumerable<SourcedEvent>> AllEventsAsync()
        {
            return await Task.FromResult(events.Select(c => c.Event));
        }
    }
}
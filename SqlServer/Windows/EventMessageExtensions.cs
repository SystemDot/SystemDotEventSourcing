using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Streams;
using NEventStore;

namespace SystemDot.EventSourcing.Sql.Windows
{
    public static class EventMessageExtensions
    {
        public static SourcedEvent CreateSourcedEvent(this EventMessage @from)
        {
            var @event = new SourcedEvent
            {
                Body = @from.Body
            };

            @from.Headers.ForEach(h => @event.Headers.Add(h.Key, h.Value));

            return @event;
        }
    }
}
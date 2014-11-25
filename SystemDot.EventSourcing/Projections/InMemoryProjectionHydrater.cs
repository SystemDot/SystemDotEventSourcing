using System.Collections;
using System.Threading.Tasks;
using SystemDot.Core.Collections;
using SystemDot.Messaging.Handling;

namespace SystemDot.EventSourcing.Projections
{
    public class InMemoryProjectionHydrater
    {
        readonly MessageHandlerRouter eventRouter;
        readonly EventRetreiver eventRetreiver;

        public InMemoryProjectionHydrater(EventRetreiver eventRetreiver)
        {
            this.eventRetreiver = eventRetreiver;

            eventRouter = new MessageHandlerRouter();
        }

        public async Task HydrateAsync(IEnumerable projections)
        {
            PopulateRouter(projections);
            var allEvents = await eventRetreiver.GetAllEventsAsync();
            foreach (var sourcedEvent in allEvents)
            {
                await BuildFromEventAsync(sourcedEvent.Body);
            }
        }

        async Task BuildFromEventAsync(object @event)
        {
            await eventRouter.RouteMessageToHandlersAsync(@event);
        }

        void PopulateRouter(IEnumerable mappers)
        {
            mappers.ForEach(eventRouter.RegisterHandler);
        }
    }
}
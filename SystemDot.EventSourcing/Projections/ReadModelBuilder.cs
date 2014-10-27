using System.Collections;
using System.Threading.Tasks;
using SystemDot.Core.Collections;
using SystemDot.Messaging.Handling;

namespace SystemDot.EventSourcing.Projections
{
    public class ReadModelBuilder
    {
        readonly MessageHandlerRouter eventRouter;
        readonly EventRetreiver eventRetreiver;

        public ReadModelBuilder(EventRetreiver eventRetreiver)
        {
            this.eventRetreiver = eventRetreiver;

            eventRouter = new MessageHandlerRouter();
        }

        public async Task BuildAsync(IEnumerable mappers)
        {
            PopulateRouter(mappers);
            var allEvents = await eventRetreiver.GetAllEventsAsync();
            foreach (var sourcedEvent in allEvents)
            {
                await BuildFromEventAsync(sourcedEvent);
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
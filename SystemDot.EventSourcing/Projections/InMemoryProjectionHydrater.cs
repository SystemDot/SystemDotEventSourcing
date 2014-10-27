using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Sessions;
using SystemDot.Messaging.Handling;

namespace SystemDot.EventSourcing.Projections
{
    public class InMemoryProjectionHydrater
    {
        readonly IEventSessionFactory eventSessionFactory;

        public InMemoryProjectionHydrater(IEventSessionFactory eventSessionFactory)
        {
            this.eventSessionFactory = eventSessionFactory;
        }

        public async Task HydrateProjectionsAsync(IEnumerable projections)
        {
            MessageHandlerRouter router = PopulateHandlerRouter(projections);
            IEnumerable<SourcedEvent> sourcedEvents = await eventSessionFactory.Create().AllEventsAsync();

            foreach (var @event in sourcedEvents.Select(s => s.Body))
            {
                await router.RouteMessageToHandlersAsync(@event);
            }
        }

        MessageHandlerRouter PopulateHandlerRouter(IEnumerable projections)
        {
            var router = new MessageHandlerRouter();

            projections.ForEach(router.RegisterHandler);

            return router;
        }
    }
}
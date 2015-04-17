namespace SystemDot.EventSourcing.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SystemDot.Core.Collections;
    using SystemDot.Domain;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Messaging.Handling;

    public class EventProjector
    {
        private readonly EventRetreiver retreiver;

        public EventProjector(EventRetreiver retreiver)
        {
            this.retreiver = retreiver;
        }

        public async Task ProjectAsync<TProjection>(MultiSiteId id, Action<TProjection> onLoaded) where TProjection : new()
        {
            await CreateProjectionFromEvents(retreiver.GetEvents(id.ToEventStreamId()), onLoaded);
        }

        public async Task ProjectAsync<TProjection>(string siteId, Action<TProjection> onLoaded) where TProjection : new()
        {
            await CreateProjectionFromEvents(retreiver.GetAllEventsInBucket(siteId), onLoaded);
        }

        async Task CreateProjectionFromEvents<TProjection>(IEnumerable<SourcedEvent> events, Action<TProjection> onLoaded) where TProjection : new()
        {
            var projection = new TProjection();

            var router = new MessageHandlerRouter();
            router.RegisterHandler(projection);

            foreach (var e in events)
            {
                await router.RouteMessageToHandlersAsync(e.Body);
            }

            router.UnregisterHandler(projection);
            onLoaded(projection);
        }
    }
}
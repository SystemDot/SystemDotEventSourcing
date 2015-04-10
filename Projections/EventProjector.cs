namespace SystemDot.EventSourcing.Projections
{
    using System;
    using SystemDot.Core.Collections;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Messaging.Handling;

    public class EventProjector
    {
        private readonly EventRetreiver retreiver;

        public EventProjector(EventRetreiver retreiver)
        {
            this.retreiver = retreiver;
        }

        public void Project<TProjection>(EventStreamId id, Action<TProjection> onLoaded) where TProjection : new()
        {
            var projection = new TProjection();

            var router = new MessageHandlerRouter();
            router.RegisterHandler(projection);
            retreiver.GetEvents(id).ForEach(e => router.RouteMessageToHandlers(e));
            router.UnregisterHandler(projection);

            onLoaded(projection);
        }
    }
}
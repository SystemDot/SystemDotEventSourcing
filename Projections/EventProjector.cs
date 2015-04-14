namespace SystemDot.EventSourcing.Projections
{
    using System;
    using System.Collections.Generic;
    using SystemDot.Core.Collections;
    using SystemDot.EventSourcing.Aggregation;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Messaging.Handling;

    public class EventProjector
    {
        private readonly EventRetreiver retreiver;

        public EventProjector(EventRetreiver retreiver)
        {
            this.retreiver = retreiver;
        }

        public void Project<TProjection>(string bucketId, Action<TProjection> onLoaded) where TProjection : new()
        {
            CreateProjectionFromEvents(retreiver.GetAllEventsInBucket(bucketId), onLoaded);
        }

        public void Project<TProjection>(string bucketId, string id, Action<TProjection> onLoaded) where TProjection : new()
        {
            CreateProjectionFromEvents(retreiver.GetEvents(new EventStreamId(id, bucketId)), onLoaded);
        }

        public void Project<TProjection>(AggregateRootId id, Action<TProjection> onLoaded) where TProjection : new()
        {
            CreateProjectionFromEvents(retreiver.GetEvents(id.ToEventStreamId()), onLoaded);
        }

        void CreateProjectionFromEvents<TProjection>(IEnumerable<SourcedEvent> events, Action<TProjection> onLoaded) where TProjection : new()
        {
            var projection = new TProjection();

            var router = new MessageHandlerRouter();
            router.RegisterHandler(projection);
            events.ForEach(e => router.RouteMessageToHandlers(e.Body));
            router.UnregisterHandler(projection);

            onLoaded(projection);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing
{
    public class EventRetreiver
    {
        readonly IEventSessionFactory factory;

        public EventRetreiver(IEventSessionFactory factory)
        {
            this.factory = factory;
        }

        public IEnumerable<SourcedEvent> GetAllEvents()
        {
            return factory.Create()
                .AllCommits()
                .SelectMany(c => c.Events)
                .ToList();
        }

        public IEnumerable<SourcedEvent> GetAllEventsInBucket(string bucketId)
        {
            return factory.Create()
                .AllCommitsFrom(bucketId, DateTime.MinValue)
                .SelectMany(c => c.Events)
                .ToList();
        }

        public IEnumerable<SourcedEvent> GetEvents(EventStreamId id)
        {
            return factory.Create()
                .GetEvents(id)
                .ToList();
        }
    }
}
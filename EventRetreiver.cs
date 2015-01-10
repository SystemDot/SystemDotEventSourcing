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
                .AllCommitsFrom(DateTime.MinValue)
                .SelectMany(c => c.Events)
                .ToList();
        }
    }
}
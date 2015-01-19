using System.Collections.Generic;

namespace SystemDot.EventSourcing.Streams
{
    public class SourcedEvent
    {
        public Dictionary<string, object> Headers { get; private set; }

        public object Body { get; set; }

        public SourcedEvent()
        {
            Headers = new Dictionary<string, object>();
        }
    }
}
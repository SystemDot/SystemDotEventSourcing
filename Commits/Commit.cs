using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Commits
{
    using SystemDot.Core;

    public class Commit
    {
        public Guid CommitId { get; private set; }
        
        public string StreamId { get; private set; }

        public IEnumerable<SourcedEvent> Events { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public IDictionary<string, object> Headers { get; private set; }

        public T GetHeader<T>(string key)
        {
            return Headers[key].As<T>();
        }

        public Commit(Guid commitId, string streamId, IEnumerable<SourcedEvent> events, IDictionary<string, object> headers)
        {
            CommitId = commitId;
            StreamId = streamId;
            Events = events;
            CreatedOn = DateTime.Now;
            Headers = headers;
        }
    }
}
using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Commits
{
    public class Commit
    {
        public Guid CommitId { get; private set; }
        
        public string StreamId { get; private set; }

        public IEnumerable<SourcedEvent> Events { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public Commit(Guid commitId, string streamId, IEnumerable<SourcedEvent> events)
        {
            CommitId = commitId;
            StreamId = streamId;
            Events = events;
            CreatedOn = DateTime.Now;
        }
    }
}
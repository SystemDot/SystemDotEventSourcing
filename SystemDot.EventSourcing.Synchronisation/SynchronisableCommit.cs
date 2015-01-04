using System;
using System.Collections.Generic;

namespace SystemDot.EventSourcing.Synchronisation
{
    public class SynchronisableCommit
    {
        public Guid CommitId { get; set; }
        public string StreamId { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<SynchronisableSourcedEvent> Events { get; set; }
    }
}
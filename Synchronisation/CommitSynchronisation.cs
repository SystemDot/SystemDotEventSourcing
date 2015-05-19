namespace SystemDot.EventSourcing.Synchronisation
{
    using System.Collections.Generic;

    public class CommitSynchronisation
    {
        public List<SynchronisableCommit> Commits { get; set; }
    }
}
namespace SystemDot.EventSourcing.Synchronisation.Server
{
    using System.Collections.Generic;

    public class CommitSynchronisableCommits
    {
        public IEnumerable<SynchronisableCommit> Commits { get; set; }
    }
}
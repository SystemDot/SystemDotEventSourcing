using SystemDot.EventSourcing.Commits;

namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps
{
    public class CommitContext
    {
        public Commit CommitInUse { get; set; }
    }
}
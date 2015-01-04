using SystemDot.EventSourcing.Commits;

namespace SystemDot.Domain.Synchronisation.Specifications
{
    public class CommitContext
    {
        public Commit CommitInUse { get; set; }
    }
}
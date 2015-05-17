namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Commits
{
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Sessions;

    public class CommitContext
    {
        public Commit CommitInUse { get; set; }
        public IEventSessionFactory EventSessionFactory { get; set; }
    }
}
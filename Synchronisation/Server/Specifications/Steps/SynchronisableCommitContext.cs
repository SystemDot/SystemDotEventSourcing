namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps
{
    using Microsoft.Owin.Testing;

    public class SynchronisableCommitContext
    {
        public SynchronisableCommit CommitInUse { get; set; }
        public TestServer TestServer { get; set; }
    }
}
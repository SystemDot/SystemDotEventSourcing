namespace SystemDot.EventSourcing.Synchronisation.Client
{
    using System;

    public class CommitSynchronisationResult
    {
        public DateTime LastPullCommitDate { get; private set; }
        public DateTime LastPushCommitDate { get; private set; }

        public CommitSynchronisationResult(DateTime lastPullCommitDate, DateTime lastPushCommitDate)
        {
            LastPullCommitDate = lastPullCommitDate;
            LastPushCommitDate = lastPushCommitDate;
        }
    }
}
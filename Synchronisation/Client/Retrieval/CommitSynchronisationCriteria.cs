using System;

namespace SystemDot.EventSourcing.Synchronisation.Client.Retrieval
{
    public class CommitSynchronisationCriteria
    {
        public DateTime PullCommitsFrom { get; private set; }
        public DateTime PushCommitsFrom { get; private set; }

        public string ClientId { get; private set; }

        public CommitSynchronisationCriteria(DateTime pullCommitsFrom, DateTime pushCommitsFrom, string clientId)
        {
            PushCommitsFrom = pushCommitsFrom;
            PullCommitsFrom = pullCommitsFrom;
            ClientId = clientId;
        }
    }
}
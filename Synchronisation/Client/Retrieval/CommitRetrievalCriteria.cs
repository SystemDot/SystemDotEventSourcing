using System;

namespace SystemDot.EventSourcing.Synchronisation.Client.Retrieval
{
    public class CommitRetrievalCriteria
    {
        public DateTime GetCommitsFrom { get; private set; }

        public string ClientId { get; private set; }

        public CommitRetrievalCriteria(DateTime getCommitsFrom, string clientId)
        {
            GetCommitsFrom = getCommitsFrom;
            ClientId = clientId;
        }
    }
}
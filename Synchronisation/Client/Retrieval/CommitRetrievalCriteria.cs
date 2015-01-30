using System;

namespace SystemDot.EventSourcing.Synchronisation.Client.Retrieval
{
    public class CommitRetrievalCriteria
    {
        public Uri ServerUri { get; private set; }

        public DateTime GetCommitsFrom { get; private set; }

        public string ClientId { get; private set; }

        public CommitRetrievalCriteria(Uri serverUri, DateTime getCommitsFrom, string clientId)
        {
            ServerUri = serverUri;
            GetCommitsFrom = getCommitsFrom;
            ClientId = clientId;
        }
    }
}
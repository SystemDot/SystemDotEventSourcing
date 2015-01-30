using System;

namespace SystemDot.EventSourcing.Synchronisation.Client.Retrieval
{
    public class SynchronisationUri : Uri
    {
        SynchronisationUri(Uri serverUri, string relativeUri) : base(serverUri, relativeUri)
        {
        }

        public static SynchronisationUri Parse(Uri serverUri, string clientId, long @fromCommitInTicks)
        {
            return new SynchronisationUri(serverUri, string.Format("Synchronisation/{0}/{1}", clientId, @fromCommitInTicks));
        }
    }
}
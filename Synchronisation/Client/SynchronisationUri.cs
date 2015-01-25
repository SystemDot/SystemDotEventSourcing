using System;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class SynchronisationUri : Uri
    {
        SynchronisationUri(Uri serverUri, string format) : base(serverUri, format)
        {
        }

        public static SynchronisationUri Parse(Uri serverUri, long @fromCommitInTicks)
        {
            return new SynchronisationUri(serverUri, string.Format("Synchronisation/{0}", @fromCommitInTicks));
        }
    }
}
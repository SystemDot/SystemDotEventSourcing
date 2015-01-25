using System;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class SynchronisationUri : Uri
    {
        SynchronisationUri(Uri serverUri, string format) : base(serverUri, format)
        {
        }

        public static SynchronisationUri Parse(Uri serverUri, DateTime @from)
        {
            return new SynchronisationUri(serverUri, string.Format("Synchronisation/{0}", @from));
        }
    }
}
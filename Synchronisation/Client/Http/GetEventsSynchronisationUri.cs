namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    using System;

    public class GetEventsSynchronisationUri : Uri
    {
        GetEventsSynchronisationUri(Uri serverUri, string relativeUri) : base(serverUri, relativeUri)
        {
        }

        public static GetEventsSynchronisationUri Parse(Uri serverUri, string clientId, long @fromCommitInTicks)
        {
            return new GetEventsSynchronisationUri(serverUri, string.Format("Synchronisation/{0}/{1}", clientId, @fromCommitInTicks));
        }
    }
}
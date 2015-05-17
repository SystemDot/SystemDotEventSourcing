namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    using System;

    public class PostEventsSynchronisationUri : Uri
    {
        PostEventsSynchronisationUri(Uri serverUri, string relativeUri) : base(serverUri, relativeUri)
        {
        }

        public static PostEventsSynchronisationUri Parse(Uri serverUri)
        {
            return new PostEventsSynchronisationUri(serverUri, "Synchronisation");
        }
    }
}
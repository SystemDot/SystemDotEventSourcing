namespace SystemDot.EventSourcing.Synchronisation.Client.Http
{
    using System;

    public class SynchronisationServerUriProvider
    {
        public Uri ServerUri { get; private set; }

        public SynchronisationServerUriProvider(Uri serverUri)
        {
            ServerUri = serverUri;
        }
    }
}
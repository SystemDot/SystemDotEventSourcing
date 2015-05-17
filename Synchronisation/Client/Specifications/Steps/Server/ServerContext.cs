namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Server
{
    using System;

    public class ServerContext
    {
        public Uri ServerUri { get; set; }
        public TestSynchronisationHttpClient SynchronisationHttpClient { get; set; }
    }
}
namespace SystemDot.EventSourcing.Synchronisation.Client.Messages
{
    public class ClientSynchronisationProcessInitialised
    {
        public string ServerUri { get; set; }
        public string ClientId { get; set; }
    }
}
namespace SystemDot.EventSourcing.Synchronisation.Messages
{
    public class InitialiseClientSynchronisationProcess
    {
        public string ClientId { get; set; }
        public string ServerUri { get; set; }
    }
}
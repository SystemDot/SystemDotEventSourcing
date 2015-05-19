namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps
{
    using SystemDot.EventSourcing.Sessions;

    public class EventSessionContext
    {
        public IEventSessionFactory SessionFactory { get; set; }
    }
}
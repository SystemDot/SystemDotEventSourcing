namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Handling
{
    using SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps;

    public class EventHandlerContext
    {
        public TestEvent LastEvent { get; set; }
    }
}
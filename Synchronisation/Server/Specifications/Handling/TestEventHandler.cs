namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Handling
{
    using System.Threading.Tasks;
    using SystemDot.Domain.Events;
    using SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps;

    public class TestEventHandler : IAsyncEventHandler<TestEvent>
    {
        readonly EventHandlerContext context;

        public TestEventHandler(EventHandlerContext context)
        {
            this.context = context;
        }

        public Task Handle(TestEvent message)
        {
            context.LastEvent = message;
            return Task.FromResult(false);
        }
    }
}
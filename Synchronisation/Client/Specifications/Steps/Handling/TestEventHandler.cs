namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Handling
{
    using System.Threading.Tasks;
    using SystemDot.Domain.Events;

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
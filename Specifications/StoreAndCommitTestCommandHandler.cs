using SystemDot.Environment;
using SystemDot.EventSourcing.Handlers;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Specifications
{
    public class StoreAndCommitTestCommandHandler : StoreAndCommitEventCommandHandler<TestCommand, TestStoreAndCommitEvent>
    {
        public StoreAndCommitTestCommandHandler(IEventSessionFactory eventSessionFactory, ILocalMachine localMachine)
            : base(eventSessionFactory, localMachine)
        {
        }

        protected override TestStoreAndCommitEvent CreateEventFromCommand(TestCommand command)
        {
            return new TestStoreAndCommitEvent {Id = command.Id};
        }

        protected override string CreateEventIdFromCommand(TestCommand command)
        {
            return command.Id;
        }
    }
}
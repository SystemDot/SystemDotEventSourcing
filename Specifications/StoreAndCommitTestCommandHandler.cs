using SystemDot.Environment;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Handlers;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Domain;

    public class StoreAndCommitTestCommandHandler : StoreAndCommitEventCommandHandler<TestCommand, TestStoreAndCommitEvent>
    {
        public StoreAndCommitTestCommandHandler(IEventSessionFactory eventSessionFactory, ILocalMachine localMachine)
            : base(eventSessionFactory, localMachine)
        {
        }

        protected override TestStoreAndCommitEvent CreateEventFromCommand(TestCommand command, MultiSiteId id)
        {
            return new TestStoreAndCommitEvent { Id = id.Id };
        }

        protected override MultiSiteId CreateAggregateRootIdFromCommand(TestCommand command)
        {
            return new TestAggregateRootId(command.Id, command.BucketId);
        }
    }
}
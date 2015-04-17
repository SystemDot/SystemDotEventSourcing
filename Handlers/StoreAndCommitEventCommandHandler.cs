using System.Threading.Tasks;
using SystemDot.Environment;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Handlers
{
    using SystemDot.Domain;

    public abstract class StoreAndCommitEventCommandHandler<TCommand, TEvent>
        where TEvent : new()
    {
        readonly IEventSessionFactory eventSessionFactory;
        readonly ILocalMachine localMachine;

        protected StoreAndCommitEventCommandHandler(IEventSessionFactory eventSessionFactory, ILocalMachine localMachine)
        {
            this.eventSessionFactory = eventSessionFactory;
            this.localMachine = localMachine;
        }

        public Task Handle(TCommand command)
        {
            MultiSiteId id = CreateAggregateRootIdFromCommand(command);

            eventSessionFactory
                .Create().StoreEventAndCommit(
                    CreateEventFromCommand(command, id), 
                    id.ToEventStreamId(), 
                    localMachine);

            return Task.FromResult(false);
        }

        protected abstract TEvent CreateEventFromCommand(TCommand command, MultiSiteId id);

        protected abstract MultiSiteId CreateAggregateRootIdFromCommand(TCommand command);
    }
}
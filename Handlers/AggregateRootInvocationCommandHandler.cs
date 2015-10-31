namespace SystemDot.EventSourcing.Handlers
{
    using System.Threading.Tasks;
    using SystemDot.Core;
    using SystemDot.Domain.Commands;
    using SystemDot.EventSourcing.Aggregation;

    public class AggregateRootInvocationCommandHandler<TCommand, TAggregateRoot> : IAsyncCommandHandler<TCommand>
        where TAggregateRoot : AggregateRoot, new()
    {
        readonly IDomainRepository domainRepository;
        readonly IFindAggregates<TCommand> finder;

        public AggregateRootInvocationCommandHandler(IDomainRepository domainRepository, IFindAggregates<TCommand> finder)
        {
            this.domainRepository = domainRepository;
            this.finder = finder;
        }

        public Task Handle(TCommand message)
        {
            var aggregateRoot = domainRepository.Get<TAggregateRoot>(finder.GetIdFromCommandToFindAggregateWith(message));
            aggregateRoot.As<IInvokeAggregate<TCommand>>().When(message);
            domainRepository.Save(aggregateRoot);
            return Task.FromResult(false);
        }
    }
}
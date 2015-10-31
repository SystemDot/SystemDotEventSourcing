namespace SystemDot.EventSourcing.Bootstrapping
{
    using SystemDot.Bootstrapping;
    using SystemDot.Domain.Commands;
    using SystemDot.EventSourcing.Aggregation;
    using SystemDot.EventSourcing.Handlers;

    public class ForCommandAggregateInvocationConfiguration<TCommand> : AggregateInvocationConfiguration
    {
        public ForCommandAggregateInvocationConfiguration(BootstrapBuilderConfiguration @from)
            : base(@from)
        {
        }

        public InvokeOnAggregateRootInvocationConfiguration<TCommand> InvokeOn<TAggregateRoot>()
            where TAggregateRoot : AggregateRoot, IInvokeCommands<TCommand>, new()
        {
            RegisterBuildAction(c => c.RegisterInstance<IAsyncCommandHandler<TCommand>, AggregateRootInvocationCommandHandler<TCommand, TAggregateRoot>>());
            return new InvokeOnAggregateRootInvocationConfiguration<TCommand>(this);
        }
    }
}
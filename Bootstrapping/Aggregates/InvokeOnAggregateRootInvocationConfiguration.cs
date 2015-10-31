namespace SystemDot.EventSourcing.Bootstrapping.Aggregates
{
    using SystemDot.Bootstrapping;
    using SystemDot.EventSourcing.Aggregation;

    public class InvokeOnAggregateRootInvocationConfiguration<TCommand> : ForCommandAggregateInvocationConfiguration<TCommand>
    {
        public InvokeOnAggregateRootInvocationConfiguration(BootstrapBuilderConfiguration @from)
            : base(@from)
        {
        }

        public ForCommandAggregateInvocationConfiguration<TCommand> FindUsing<TFinder>() where TFinder : class, IFindAggregates<TCommand>
        {
            RegisterBuildAction(c => c.RegisterInstance<IFindAggregates<TCommand>, TFinder>());
            return this;
        }
    }
}
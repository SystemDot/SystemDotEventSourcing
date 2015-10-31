namespace SystemDot.EventSourcing.Bootstrapping
{
    using SystemDot.Bootstrapping;

    public class AggregateInvocationConfiguration : BootstrapBuilderConfiguration
    {
        public AggregateInvocationConfiguration(BootstrapBuilderConfiguration @from)
            : base(@from)
        {
        }

        public ForCommandAggregateInvocationConfiguration<TCommand> For<TCommand>()
        {
            return new ForCommandAggregateInvocationConfiguration<TCommand>(this);
        }
    }
}
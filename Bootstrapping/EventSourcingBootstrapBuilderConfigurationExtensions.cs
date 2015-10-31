namespace SystemDot.EventSourcing.Bootstrapping
{
    using SystemDot.Bootstrapping;

    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static AggregateInvocationConfiguration ConfigureAggregateInvocation(this BootstrapBuilderConfiguration config)
        {
            return new AggregateInvocationConfiguration(config);
        }
    }
}
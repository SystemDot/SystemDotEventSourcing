namespace SystemDot.EventSourcing.Bootstrapping
{
    using SystemDot.Bootstrapping;
    using SystemDot.Domain.Events;
    using SystemDot.EventSourcing.Bootstrapping.Aggregates;
    using SystemDot.EventSourcing.Bootstrapping.Sagas;

    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static AggregateInvocationConfiguration ConfigureAggregateInvocation(this BootstrapBuilderConfiguration config)
        {
            return new AggregateInvocationConfiguration(config);
        }
        
        public static SagaInvocationConfiguration ConfigureSagaInvocation(this BootstrapBuilderConfiguration config)
        {
            return new SagaInvocationConfiguration(config);
        }
    }
}
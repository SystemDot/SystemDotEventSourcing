namespace SystemDot.EventSourcing.Bootstrapping.Sagas
{
    using SystemDot.Bootstrapping;
    using SystemDot.Domain.Events;
    using SystemDot.EventSourcing.Handlers;

    public class SagaInvocationConfiguration : BootstrapBuilderConfiguration
    {
        public SagaInvocationConfiguration(BootstrapBuilderConfiguration @from)
            : base(@from)
        {
        }

        public ForEventSagaInvocationConfiguration<TEvent> For<TEvent>()
        {
            RegisterBuildAction(c => c.RegisterInstance<IAsyncEventHandler<TEvent>, SagaInvocationEventHandler<TEvent>>());
            return new ForEventSagaInvocationConfiguration<TEvent>(this);
        }
    }
}
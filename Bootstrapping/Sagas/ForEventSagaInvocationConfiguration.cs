namespace SystemDot.EventSourcing.Bootstrapping.Sagas
{
    using SystemDot.Bootstrapping;
    using SystemDot.EventSourcing.Handlers;
    using SystemDot.EventSourcing.Sagas;

    public class ForEventSagaInvocationConfiguration<TEvent> : SagaInvocationConfiguration
    {
        public ForEventSagaInvocationConfiguration(BootstrapBuilderConfiguration @from)
            : base(@from)
        {
        }

        public InvokeOnSagaInvocationConfiguration<TEvent> InvokeOn<TSaga>()
            where TSaga : Saga, IContinueSaga<TEvent>, new()
        {
            RegisterBuildAction(c => c.Resolve<SagaLoaderCollection<TEvent>>().RegisterSaga(c.Resolve<SagaLoader<TEvent, TSaga>>()), BuildOrder.Late);
            return new InvokeOnSagaInvocationConfiguration<TEvent>(this);
        }
    }
}
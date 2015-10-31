namespace SystemDot.EventSourcing.Bootstrapping.Sagas
{
    using SystemDot.Bootstrapping;
    using SystemDot.EventSourcing.Aggregation;
    using SystemDot.EventSourcing.Sagas;

    public class InvokeOnSagaInvocationConfiguration<TEvent> : ForEventSagaInvocationConfiguration<TEvent>
    {
        public InvokeOnSagaInvocationConfiguration(BootstrapBuilderConfiguration @from)
            : base(@from)
        {
        }

        public ForEventSagaInvocationConfiguration<TEvent> FindUsing<TFinder>() where TFinder : class, IFindSagas<TEvent>
        {
            RegisterBuildAction(c => c.RegisterInstance<IFindSagas<TEvent>, TFinder>());
            return this;
        }
    }
}
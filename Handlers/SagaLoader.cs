namespace SystemDot.EventSourcing.Handlers
{
    using SystemDot.EventSourcing.Sagas;

    public class SagaLoader<TEvent, TSaga> : ISagaLoader<TEvent>
        where TSaga : Saga, new()
    {
        readonly IDomainRepository repository;
        readonly IFindSagas<TEvent> sagaFinder;

        public SagaLoader(IDomainRepository repository, IFindSagas<TEvent> sagaFinder)
        {
            this.repository = repository;
            this.sagaFinder = sagaFinder;
        }

        public Saga Load(TEvent toLoad)
        {
            return repository.Get<TSaga>(sagaFinder.GetIdFromEventToFindSagaWith(toLoad));
        }
    }
}
namespace SystemDot.EventSourcing.Handlers
{
    using SystemDot.EventSourcing.Sagas;

    public interface ISagaLoader<in TEvent>
    {
        Saga Load(TEvent toLoad);
    }
}
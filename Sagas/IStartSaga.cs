namespace SystemDot.EventSourcing.Sagas
{
    public interface IStartSaga<in TEvent> : IContinueSaga<TEvent>
    {
    }
}
namespace SystemDot.EventSourcing.Sagas
{
    public interface IContinueSaga<in TEvent>
    {
        void When(TEvent command);
    }
}
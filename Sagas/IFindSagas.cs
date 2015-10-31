namespace SystemDot.EventSourcing.Sagas
{
    using SystemDot.Domain;

    public interface IFindSagas<in TEvent>
    {
        MultiSiteId GetIdFromEventToFindSagaWith(TEvent message);
    }
}
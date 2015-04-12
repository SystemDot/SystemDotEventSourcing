namespace SystemDot.EventSourcing.Projections
{
    public interface IDataStoreProjection<in T>
    {
        void Handle(T message);
    }
}
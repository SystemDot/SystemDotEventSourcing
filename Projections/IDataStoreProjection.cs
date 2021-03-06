namespace SystemDot.EventSourcing.Projections
{
    using System.Threading.Tasks;

    public interface IDataStoreProjection<in T>
    {
        Task Handle(T message);
    }
}
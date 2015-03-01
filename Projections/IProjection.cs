namespace SystemDot.EventSourcing.Projections
{
    using System.Threading.Tasks;

    public interface IProjection<in T>
    {
        Task Handle(T message);
    }
}
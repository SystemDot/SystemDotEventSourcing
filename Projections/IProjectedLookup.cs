namespace SystemDot.EventSourcing.Projections
{
    using System.Threading.Tasks;

    public interface IProjectedLookup<in T>
    {
        Task Handle(T message);
    }
}
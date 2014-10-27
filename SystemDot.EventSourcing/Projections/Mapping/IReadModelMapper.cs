using SystemDot.Domain.Events;

namespace SystemDot.EventSourcing.Projections.Mapping
{
    public interface IReadModelMapper<in T> : IAsyncEventHandler<T>
    {
    }
}
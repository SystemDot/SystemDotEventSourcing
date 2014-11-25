using SystemDot.Domain.Events;

namespace SystemDot.EventSourcing.Projections
{
    public interface IProjection<in T> : IAsyncEventHandler<T>
    {
    }
}
namespace SystemDot.EventSourcing.Projections
{
    using SystemDot.Domain.Events;

    public interface IProjection<in T> : IAsyncEventHandler<T>
    {
    }
}
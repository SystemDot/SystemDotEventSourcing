namespace SystemDot.EventSourcing.Projections
{
    using Domain.Events;

    public interface IProjection<in T> : IAsyncEventHandler<T>
    {
    }
}
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Projections
{
    public abstract class Projection<T> : IProjection<T>
    {
        public async Task Handle(T @event)
        {
            await MapAsync(@event);
        }

        protected abstract Task MapAsync (T @event);
    }
}
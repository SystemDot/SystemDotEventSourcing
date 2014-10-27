using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Projections.Mapping
{
    public abstract class ReadModelMapper<T> : IReadModelMapper<T>
    {
        public async Task Handle(T @event)
        {
            await MapAsync(@event);
        }

        protected abstract Task MapAsync (T @event);
    }
}
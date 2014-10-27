using System.Threading.Tasks;
using SystemDot.Domain.Commands;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing
{
    public class EventSessionAsyncCommandHandler<T> : IAsyncCommandHandler<T>
    {
        readonly IEventSessionFactory eventSessionFactory;
        readonly IAsyncCommandHandler<T> decorated;

        public EventSessionAsyncCommandHandler(
            IEventSessionFactory eventSessionFactory,
            IAsyncCommandHandler<T> decorated)
        {
            this.eventSessionFactory = eventSessionFactory;
            this.decorated = decorated;
        }

        public async Task Handle(T message)
        {
            using (var session = eventSessionFactory.Create())
            {
                await decorated.Handle(message);
                await session.CommitAsync();
            } 
        }
    }
}
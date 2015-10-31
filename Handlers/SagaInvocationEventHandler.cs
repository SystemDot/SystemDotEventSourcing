namespace SystemDot.EventSourcing.Handlers
{
    using System.Linq;
    using System.Threading.Tasks;
    using SystemDot.Core;
    using SystemDot.Domain.Commands;
    using SystemDot.Domain.Events;
    using SystemDot.EventSourcing.Sagas;

    public class SagaInvocationEventHandler<TEvent> : IAsyncEventHandler<TEvent>
    {
        readonly ICommandBus commandBus;
        readonly SagaLoaderCollection<TEvent> loaders;

        public SagaInvocationEventHandler(ICommandBus commandBus, SagaLoaderCollection<TEvent> loaders)
        {
            this.commandBus = commandBus;
            this.loaders = loaders;
        }

        public async Task Handle(TEvent message)
        {
            Saga saga = loaders.Select(l => l.Load(message)).Where(l => l.StartedAt > 0).OrderBy(l => l.StartedAt).LastOrDefault();

            if (saga == null)
            {
                return;
            }

            saga.As<IContinueSaga<TEvent>>().When(message);

            foreach (var command in saga.SentCommands)
            {
                await commandBus.SendCommandAsync(command);
            }
        }
    }
}
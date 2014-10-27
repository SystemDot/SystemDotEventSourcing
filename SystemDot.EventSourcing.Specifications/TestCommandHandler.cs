using System.Threading.Tasks;
using SystemDot.Domain.Commands;

namespace SystemDot.EventSourcing.Specifications
{
    public class TestCommandHandler : IAsyncCommandHandler<TestCommand>
    {
        public Task Handle(TestCommand message)
        {
            TestAggregateRoot.Create(message.Id);
            return Task.FromResult(false);
        }
    }
}
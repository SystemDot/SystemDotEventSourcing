using System.Threading.Tasks;
using SystemDot.Domain.Commands;

namespace SystemDot.EventSourcing.Specifications
{
    public class TestCommandHandler : IAsyncCommandHandler<TestCommand>
    {
        readonly IDomainRepository domainRepository;

        public TestCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public Task Handle(TestCommand message)
        {
            domainRepository.Save(TestAggregateRoot.Create(new TestAggregateRootId(message.Id, message.BucketId)));
            return Task.FromResult(false);
        }
    }
}
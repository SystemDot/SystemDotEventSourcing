using SystemDot.Bootstrapping;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.Ioc;
using FluentAssertions;
using Machine.Specifications;
using SystemDot.Environment;

namespace SystemDot.EventSourcing.Specifications
{

    public class when_saving_an_aggregate_root_and_getting_it_back_again_via_repository
    {
        private const string Id = "Id";
        private const string BucketId = "BucketId";
        
        static IDomainRepository repository;
        static TestAggregateRoot root;
        static IEventSessionFactory eventSessionFactory;

        Establish context = () =>
        {
            IIocContainer container = new IocContainer();

            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToMemory()
                .Initialise();

            repository = container.Resolve<IDomainRepository>();
            eventSessionFactory = container.Resolve<IEventSessionFactory>();

            var root = TestAggregateRoot.Create(new TestAggregateRootId(Id, BucketId));
            root.SetSomeMoreStateResultingInEvent();
            repository.Save(root);
        };

        Because of = () =>
        {
            using (eventSessionFactory.Create())
            {
                root = repository.Get<TestAggregateRoot>(new TestAggregateRootId(Id, BucketId));
            }
        };

        It should_have_hydrated_the_root_with_the_first_event_with_the_correct_id = () => root.Id.Should().Be(Id);

        It should_have_hydrated_the_root_with_the_first_event_the_correct_bucket_id = () => root.SiteId.Should().Be(BucketId);

        It should_have_hydrated_the_root_with_the_state_from_the_second_event = () => root.State.Should().BeTrue();
    }
}
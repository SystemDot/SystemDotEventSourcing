namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Bootstrapping;
    using SystemDot.Domain;
    using SystemDot.Domain.Bootstrapping;
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Bootstrapping;
    using SystemDot.EventSourcing.InMemory.Bootstrapping;
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.Ioc;
    using FluentAssertions;
    using Machine.Specifications;

    public class when_saving_a_stateful_aggregate_root_and_getting_it_back_again_via_repository
    {
        private const string Id = "Id";
        private const string BucketId = "BucketId";

        static IDomainRepository repository;
        static TestStatefulAggregateRoot root;
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

            var root = TestStatefulAggregateRoot.Create(new TestAggregateRootId(Id, BucketId));
            root.SetSomeMoreStateResultingInEvent();
            repository.Save(root);
        };

        Because of = () =>
        {
            using (eventSessionFactory.Create())
            {
                root = repository.Get<TestStatefulAggregateRoot>(new TestAggregateRootId(Id, BucketId));
            }
        };

        It should_have_hydrated_the_root_with_the_state_from_the_second_event = () => root.SomeState.Should().BeTrue();
    }
}
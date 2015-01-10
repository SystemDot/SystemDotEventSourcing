using System;
using SystemDot.Bootstrapping;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.Ioc;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.EventSourcing.Specifications
{
    public class when_putting_events_in_the_session_via_an_aggegate_root_and_getting_it_back_again_via_repository
    {
        const string Id = "Id";

        static IDomainRepository repository;
        static TestAggregateRoot root;
        static IEventSessionFactory eventSessionFactory;

        Establish context = () =>
        {
            IIocContainer container = new IocContainer();

            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToMemory()
                .Initialise();

            repository = container.Resolve<IDomainRepository>();
            eventSessionFactory = container.Resolve<IEventSessionFactory>();

            using (var session = eventSessionFactory.Create())
            {
                var root = TestAggregateRoot.Create(Id);
                root.SetSomeMoreStateResultingInEvent();
                session.Commit(Guid.NewGuid());
            }   
        };

        Because of = () =>
        {
            using (eventSessionFactory.Create())
            {
                root = repository.Get<TestAggregateRoot>(Id);
            }
        };

        It should_have_hydrated_the_root_with_the_first_event = () => root.Id.Should().Be(Id);

        It should_have_hydrated_the_root_with_the_state_from_the_second_event = () => root.State.Should().BeTrue();
    }
}
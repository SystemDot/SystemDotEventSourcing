using System;
using System.Linq;
using SystemDot.Bootstrapping;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Environment;
using SystemDot.EventSourcing;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.Headers;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.Ioc;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.EventSourcing.Specifications
{
    public class when_saving_an_aggregate_root_via_the_repository
    {
        private const string Id = "Id";
        private const string BucketId = "BucketId";
        
        static IDomainRepository repository;
        static IEventSessionFactory eventSessionFactory;
        static ILocalMachine localMachine;

        Establish context = () =>
        {
            IIocContainer container = new IocContainer();
            container.RegisterInstance<ILocalMachine, LocalMachine>();

            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToMemory()
                .Initialise();

            repository = container.Resolve<IDomainRepository>();
            eventSessionFactory = container.Resolve<IEventSessionFactory>();
            localMachine = container.Resolve<ILocalMachine>();
        };

        Because of = () => repository.Save(TestAggregateRoot.Create(new TestAggregateRootId(Id, BucketId)));

        It should_associate_any_events_with_a_header_specifying_the_name_of_the_machine_the_event_originated_from = () =>
        {
            using (var eventSession = eventSessionFactory.Create())
            {
                eventSession.AllCommits()
                    .Single()
                    .Headers["Origin"]
                    .As<EventOriginHeader>().MachineName.Should().Be(localMachine.GetName());
            }
        };
    }
}
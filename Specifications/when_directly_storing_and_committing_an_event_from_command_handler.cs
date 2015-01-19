using System;
using System.Linq;
using SystemDot.Bootstrapping;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Environment;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.Headers;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.Ioc;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.EventSourcing.Specifications
{
    public class when_directly_storing_and_committing_an_event_from_command_handler
    {
        static IEventSessionFactory eventSessionFactory;
        static ILocalMachine localMachine;
        static StoreAndCommitTestCommandHandler handler;
        static TestCommand testCommand;

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

            eventSessionFactory = container.Resolve<IEventSessionFactory>();
            localMachine = container.Resolve<ILocalMachine>();
            handler = new StoreAndCommitTestCommandHandler(eventSessionFactory, localMachine);

            testCommand = new TestCommand {Id = "1"};
        };

        Because of = () => handler.Handle(testCommand);

        It should_associate_any_events_with_a_header_specifying_the_name_of_the_machine_the_event_originated_from = () =>
        {
            using (var eventSession = eventSessionFactory.Create())
            {
                eventSession.AllCommitsFrom(DateTime.MinValue)
                    .Single()
                    .Headers["Origin"]
                    .As<EventOriginHeader>().MachineName.Should().Be(localMachine.GetName());
            }
        };

        It should_place_the_appropriate_event_in_the_session = () =>
        {
            using (var eventSession = eventSessionFactory.Create())
            {
                eventSession.AllCommitsFrom(DateTime.MinValue)
                    .Single(e => e.StreamId == testCommand.Id)
                    .Events.Single()
                    .Body.As<TestStoreAndCommitEvent>().Id.Should().Be(testCommand.Id);
            }
        };

    }
}
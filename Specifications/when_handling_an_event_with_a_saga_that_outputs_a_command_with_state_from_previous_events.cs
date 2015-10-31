namespace SystemDot.EventSourcing.Specifications
{
    using System;
    using SystemDot.Bootstrapping;
    using SystemDot.Domain;
    using SystemDot.Domain.Bootstrapping;
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Bootstrapping;
    using SystemDot.EventSourcing.InMemory.Bootstrapping;
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Ioc;
    using SystemDot.Messaging.Simple;
    using FluentAssertions;
    using Machine.Specifications;

    public class when_handling_an_event_with_a_saga_that_outputs_a_command_with_state_from_previous_events
    {
        private const string Id = "Id";
        private const string BucketId = "BucketId";
        private const string State = "State";
        static IIocContainer container;
        static TestSagaGeneratedCommandWithState handledCommand;
        static IEventSession session;

        Establish context = () =>
        {
            container = new IocContainer();

            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing()
                .PersistToMemory()
                .ConfigureSagaInvocation()
                .For<TestStartSagaEvent>().InvokeOn<TestSaga>().FindUsing<TestSagaFinder>()
                .For<TestSagaEvent>().InvokeOn<TestSaga>().FindUsing<TestSagaFinder>()
                .Initialise();

            container.Resolve<Dispatcher>().RegisterHandler<TestSagaGeneratedCommandWithState>(e => handledCommand = e);

            session = container.Resolve<IEventSessionFactory>().Create();
            session.StoreEvent(new SourcedEvent { Body = new TestStartSagaEvent { Id = Id, SiteId = BucketId, State = State }, }, new EventStreamId(Id, BucketId));
            session.Commit(Guid.NewGuid());
        };

        Because of = () =>
        {
            session.StoreEvent(new SourcedEvent { Body = new TestSagaEvent { Id = Id, SiteId = BucketId }, }, new EventStreamId(Id, BucketId));
            session.Commit(Guid.NewGuid());
        };

        It should_send_the_command_with_the_expected_state = () => handledCommand.State.Should().Be(State);
    }
}
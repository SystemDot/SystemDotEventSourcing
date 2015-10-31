namespace SystemDot.EventSourcing.Specifications
{
    using System;
    using SystemDot.Bootstrapping;
    using SystemDot.Domain;
    using SystemDot.Domain.Bootstrapping;
    using SystemDot.Domain.Commands;
    using SystemDot.Domain.Events;
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Bootstrapping;
    using SystemDot.EventSourcing.InMemory.Bootstrapping;
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Ioc;
    using SystemDot.Messaging.Simple;
    using FluentAssertions;
    using Machine.Specifications;

    public class when_handling_an_event_with_a_saga_that_outputs_a_command
    {
        private const string Id = "Id";
        private const string BucketId = "BucketId";
        static IIocContainer container;
        static TestSagaGeneratedCommand handledCommand;
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
                .Initialise();

            container.Resolve<Dispatcher>().RegisterHandler<TestSagaGeneratedCommand>(e => handledCommand = e);
            session = container.Resolve<IEventSessionFactory>().Create();
        };

        Because of = () =>
        {
            session.StoreEvent(new SourcedEvent { Body = new TestStartSagaEvent { Id = Id, SiteId = BucketId }, }, new EventStreamId(Id, BucketId));
            session.Commit(Guid.NewGuid());
        };

        It should_send_the_command = () => handledCommand.Id.Should().Be(Id);
    }
}
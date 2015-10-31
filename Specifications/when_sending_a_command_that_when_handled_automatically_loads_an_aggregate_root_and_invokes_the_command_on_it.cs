namespace SystemDot.EventSourcing.Specifications
{
    using System.Linq;
    using System.Threading.Tasks;
    using SystemDot.Bootstrapping;
    using SystemDot.Domain;
    using SystemDot.Domain.Bootstrapping;
    using SystemDot.Domain.Commands;
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Aggregation;
    using SystemDot.EventSourcing.Bootstrapping;
    using SystemDot.EventSourcing.InMemory.Bootstrapping;
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Ioc;
    using SystemDot.Messaging.Simple;
    using FluentAssertions;
    using Machine.Specifications;

    public class when_sending_a_command_that_when_handled_automatically_loads_an_aggregate_root_and_invokes_the_command_on_it
    {
        private const string Id = "Id";
        private const string BucketId = "BucketId";
        static IIocContainer container;
        static TestAggregateRootCreatedEvent handledEvent;

        Establish context = () =>
        {
            container = new IocContainer();

            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing()
                .PersistToMemory()
                .ConfigureAggregateInvocation()
                    .For<TestCommand>()
                    .InvokeOn<TestCommandInvokedAggregateRoot>()
                    .FindUsing<TestCommandInvokedAggregateRootFinder>()
                .Initialise();

            container.Resolve<Dispatcher>().RegisterHandler<TestAggregateRootCreatedEvent>(e => handledEvent = e);
        };

        Because of = () => container.Resolve<IAsyncCommandHandler<TestCommand>>()
            .Handle(new TestCommand { Id = Id, BucketId = BucketId })
            .Wait();

        It should_put_the_sourced_event_in_the_session_with_the_event_as_its_session = () =>
            container.Resolve<IEventSessionFactory>().Create().GetEvents(new EventStreamId(Id, BucketId)).Single()
                .Body.As<TestAggregateRootCreatedEvent>().Id.Should().Be(Id);

        It should_send_the_event = () => handledEvent.Id.Should().Be(Id);
    }
}
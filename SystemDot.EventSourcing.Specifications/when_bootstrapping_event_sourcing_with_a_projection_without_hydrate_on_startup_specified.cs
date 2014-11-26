namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Bootstrapping;
    using SystemDot.Domain;
    using SystemDot.Domain.Bootstrapping;
    using SystemDot.EventSourcing.Bootstrapping;
    using SystemDot.EventSourcing.InMemory;
    using SystemDot.EventSourcing.InMemory.Bootstrapping;
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.Ioc;
    using FluentAssertions;
    using Machine.Specifications;

    public class when_bootstrapping_event_sourcing_with_a_projection_without_hydrate_on_startup_specified
    {
        private const string Id = "Id";
        private static IIocContainer container;
        private static TestProjection projection;

        private Establish context = async () =>
        {
            container = new IocContainer();

            projection = new TestProjection();
            container.RegisterInstance(() => projection);

            var session = new InMemoryEventSession(new NullEventDispatcher());
            container.RegisterInstance(() => session);

            session.StoreEvent(new SourcedEvent { Body = new TestAggregateRootCreatedEvent { Id = Id }, }, Id);
            await session.CommitAsync();
        };

        Because of = async () =>
        {
            await Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToMemory()
                .InitialiseAsync();
        };

        It should_have_hydrated_the_projection = () => projection.IdFromEvent.Should().Be(null);
    }
}
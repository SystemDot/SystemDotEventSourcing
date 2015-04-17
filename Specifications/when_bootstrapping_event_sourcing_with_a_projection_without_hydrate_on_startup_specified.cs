using System;
using SystemDot.Domain.Events.Dispatching;
using SystemDot.Environment;
using SystemDot.EventSourcing.Streams;
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

namespace SystemDot.EventSourcing.Specifications
{
    public class when_bootstrapping_event_sourcing_with_a_projection_without_hydrate_on_startup_specified
    {
        private const string Id = "Id";
        private const string BucketId = "SiteId";
        private static IIocContainer container;

        private Establish context = () =>
        {
            container = new IocContainer();
            container.RegisterInstance<IEventSessionFactory>(() => new EventSessionFactory(new InMemoryEventStore(new NullEventDispatcher())));

            IEventSession session = container.Resolve<IEventSessionFactory>().Create();
            session.StoreEvent(new SourcedEvent { Body = new TestAggregateRootCreatedEvent { Id = Id }, }, new EventStreamId(Id, BucketId));
            session.Commit(Guid.NewGuid());
        };

        Because of = async () =>
        {
            await Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToMemory()
                .InitialiseAsync();
        };

        It should_have_hydrated_the_projection = () => container.Resolve<TestProjection>().IdFromEvent.Should().Be(null);
    }
}
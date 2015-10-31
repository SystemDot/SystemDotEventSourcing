using SystemDot.EventSourcing.Aggregation;

namespace SystemDot.EventSourcing
{
    using SystemDot.Domain;

    public interface IDomainRepository
    {
        TEventSourcedEntity Get<TEventSourcedEntity>(MultiSiteId id)
            where TEventSourcedEntity : EventSourcedEntity, new();

        bool Exists(MultiSiteId id);

        void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : AggregateRoot;
    }
}
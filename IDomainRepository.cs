using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing
{
    using SystemDot.Domain;

    public interface IDomainRepository
    {
        TAggregateRoot Get<TAggregateRoot>(MultiSiteId aggregateRootId) 
            where TAggregateRoot : AggregateRoot, new();

        bool Exists(MultiSiteId aggregateRootId);

        void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : AggregateRoot;
    }
}
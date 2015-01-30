using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing
{
    public interface IDomainRepository
    {
        TAggregateRoot Get<TAggregateRoot>(AggregateRootId aggregateRootId) 
            where TAggregateRoot : AggregateRoot, new();

        bool Exists(AggregateRootId aggregateRootId);

        void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : AggregateRoot;
    }
}
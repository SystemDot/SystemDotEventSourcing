using SystemDot.EventSourcing.Aggregation;

namespace SystemDot.EventSourcing
{
    public interface IDomainRepository
    {
        TAggregateRoot Get<TAggregateRoot>(string aggregateRootId) 
            where TAggregateRoot : AggregateRoot, new();

        bool Exists(string aggregateRootId);
    }
}
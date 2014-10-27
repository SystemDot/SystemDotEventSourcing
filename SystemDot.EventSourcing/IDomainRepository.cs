using System.Threading.Tasks;
using SystemDot.EventSourcing.Aggregation;

namespace SystemDot.EventSourcing
{
    public interface IDomainRepository
    {
        Task<TAggregateRoot> GetAsync<TAggregateRoot>(string aggregateRootId) 
            where TAggregateRoot : AggregateRoot, new();

        Task<bool> ExistsAsync(string aggregateRootId);
    }
}
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Sessions
{
    public interface IEventSessionFactory
    {
        Task<IEventSession> CreateAsync();
    }
}
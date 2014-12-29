using SystemDot.Core;
using NEventStore;
using NEventStore.Dispatcher;

namespace SystemDot.EventSourcing.Sql.Windows
{
    
    public class NullCommitDispatcher : Disposable, IDispatchCommits
    {
        public void Dispatch(ICommit commit)
        {
        }
    }
}
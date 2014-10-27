using SystemDot.Core;
using EventStore;
using EventStore.Dispatcher;

namespace SystemDot.EventSourcing.Sql.Windows
{
    public class NullCommitDispatcher : Disposable, IDispatchCommits
    {
        public void Dispatch(Commit commit)
        {
        }
    }
}
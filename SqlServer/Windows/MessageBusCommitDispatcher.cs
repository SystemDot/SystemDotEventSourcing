using System.Linq;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.Domain.Events.Dispatching;
using NEventStore.Dispatcher;

namespace SystemDot.EventSourcing.Sql.Windows
{
    using NEventStore;

    public class MessageBusCommitDispatcher : Disposable, IDispatchCommits
    {
        readonly IEventDispatcher innerDispatcher;

        public MessageBusCommitDispatcher(IEventDispatcher innerDispatcher)
        {
            this.innerDispatcher = innerDispatcher;
        }

        public void Dispatch(ICommit commit)
        {
           commit.Events.Select(m => m.Body).ForEach(e => innerDispatcher.Dispatch(e));
        }
    }
}

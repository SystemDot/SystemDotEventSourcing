using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Commits;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Sessions
{
    public class EventSession : Disposable, IEventSession
    {
        readonly IEventStore eventStore;
        readonly Dictionary<string, IEventStream> streams;

        public EventSession(IEventStore eventStore)
        {
            this.eventStore = eventStore;
            streams = new Dictionary<string, IEventStream>();
        }

        public IEnumerable<SourcedEvent> GetEvents(string streamId)
        {
            IEventStream stream = GetStream(streamId);
            return stream.CommittedEvents.Concat(stream.UncommittedEvents);
        }

        public void StoreEvent(SourcedEvent @event, string aggregateRootId)
        {
            GetStream(aggregateRootId).Add(@event);
        }

        public IEnumerable<Commit> AllCommitsFrom(DateTime @from)
        {
            return eventStore.GetCommitsFrom(@from);
        }

        public void StoreHeader(string id, string key, object value)
        {
            GetStream(id).UncommittedHeaders[key] = value;
        }

        IEventStream GetStream(string aggregateRootId)
        {
            IEventStream stream;

            if (!streams.TryGetValue(aggregateRootId, out stream)) 
                streams[aggregateRootId] = stream = eventStore.OpenStream(aggregateRootId);

            return stream;
        }

        public void Commit(Guid commitId)
        {
            foreach (var stream in streams)
            {
                CommitStream(commitId, stream.Value);
            }
        }

        void CommitStream(Guid commitId, IEventStream toCommit)
        {
            try
            {
                toCommit.CommitChanges(commitId);
            }
            catch (DuplicateCommitException)
            {
                toCommit.ClearChanges();
            }
            catch (ConcurrencyException)
            {
                toCommit.ClearChanges();
                throw;
            }
        }

        protected override void DisposeOfManagedResources()
        {
            streams.ForEach(s => s.Value.Dispose());
            streams.Clear();

            base.DisposeOfManagedResources();
        }
    }
}
namespace SystemDot.EventSourcing.Sqlite.Android
{
    using System;
    using System.Collections.Generic;
    using SystemDot.Core;
    using SystemDot.Core.Collections;
    using SystemDot.Domain.Events.Dispatching;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Streams;

    public class SqlLiteEventStream : Disposable, IEventStream
    {
        readonly SqlLitePersistenceEngine persistenceEngine;
        readonly EventStreamId streamId;
        readonly IEventDispatcher eventDispatcher;
        readonly List<SourcedEvent> uncommittedEvents;
        readonly List<SourcedEvent> committedEvents;
        int currentSequence;
        readonly List<Guid> commits;

        public SqlLiteEventStream(SqlLitePersistenceEngine persistenceEngine, EventStreamId streamId, IEventDispatcher eventDispatcher)
        {
            this.persistenceEngine = persistenceEngine;
            this.streamId = streamId;
            this.eventDispatcher = eventDispatcher;

            uncommittedEvents = new List<SourcedEvent>();
            committedEvents = new List<SourcedEvent>();
            UncommittedHeaders = new Dictionary<string, object>();
            commits = new List<Guid>();
            PopulateStream();
        }

        void PopulateStream()
        {
            committedEvents.Clear();
            
            foreach (Commit commit in persistenceEngine.GetCommits(streamId.BucketId, streamId.Id))
            {
                committedEvents.AddRange(commit.Events);
                currentSequence++;
                commits.Add(commit.CommitId);
            }  
         
            ClearChanges();
        }

        protected override void DisposeOfManagedResources()
        {
            uncommittedEvents.Clear();
            committedEvents.Clear();
            UncommittedHeaders.Clear();
            base.DisposeOfManagedResources();
        }
        
        public void Add(SourcedEvent @event)
        {
            uncommittedEvents.Add(@event);
        }

        public void CommitChanges(Guid commitId)
        {
            if (commits.Contains(commitId))
            {
                throw new DuplicateCommitException();
            }

            persistenceEngine.Commit(streamId.BucketId, streamId.Id, commitId, currentSequence, UncommittedEvents, UncommittedHeaders);
            
            if (!UncommittedHeaders.ContainsKey(PreventCommitDispatchHeader.Key))
            {
                UncommittedEvents.ForEach(e => eventDispatcher.Dispatch(e.Body));
            }

            PopulateStream();
        }

        public void ClearChanges()
        {
            uncommittedEvents.Clear();
            UncommittedHeaders.Clear();
        }

        public IEnumerable<SourcedEvent> CommittedEvents
        {
            get { return committedEvents; }
        }

        public IEnumerable<SourcedEvent> UncommittedEvents
        {
            get { return uncommittedEvents; }
        }

        public IDictionary<string, object> UncommittedHeaders { get; private set; }
    }
}

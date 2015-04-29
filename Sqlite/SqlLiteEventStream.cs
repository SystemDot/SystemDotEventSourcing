namespace SystemDot.EventSourcing.Sqlite.Android
{
    using System;
    using System.Collections.Generic;
    using SystemDot.Core;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Streams;

    public class SqlLiteEventStream : Disposable, IEventStream
    {
        readonly SqlLitePersistenceEngine persistenceEngine;
        readonly EventStreamId streamId;
        readonly List<SourcedEvent> uncommittedEvents;
        readonly List<SourcedEvent> committedEvents;
        int currentSequence;

        public SqlLiteEventStream(SqlLitePersistenceEngine persistenceEngine, EventStreamId streamId)
        {
            this.persistenceEngine = persistenceEngine;
            this.streamId = streamId;

            uncommittedEvents = new List<SourcedEvent>();
            committedEvents = new List<SourcedEvent>();
            UncommittedHeaders = new Dictionary<string, object>();

            PopulateStream();
        }

        void PopulateStream()
        {
            committedEvents.Clear();
            
            foreach (Commit commit in persistenceEngine.GetCommits(streamId.BucketId, streamId.Id))
            {
                committedEvents.AddRange(commit.Events);
                currentSequence++;
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
            persistenceEngine.Commit(streamId.BucketId, streamId.Id, commitId, currentSequence, UncommittedEvents, UncommittedHeaders);
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

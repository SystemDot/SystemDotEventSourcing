namespace SystemDot.EventSourcing.Sqlite.Android
{
    using System;
    using System.Collections.Generic;
    using SystemDot.Domain.Events.Dispatching;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Streams;

    public class SqlLiteEventStore : IEventStore
    {
        readonly SqlLitePersistenceEngine persistenceEngine;
        readonly EventDispatcher eventDispatcher;

        public SqlLiteEventStore(SqlLitePersistenceEngine persistenceEngine, EventDispatcher eventDispatcher)
        {
            this.persistenceEngine = persistenceEngine;
            this.eventDispatcher = eventDispatcher;
        }

        public IEventStream OpenStream(EventStreamId streamId)
        {
            return new SqlLiteEventStream(persistenceEngine, streamId, eventDispatcher);
        }

        public IEnumerable<Commit> GetCommitsFrom(string bucketId, DateTime @from)
        {
            return persistenceEngine.GetCommitsFrom(bucketId, @from);
        }

        public IEnumerable<Commit> GetCommits()
        {
            return persistenceEngine.GetCommits();
        }
    }
}
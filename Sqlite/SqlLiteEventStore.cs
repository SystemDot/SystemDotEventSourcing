namespace SystemDot.EventSourcing.Sqlite.Android
{
    using System;
    using System.Collections.Generic;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Streams;

    public class SqlLiteEventStore : IEventStore
    {
        readonly SqlLitePersistenceEngine persistenceEngine;

        public SqlLiteEventStore(SqlLitePersistenceEngine persistenceEngine)
        {
            this.persistenceEngine = persistenceEngine;
        }

        public IEventStream OpenStream(EventStreamId streamId)
        {
            return new SqlLiteEventStream(persistenceEngine, streamId);
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
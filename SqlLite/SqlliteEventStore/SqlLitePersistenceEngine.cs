namespace SqlliteEventStore
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Streams;
    using Mono.Data.Sqlite;

    public class SqlLitePersistenceEngine
    {
        readonly string connectionString;

        public SqlLitePersistenceEngine(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Commit(string bucketId, string streamId, Guid commitId, int sequence, IEnumerable<SourcedEvent> uncommittedEvents, IDictionary<string, object> uncommittedHeaders)
        {
            using (var connection = GetConnection())
            {
                connection.Execute(
                    "insert into Commits(BucketId, StreamId, CommitId, CommitSequence, Headers, Payload) values(@BucketId, @StreamId, @CommitId, @CommitSequence, @Headers, @Payload)",
                    command =>
                    {
                        AddParameter(command.Parameters, "@BucketId", bucketId);
                        AddParameter(command.Parameters, "@StreamId", streamId);
                        AddParameter(command.Parameters, "@CommitId", commitId.ToString());
                        AddParameter(command.Parameters, "@CommitSequence", sequence);
                        AddParameter(command.Parameters, "@Headers", uncommittedHeaders.SerialiseToBytes());
                        AddParameter(command.Parameters, "@Payload", uncommittedEvents.SerialiseToBytes());
                    });
            }
        }

        public IEnumerable<Commit> GetCommits()
        {
            yield break;
        }

        public IEnumerable<Commit> GetCommits(string bucketId, string id)
        {
            yield break;
        }

        public IEnumerable<Commit> GetCommitsFrom(string bucketId, DateTime @from)
        {
            yield break;
        }

        string GetInitialisationSql()
        {
            return @"
CREATE TABLE IF NOT EXISTS Commits(
    BucketId TEXT NOT NULL,
    StreamId TEXT NOT NULL,
	CommitId TEXT NOT NULL,
	CommitSequence INTEGER NOT NULL,
	CheckpointNumber INTEGER PRIMARY KEY,
    Headers BLOB NULL,
    Payload BLOB NOT NULL,
    CommitStamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
        }

        void AddParameter(DbParameterCollection collection, string name, object value)
        {
            collection.Add(new SqliteParameter(name, value));
        }

        DbConnection CreateConnection()
        {
            return new SqliteConnection(connectionString);
        }

        DbConnection GetConnection()
        {
            DbConnection connection = CreateConnection();
            connection.Open();
            return connection;
        }
    }

    public static class ObjectExtensions
    {
        public static byte[] SerialiseToBytes(this object toSerialise)
        {
            throw new NotImplementedException();
        }
    }
}
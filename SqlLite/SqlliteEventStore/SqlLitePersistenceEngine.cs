namespace SqlliteEventStore
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.IO;
    using System.Linq;
    using System.Text;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Streams;
    using Mono.Data.Sqlite;
    using Newtonsoft.Json;
    using ProtoBuf;
    using ProtoBuf.Meta;

    public class SqlLitePersistenceEngine
    {
        readonly string connectionString;
        readonly ISerialize serialiser;

        public SqlLitePersistenceEngine(string connectionString, ISerialize serialiser)
        {
            this.connectionString = connectionString;
            this.serialiser = serialiser;

            using (var connection = GetConnection())
            {
                connection.Execute(GetInitialisationSql(), command => { });
            }
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
                        AddParameter(command.Parameters, "@Headers", uncommittedHeaders.SerialiseToBytes(serialiser));
                        AddParameter(command.Parameters, "@Payload", uncommittedEvents.SerialiseToBytes(serialiser));
                    });
            }
        }

        public IEnumerable<Commit> GetCommits()
        {
            return GetCommits("select BucketId, StreamId, CommitId, Headers, Payload from Commits order by BucketId ASC, StreamId ASC, CommitSequence ASC");
        }

        public IEnumerable<Commit> GetCommits(string bucketId, string id)
        {
            return GetCommits("select BucketId, StreamId, CommitId, Headers, Payload from Commits where BucketId = '" + bucketId + "' and StreamId = '" + id + "' order by CommitSequence ASC");
        }

        public IEnumerable<Commit> GetCommitsFrom(string bucketId, DateTime @from)
        {
            return GetCommits("select BucketId, StreamId, CommitId, Headers, Payload from Commits where BucketId = '" + bucketId + "' and CommitStamp = '" + @from.ToSqliteFormat() + "' order by StreamId ASC, CommitSequence ASC");
        }

        IEnumerable<Commit> GetCommits(string sql)
        {
            var commits = new List<Commit>();

            using (var connection = GetConnection())
            {
                connection.ExecuteReader(sql, reader => commits.Add(
                    new Commit(
                        reader.GetGuid(2),
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetBytes(4).DeserialiseTo<IEnumerable<SourcedEvent>>(serialiser),
                        reader.GetBytes(3).DeserialiseTo<IDictionary<String, object>>(serialiser))));
            }

            return commits;
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
        public static byte[] SerialiseToBytes(this object toSerialise, ISerialize serializer)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, toSerialise);
                return stream.ToArray();
            }
        }
    }

    public static class ByteExtensions
    {
        public static T DeserialiseTo<T>(this byte[] toDeserialise, ISerialize serializer)
        {
            toDeserialise = toDeserialise ?? new byte[] { };
            if (toDeserialise.Length == 0)
            {
                return default(T);
            }

            using (var stream = new MemoryStream(toDeserialise))
                return serializer.Deserialize<T>(stream);
        }
    }

    public interface ISerialize
    {
        void Serialize<T>(Stream output, T graph);
        T Deserialize<T>(Stream input);
    }

    public static class DateTimeExtensions
    {
        public static string ToSqliteFormat(this DateTime datetime)
        {
            return string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
        }
    }

    public static class DbDataReaderExtensions
    {
        public static byte[] GetBytes(this DbDataReader reader, int ordinal)
        {
            long length = reader.GetBytes(ordinal, 0, null, 0, 0);

            var buffer = new byte[length];
            reader.GetBytes(ordinal, 0, buffer, 0, (int)length);

            return buffer;
        }
    }

    public class JsonSerializer : ISerialize
    {
        private readonly IEnumerable<Type> _knownTypes = new[] { typeof(List<SourcedEvent>), typeof(Dictionary<string, object>) };

        private readonly Newtonsoft.Json.JsonSerializer _typedSerializer = new Newtonsoft.Json.JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.All,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly Newtonsoft.Json.JsonSerializer _untypedSerializer = new Newtonsoft.Json.JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.Auto,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        public virtual void Serialize<T>(Stream output, T graph)
        {
            using (var streamWriter = new StreamWriter(output, Encoding.UTF8))
                Serialize(new JsonTextWriter(streamWriter), graph);
        }

        public virtual T Deserialize<T>(Stream input)
        {
            using (var streamReader = new StreamReader(input, Encoding.UTF8))
                return Deserialize<T>(new JsonTextReader(streamReader));
        }

        protected virtual void Serialize(JsonWriter writer, object graph)
        {
            using (writer)
                GetSerializer(graph.GetType()).Serialize(writer, graph);
        }

        protected virtual T Deserialize<T>(JsonReader reader)
        {
            Type type = typeof(T);

            using (reader)
                return (T)GetSerializer(type).Deserialize(reader, type);
        }

        protected virtual Newtonsoft.Json.JsonSerializer GetSerializer(Type typeToSerialize)
        {
            if (_knownTypes.Contains(typeToSerialize))
            {
                return _untypedSerializer;
            }

            return _typedSerializer;
        }
    }

    /*
    USE [Financier]
    GO

    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO

    SET ANSI_PADDING ON
    GO

    CREATE TABLE [dbo].[Commits](
        [BucketId] [varchar](40) NOT NULL,
        [StreamId] [char](40) NOT NULL,
        [StreamIdOriginal] [nvarchar](1000) NOT NULL,
        [CommitId] [uniqueidentifier] NOT NULL,
        [CommitSequence] [int] NOT NULL,
        [CheckpointNumber] [bigint] IDENTITY(1,1) NOT NULL,
        [Headers] [varbinary](max) NULL,
        [Payload] [varbinary](max) NOT NULL,
        [CommitStamp] [datetime2](7) NOT NULL,
     CONSTRAINT [PK_Commits] PRIMARY KEY CLUSTERED 
    (
        [CheckpointNumber] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    )

    GO

    SET ANSI_PADDING OFF
    GO

    ALTER TABLE [dbo].[Commits] ADD  DEFAULT ((0)) FOR [Dispatched]
    GO

    ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([CommitId]<>0x00))
    GO

    ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([CommitSequence]>(0)))
    GO

    ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([Headers] IS NULL OR datalength([Headers])>(0)))
    GO

    ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([Items]>(0)))
    GO

    ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  ((datalength([Payload])>(0)))
    GO

    ALTER TABLE [dbo].[Commits]  WITH CHECK ADD CHECK  (([StreamRevision]>(0)))
    GO

    USE [Financier]
    GO

    CREATE UNIQUE NONCLUSTERED INDEX [IX_Commits_CommitId] ON [dbo].[Commits]
    (
        [BucketId] ASC,
        [StreamId] ASC,
        [CommitId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    GO

    CREATE UNIQUE NONCLUSTERED INDEX [IX_Commits_CommitSequence] ON [dbo].[Commits]
    (
        [BucketId] ASC,
        [StreamId] ASC,
        [CommitSequence] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    GO

    CREATE NONCLUSTERED INDEX [IX_Commits_Dispatched] ON [dbo].[Commits]
    (
        [Dispatched] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    GO


    CREATE NONCLUSTERED INDEX [IX_Commits_Stamp] ON [dbo].[Commits]
    (
        [CommitStamp] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    GO

    ALTER TABLE [dbo].[Commits] ADD  CONSTRAINT [PK_Commits] PRIMARY KEY CLUSTERED 
    (
        [CheckpointNumber] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    GO



    */
}
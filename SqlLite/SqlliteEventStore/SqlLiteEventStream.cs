namespace SqlliteEventStore
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
            foreach (Commit commit in persistenceEngine.GetCommits(streamId.BucketId, streamId.Id))
            {
                committedEvents.AddRange(commit.Events);
                currentSequence++;
            }           
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
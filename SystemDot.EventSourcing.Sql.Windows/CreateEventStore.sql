
CREATE TABLE [dbo].[Commits](
	[StreamId] [uniqueidentifier] NOT NULL,
	[StreamRevision] [int] NOT NULL,
	[Items] [tinyint] NOT NULL,
	[CommitId] [uniqueidentifier] NOT NULL,
	[CommitSequence] [int] NOT NULL,
	[CommitStamp] [datetime] NOT NULL,
	[Dispatched] [bit] NOT NULL,
	[Headers] [varbinary](max) NULL,
	[Payload] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Commits] PRIMARY KEY CLUSTERED 
(
	[StreamId] ASC,
	[CommitSequence] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Commits] ON [dbo].[Commits] 
(
	[StreamId] ASC,
	[CommitId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Commits_Dispatched] ON [dbo].[Commits] 
(
	[Dispatched] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Commits_Revisions] ON [dbo].[Commits] 
(
	[StreamId] ASC,
	[StreamRevision] ASC,
	[Items] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Commits_Stamp] ON [dbo].[Commits] 
(
	[CommitStamp] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Snapshots](
	[StreamId] [uniqueidentifier] NOT NULL,
	[StreamRevision] [int] NOT NULL,
	[Payload] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Snapshots] PRIMARY KEY CLUSTERED 
(
	[StreamId] ASC,
	[StreamRevision] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lookups](
	[Type] [nvarchar](100) NOT NULL,
	[SourceKey] [nvarchar](100) NOT NULL,
	[OutputId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Lookups] PRIMARY KEY CLUSTERED 
(
	[OutputId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Lookups_Type_SourceKey] ON [dbo].[Lookups] 
(
	[Type] ASC,
	[SourceKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupAggregate]
(
      @Type NVARCHAR(100),
      @SourceKey NVARCHAR(100)
)
AS

DECLARE @OutputId UNIQUEIDENTIFIER

SELECT 
      @OutputId = OutputId 
FROM
      [dbo].Lookups
WHERE
      [Type] = @Type
      AND SourceKey = @SourceKey
      
IF(@OutputId IS NULL) 
BEGIN
	  DECLARE @NewOutputId table (OutputId UNIQUEIDENTIFIER)

      INSERT dbo.Lookups
      (
            [Type], 
            SourceKey
      )
      OUTPUT inserted.OutputId
      INTO @NewOutputId
            VALUES
      (
            @Type, 
            @SourceKey
      )
      SELECT @OutputId = OutputId
      FROM @NewOutputId
END

SELECT @OutputId
GO
ALTER TABLE [dbo].[Commits] ADD  CONSTRAINT [DF_Commits_StreamId]  DEFAULT ((newsequentialid)) FOR [StreamId]
GO
ALTER TABLE [dbo].[Commits] ADD  DEFAULT ((0)) FOR [Dispatched]
GO
ALTER TABLE [dbo].[Lookups] ADD  CONSTRAINT [DF_Lookups_OutputId]  DEFAULT (newsequentialid()) FOR [OutputId]
GO
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD  CONSTRAINT [CK_Commits_CommitId] CHECK  (([CommitId]<>0x00))
GO
ALTER TABLE [dbo].[Commits] CHECK CONSTRAINT [CK_Commits_CommitId]
GO
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD  CONSTRAINT [CK_Commits_CommitSequence] CHECK  (([CommitSequence]>(0)))
GO
ALTER TABLE [dbo].[Commits] CHECK CONSTRAINT [CK_Commits_CommitSequence]
GO
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD  CONSTRAINT [CK_Commits_Headers] CHECK  (([Headers] IS NULL OR datalength([Headers])>(0)))
GO
ALTER TABLE [dbo].[Commits] CHECK CONSTRAINT [CK_Commits_Headers]
GO
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD  CONSTRAINT [CK_Commits_Items] CHECK  (([Items]>(0)))
GO
ALTER TABLE [dbo].[Commits] CHECK CONSTRAINT [CK_Commits_Items]
GO
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD  CONSTRAINT [CK_Commits_Payload] CHECK  ((datalength([Payload])>(0)))
GO
ALTER TABLE [dbo].[Commits] CHECK CONSTRAINT [CK_Commits_Payload]
GO
ALTER TABLE [dbo].[Commits]  WITH CHECK ADD  CONSTRAINT [CK_Commits_StreamRevision] CHECK  (([StreamRevision]>(0)))
GO
ALTER TABLE [dbo].[Commits] CHECK CONSTRAINT [CK_Commits_StreamRevision]
GO
ALTER TABLE [dbo].[Snapshots]  WITH CHECK ADD  CONSTRAINT [CK_Snapshots_Payload] CHECK  ((datalength([Payload])>(0)))
GO
ALTER TABLE [dbo].[Snapshots] CHECK CONSTRAINT [CK_Snapshots_Payload]
GO
ALTER TABLE [dbo].[Snapshots]  WITH CHECK ADD  CONSTRAINT [CK_Snapshots_StreamRevision] CHECK  (([StreamRevision]>(0)))
GO
ALTER TABLE [dbo].[Snapshots] CHECK CONSTRAINT [CK_Snapshots_StreamRevision]
GO
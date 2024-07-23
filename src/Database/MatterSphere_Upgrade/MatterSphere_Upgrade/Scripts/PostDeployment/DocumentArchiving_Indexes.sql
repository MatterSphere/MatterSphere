SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[dbDocumentLog]') AND name = N'IX_dbDocumentLog_logType')
CREATE NONCLUSTERED INDEX [IX_dbDocumentLog_logType]
	ON [dbo].[dbDocumentLog]([logType] ASC) INCLUDE ([docID])
	ON [IndexGroup];
GO
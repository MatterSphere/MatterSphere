IF NOT EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON (t.schema_id = s.schema_id) WHERE s.name = 'dbo' AND t.name = 'dbDocuSignEnvelopes')
CREATE TABLE [dbo].[dbDocuSignEnvelopes]
(
    [envID]         INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL CONSTRAINT [PK_dbDocuSignEnvelopes] PRIMARY KEY CLUSTERED,
    [fileID]        BIGINT              NOT NULL /*CONSTRAINT [FK_dbDocuSignEnvelopes_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) ON UPDATE CASCADE ON DELETE CASCADE NOT FOR REPLICATION*/,
    [envGuid]       UNIQUEIDENTIFIER    NOT NULL,
    [envStatus]     [dbo].[uCodeLookup] NOT NULL,
    [envSubject]    NVARCHAR (100)      NOT NULL,
    [envEmailBlurb] NVARCHAR (MAX)      NULL,
    [envDocID]      BIGINT              NULL /*CONSTRAINT [FK_dbDocuSignEnvelopes_dbDocument] FOREIGN KEY ([envDocID]) REFERENCES [dbo].[dbDocument] ([docID]) ON UPDATE CASCADE ON DELETE SET NULL NOT FOR REPLICATION*/,
    [Created]       [dbo].[uCreated]    NOT NULL CONSTRAINT [DF_dbDocuSignEnvelopes_Created] DEFAULT (getutcdate()),
    [CreatedBy]     [dbo].[uCreatedBy]  NOT NULL,
    [Updated]       [dbo].[uCreated]    NULL,
    [UpdatedBy]     [dbo].[uCreatedBy]  NULL,
    [rowguid]       UNIQUEIDENTIFIER    NOT NULL CONSTRAINT [DF_dbDocuSignEnvelopes_rowguid] DEFAULT (newid()) ROWGUIDCOL
)
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_dbDocuSignEnvelopes_dbFile'))
BEGIN
    IF EXISTS (SELECT * FROM sys.tables WHERE schema_id =  schema_id('config') AND name = 'dbFile')
        ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbFile] FOREIGN KEY ([fileID]) REFERENCES [config].[dbFile] ([fileID]) ON UPDATE CASCADE ON DELETE CASCADE NOT FOR REPLICATION
    ELSE
        ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) ON UPDATE CASCADE ON DELETE CASCADE NOT FOR REPLICATION
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_dbDocuSignEnvelopes_dbDocument'))
BEGIN
    IF EXISTS (SELECT * FROM sys.tables WHERE schema_id =  schema_id('config') AND name = 'dbDocument')
        ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbDocument] FOREIGN KEY ([envDocID]) REFERENCES [config].[dbDocument] ([docID]) ON UPDATE CASCADE ON DELETE SET NULL NOT FOR REPLICATION
    ELSE
        ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbDocument] FOREIGN KEY ([envDocID]) REFERENCES [dbo].[dbDocument] ([docID]) ON UPDATE CASCADE ON DELETE SET NULL NOT FOR REPLICATION
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocusignEnvelopes_fileID' AND object_id = OBJECT_ID('[dbo].[dbDocuSignEnvelopes]'))
CREATE NONCLUSTERED INDEX [IX_dbDocuSignEnvelopes_fileID]
    ON [dbo].[dbDocuSignEnvelopes]([fileID] ASC) INCLUDE ([envDocID])
    ON [IndexGroup];
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocuSignEnvelopes_envGuid' AND object_id = OBJECT_ID('[dbo].[dbDocuSignEnvelopes]'))
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocuSignEnvelopes_envGuid]
    ON [dbo].[dbDocuSignEnvelopes]([envGuid] ASC)
    ON [IndexGroup];
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocuSignEnvelopes_rowguid' AND object_id = OBJECT_ID('[dbo].[dbDocuSignEnvelopes]'))
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocuSignEnvelopes_rowguid]
    ON [dbo].[dbDocuSignEnvelopes]([rowguid] ASC)
    ON [IndexGroup];
GO

GRANT SELECT, INSERT, UPDATE, DELETE
    ON OBJECT::[dbo].[dbDocuSignEnvelopes] TO [OMSApplicationRole]
    AS [dbo];
GO

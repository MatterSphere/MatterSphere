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

ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) ON UPDATE CASCADE ON DELETE CASCADE NOT FOR REPLICATION
GO

ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbDocument] FOREIGN KEY ([envDocID]) REFERENCES [dbo].[dbDocument] ([docID]) ON UPDATE CASCADE ON DELETE SET NULL NOT FOR REPLICATION
GO

CREATE NONCLUSTERED INDEX [IX_dbDocuSignEnvelopes_fileID]
    ON [dbo].[dbDocuSignEnvelopes]([fileID] ASC) INCLUDE ([envDocID])
    ON [IndexGroup];
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocuSignEnvelopes_envGuid]
    ON [dbo].[dbDocuSignEnvelopes]([envGuid] ASC)
    ON [IndexGroup];
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocuSignEnvelopes_rowguid]
    ON [dbo].[dbDocuSignEnvelopes]([rowguid] ASC)
    ON [IndexGroup];
GO

GRANT SELECT, INSERT, UPDATE, DELETE
    ON OBJECT::[dbo].[dbDocuSignEnvelopes] TO [OMSApplicationRole]
    AS [dbo];
GO

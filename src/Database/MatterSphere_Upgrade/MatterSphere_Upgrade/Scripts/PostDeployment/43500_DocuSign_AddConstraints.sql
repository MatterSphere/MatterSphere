IF EXISTS (SELECT * FROM sys.tables WHERE schema_id =  schema_id('config') AND name = 'dbFile')
    ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbFile] FOREIGN KEY ([fileID]) REFERENCES [config].[dbFile] ([fileID]) ON UPDATE CASCADE ON DELETE CASCADE NOT FOR REPLICATION
ELSE
    ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) ON UPDATE CASCADE ON DELETE CASCADE NOT FOR REPLICATION
GO

IF EXISTS (SELECT * FROM sys.tables WHERE schema_id =  schema_id('config') AND name = 'dbDocument')
    ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbDocument] FOREIGN KEY ([envDocID]) REFERENCES [config].[dbDocument] ([docID]) ON UPDATE CASCADE ON DELETE SET NULL NOT FOR REPLICATION
ELSE
    ALTER TABLE [dbo].[dbDocuSignEnvelopes] ADD CONSTRAINT [FK_dbDocuSignEnvelopes_dbDocument] FOREIGN KEY ([envDocID]) REFERENCES [dbo].[dbDocument] ([docID]) ON UPDATE CASCADE ON DELETE SET NULL NOT FOR REPLICATION
GO

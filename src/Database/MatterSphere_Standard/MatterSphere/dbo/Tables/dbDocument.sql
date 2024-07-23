CREATE TABLE [dbo].[dbDocument] (
    [docID]                        BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [clID]                         BIGINT              NOT NULL,
    [fileID]                       BIGINT              NOT NULL,
    [assocID]                      BIGINT              NOT NULL,
    [docbrID]                      INT                 NULL,
    [docType]                      [dbo].[uCodeLookup] NULL,
    [docprecID]                    BIGINT              NULL,
    [docbaseprecID]                BIGINT              NULL,
    [docChecksum]                  VARCHAR (50)        NULL,
    [docDesc]                      NVARCHAR (150)      NULL,
    [docStyleDesc]                 NVARCHAR (100)      NULL,
    [docWallet]                    [dbo].[uCodeLookup] NULL,
    [docFileName]                  NVARCHAR (255)      NOT NULL,
    [docLocation]                  SMALLINT            NULL,
    [docdirID]                     SMALLINT            NULL,
    [docPassword]                  NVARCHAR (25)       NULL,
    [docDirection]                 BIT                 CONSTRAINT [DF_dbDocument_DocDirection] DEFAULT ((0)) NOT NULL,
    [docArchived]                  BIT                 CONSTRAINT [DF_dbDocument_docArchived] DEFAULT ((0)) NOT NULL,
    [docArchiveLocation]           SMALLINT            CONSTRAINT [DF_dbDocument_docArchiveLocation] DEFAULT ((0)) NOT NULL,
    [docArchiveDirID]              SMALLINT            NULL,
    [docAppID]                     SMALLINT            NULL,
    [docPasswordHint]              NVARCHAR (75)       NULL,
    [docExtension]                 NVARCHAR (15)       NULL,
    [docAuthored]                  [dbo].[uCreated]    NULL,
    [Createdby]                    [dbo].[uCreatedBy]  NULL,
    [Created]                      [dbo].[uCreated]    NULL,
    [UpdatedBy]                    [dbo].[uCreatedBy]  NULL,
    [Updated]                      [dbo].[uCreated]    NULL,
    [docParent]                    BIGINT              NULL,
    [docDeleted]                   BIT                 CONSTRAINT [DF_dbDocument_docDeleted] DEFAULT ((0)) NOT NULL,
    [docRetain]                    DATETIME            NULL,
    [docFlags]                     SMALLINT            CONSTRAINT [DF_dbDocument_docFlags] DEFAULT ((0)) NOT NULL,
    [phID]                         BIGINT              NULL,
    [docIDOld]                     INT                 NULL,
    [rowguid]                      UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDocument_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [docAuthoredBy]                INT                 NULL,
    [docCheckedOut]                DATETIME            NULL,
    [docCheckedOutBy]              INT                 NULL,
    [docCheckedOutlocation]        NVARCHAR (255)      NULL,
    [docCurrentVersion]            UNIQUEIDENTIFIER    NULL,
    [docLastAccessed]              DATETIME            NULL,
    [docLastAccessedBy]            INT                 NULL,
    [docLastEdited]                DATETIME            NULL,
    [docLastEditedBy]              INT                 NULL,
    [docIDExt]                     NVARCHAR (50)       NULL,
    [docCreationEmailProcessed]    BIT                 NULL,
    [docCreationExtEmailProcessed] BIT                 NULL,
    [SecurityOptions]              BIGINT              DEFAULT ((0)) NOT NULL,
    [docFolderGUID] UNIQUEIDENTIFIER NULL, 
    [Opened] DATETIME NULL, 
    CONSTRAINT [PK_dbDocument] PRIMARY KEY NONCLUSTERED ([docID] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup],
    CONSTRAINT [FK_dbDocument_dbApplication] FOREIGN KEY ([docAppID]) REFERENCES [dbo].[dbApplication] ([appID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbAssociates] FOREIGN KEY ([assocID]) REFERENCES [dbo].[dbAssociates] ([assocID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbBranch] FOREIGN KEY ([docbrID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbClient] FOREIGN KEY ([clID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbDirectory] FOREIGN KEY ([docdirID]) REFERENCES [dbo].[dbDirectory] ([dirID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbDirectory1] FOREIGN KEY ([docArchiveDirID]) REFERENCES [dbo].[dbDirectory] ([dirID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbDocumentType] FOREIGN KEY ([docType]) REFERENCES [dbo].[dbDocumentType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbFilePhase] FOREIGN KEY ([phID]) REFERENCES [dbo].[dbFilePhase] ([phID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbPrecedents] FOREIGN KEY ([docbaseprecID]) REFERENCES [dbo].[dbPrecedents] ([PrecID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbPrecedents1] FOREIGN KEY ([docprecID]) REFERENCES [dbo].[dbPrecedents] ([PrecID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbStorageProvider] FOREIGN KEY ([docLocation]) REFERENCES [dbo].[dbStorageProvider] ([spID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_dbStorageProvider1] FOREIGN KEY ([docArchiveLocation]) REFERENCES [dbo].[dbStorageProvider] ([spID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocument_Parent] FOREIGN KEY ([docParent]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION
);




GO
CREATE CLUSTERED INDEX [IX_dbDocument_Created]
    ON [dbo].[dbDocument]([Created] DESC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_AssociateID]
    ON [dbo].[dbDocument]([assocID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_Checksum]
    ON [dbo].[dbDocument]([docChecksum] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_ClientID]
    ON [dbo].[dbDocument]([clID] ASC, [docDeleted] ASC, [docType] ASC)
    INCLUDE([docID], [fileID], [assocID], [Created], [docDesc], [docStyleDesc], [docAuthored], [docAuthoredBy], [Updated], [UpdatedBy], [docDirection], [docExtension], [docFileName], [docCheckedOut], [docCheckedOutBy], [docCheckedOutlocation], [docAppID], [docCurrentVersion], [Createdby], [docWallet], [SecurityOptions]) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_CreatedBy]
    ON [dbo].[dbDocument]([Createdby] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_DocCheckedOutBy]
    ON [dbo].[dbDocument]([docCheckedOutBy] ASC)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_docType]
    ON [dbo].[dbDocument]([docType] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_fileID]
    ON [dbo].[dbDocument]([fileID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocument_rowguid]
    ON [dbo].[dbDocument]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_Wallet]
    ON [dbo].[dbDocument]([docID] ASC, [docWallet] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_assocID]
    ON [dbo].[dbDocument]([assocID] ASC)
    INCLUDE([docID], [Created])
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocument_DocID]
    ON [dbo].[dbDocument]([docID] ASC, [docDeleted] ASC, [docType] ASC)
    INCLUDE([fileID], [clID], [assocID], [Created], [docDesc], [docStyleDesc], [docAuthored], [docAuthoredBy], [Updated], [UpdatedBy], [docDirection], [docExtension], [docFileName], [docCheckedOut], [docCheckedOutBy], [docCheckedOutlocation], [docAppID], [docCurrentVersion], [Createdby], [docWallet], [SecurityOptions]) WITH (FILLFACTOR = 80)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocument_docDeleted]
    ON [dbo].[dbDocument]([docDeleted] ASC)
    INCLUDE ([docID], [clID], [docFileName]) WITH (FILLFACTOR = 90)
    ON [IndexGroup];

GO

CREATE NONCLUSTERED INDEX [IX_dbDocument_Opened] ON [dbo].[dbDocument]
(
	[Opened] ASC
)
ON [IndexGroup];
GO

CREATE NONCLUSTERED INDEX [IX_dbDocument_Updated] ON [dbo].[dbDocument]
(
	[Updated] ASC
)
ON [IndexGroup];
GO

CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_1]
	ON [dbo].[dbDocument] ([Createdby], [UpdatedBy], [docDeleted], [docID], [Created])
	INCLUDE ([fileID], [docType], [docWallet], [Updated], [Opened], [docFolderGUID])
	ON [IndexGroup];
GO

CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_2]
	ON [dbo].[dbDocument] ([UpdatedBy], [docDeleted], [docID])
	INCLUDE ([fileID], [assocID], [docType], [docWallet], [docAuthored], [Createdby], [Updated])
	ON [IndexGroup];
GO

CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_3]
	ON [dbo].[dbDocument] ([UpdatedBy], [docDeleted], [Created])
	INCLUDE ([docID], [fileID], [assocID], [Createdby], [Updated], [docFolderGUID])
GO

CREATE TRIGGER [dbo].[tgrUpdateChildPasswords] ON [dbo].[dbDocument]
FOR UPDATE NOT FOR REPLICATION
AS

IF ( UPDATE(docPassword) OR UPDATE(docPasswordHint) )
BEGIN
	IF EXISTS ( SELECT I.docID FROM Inserted I JOIN Deleted D ON D.docID = I.docID WHERE D.docPassword <> I.docPassword OR D.docPasswordHint <> I.docPasswordHint )
	BEGIN
		UPDATE D
		SET D.docPassword = I.docPassword  , D.docPasswordHint = I.docPasswordHint 
		FROM
			-- Parent record			
			( SELECT docID , docPassword , docPasswordHint , docParent FROM Inserted ) I 
		JOIN
			-- Child record
			( SELECT docID , docPassword , docPasswordHint , docParent FROM [dbo].[dbDocument] WHERE docParent IS NOT NULL ) B ON I.docID = B.docParent
		JOIN
			[dbo].[dbDocument] D ON D.docID = B.docID
		WHERE
			B.docPassword IS NULL
		OR
			( I.docPassword <> B.docPassword )
	END
END

GO


CREATE TRIGGER [dbo].[tgrDefaultDocumentInfo] ON [dbo].[dbDocument] 
FOR INSERT  NOT FOR REPLICATION
AS

declare @id bigint
declare @type ucodelookup
declare @ext nvarchar(15)
declare @loc smallint
declare @app smallint

select @id = docid, @type = doctype, @ext = docextension, @loc = doclocation, @app = docappid from inserted

if @ext is null or @loc is null or @app is null or @ext = ''
begin
	declare @newext nvarchar(15)
	declare @newloc smallint
	select @newext = typefileext, @newloc =  typedefaultstorage, @app = typedefaultapp from dbdocumenttype where typecode = @type
	update dbdocument set docextension = coalesce(docextension, @newext), doclocation = coalesce(doclocation, @newloc), docappid = coalesce(docappid, @app)  where docid = @id
end

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


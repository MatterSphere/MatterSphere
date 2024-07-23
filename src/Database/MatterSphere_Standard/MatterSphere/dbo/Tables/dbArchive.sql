CREATE TABLE [dbo].[dbArchive] (
    [archID]          BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [clID]            BIGINT              NULL,
    [fileID]          BIGINT              NULL,
    [archType]        [dbo].[uCodeLookup] NULL,
    [archRef]         NVARCHAR (30)       NULL,
    [archDesc]        NVARCHAR (50)       NULL,
    [archAuthBy]      [dbo].[uCreatedBy]  NULL,
    [archNote]        NVARCHAR (1000)      NULL,
    [archInStorage]   BIT                 NULL,
    [archStorageLoc]  [dbo].[uCodeLookup] NULL,
    [Created]         [dbo].[uCreated]    CONSTRAINT [DF_dbArchive_Created]  DEFAULT (getutcdate()) NULL,
    [CreatedBy]       [dbo].[uCreatedBy]  NULL,
    [archDeleted]     BIT                 CONSTRAINT [DF_dbArchive_archDeleted] DEFAULT ((0)) NOT NULL,
    [archDeletedDate] DATETIME            NULL,
    [archDeletedBy]   [dbo].[uCreatedBy]  NULL,
    [archDestroyDate] DATETIME            NULL,
    [archActive]      SMALLINT            CONSTRAINT [DF_dbArchive_archActive] DEFAULT ((1)) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbArchive_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
	[archWillDate]    DATETIME            NULL,
	[archPOADate]     DATETIME            NULL,
	[archStorageDate] DATETIME            NULL,
	[archCodicil1]    DATETIME            NULL,
	[archCodicil2]    DATETIME            NULL,
	[archCodicil3]    DATETIME            NULL,
	[archCodicil4]    DATETIME            NULL,
	[archGeneralType] [dbo].[uCodeLookup] NULL,
	[archExtLinkID]   NVARCHAR (20)       NULL,
	[archRefOther]    NVARCHAR (20)       NULL,
	[archStorageLocationText] NVARCHAR (50) NULL,
	[archEmbeddedFile] NVARCHAR (MAX)     NULL,
	[archDocRef]      NVARCHAR (15)       NULL,
	[archProbateGranted] BIT              NULL,
	[archProvenance]  NVARCHAR (50)       NULL,
    CONSTRAINT [PK_dbArchive] PRIMARY KEY CLUSTERED ([archID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbArchive_rowguid]
    ON [dbo].[dbArchive]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbArchive] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbArchive] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbArchive] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbArchive] TO [OMSApplicationRole]
    AS [dbo];


CREATE TABLE [dbo].[dbFolderStructure] (
    [flID]       INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [flParentID] INT                 NOT NULL,
    [flCode]     [dbo].[uCodeLookup] NOT NULL,
    [flType]     [dbo].[uCodeLookup] CONSTRAINT [DF_dbFolderStructure_flType] DEFAULT (N'Folder') NULL,
    [ftIntType]  NVARCHAR (50)       NULL,
    [flForward]  INT                 NULL,
    [fldIcon]    SMALLINT            CONSTRAINT [DF_dbFolderStructure_fldIcon] DEFAULT ((12)) NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFolderStructure_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFolderStructures] PRIMARY KEY CLUSTERED ([flID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFolderStructure_rowguid]
    ON [dbo].[dbFolderStructure]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFolderStructure] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFolderStructure] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFolderStructure] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFolderStructure] TO [OMSApplicationRole]
    AS [dbo];


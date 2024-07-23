CREATE TABLE [dbo].[dbFolderLinks] (
    [fllnkid]  INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [flparent] INT                 CONSTRAINT [DF_dbFolderLinks_flparent] DEFAULT ((0)) NOT NULL,
    [flid]     INT                 NOT NULL,
    [id]       BIGINT              NOT NULL,
    [fldType]  [dbo].[uCodeLookup] NOT NULL,
    [fldIcon]  SMALLINT            CONSTRAINT [DF_dbFolderLinks_fldIcon] DEFAULT ((18)) NULL,
    [rowguid]  UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFolderLinks_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFolderLinks] PRIMARY KEY CLUSTERED ([fllnkid] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFolderLinks_rowguid]
    ON [dbo].[dbFolderLinks]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFolderLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFolderLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFolderLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFolderLinks] TO [OMSApplicationRole]
    AS [dbo];


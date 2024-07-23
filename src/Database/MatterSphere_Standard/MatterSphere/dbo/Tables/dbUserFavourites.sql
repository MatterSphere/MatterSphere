CREATE TABLE [dbo].[dbUserFavourites] (
    [FavID]           INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [usrID]           INT              NOT NULL,
    [usrFavType]      NVARCHAR (50)    NOT NULL,
    [usrFavDesc]      NVARCHAR (400)   NULL,
    [usrFavGlyph]     NVARCHAR (50)    NULL,
    [usrFavObjParam1] NVARCHAR (1000)  NULL,
    [usrFavObjParam2] NVARCHAR (50)    NULL,
    [usrFavObjParam3] NVARCHAR (1000)    NULL,
    [usrFavObjParam4] INT              NULL,
    [usrFavDept]      NVARCHAR (50)    NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_dbUserFavourites_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbUserFavourites] PRIMARY KEY CLUSTERED ([FavID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbUserFavourites_dbUser] FOREIGN KEY ([usrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUserFavourites_rowguid]
    ON [dbo].[dbUserFavourites]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbUserFavourites_usrFavType]
    ON [dbo].[dbUserFavourites]([usrFavType] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbUserFavourites] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbUserFavourites] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbUserFavourites] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbUserFavourites] TO [OMSApplicationRole]
    AS [dbo];


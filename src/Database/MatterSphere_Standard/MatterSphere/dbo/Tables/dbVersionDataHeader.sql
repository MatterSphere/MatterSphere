CREATE TABLE [dbo].[dbVersionDataHeader] (
    [versionID]       UNIQUEIDENTIFIER NOT NULL,
    [versionLinks]    XML              NULL,
    [versionComments] NVARCHAR (MAX)   NULL,
    [Created]         DATETIME         NULL,
    [CreatedBy]       INT              NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_dbVersionDataHeader_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbVersionDataHeader] PRIMARY KEY CLUSTERED ([versionID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbVersionDataHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbVersionDataHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbVersionDataHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbVersionDataHeader] TO [OMSApplicationRole]
    AS [dbo];


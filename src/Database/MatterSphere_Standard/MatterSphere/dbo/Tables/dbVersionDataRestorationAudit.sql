CREATE TABLE [dbo].[dbVersionDataRestorationAudit] (
    [restoreID]           BIGINT           IDENTITY (1, 1) NOT NULL,
    [code]                NVARCHAR (100)   NULL,
    [objecttype]          NVARCHAR (30)    NULL,
    [restored]            DATETIME         NULL,
    [restoredby]          BIGINT           NULL,
    [restoredfromversion] NVARCHAR (50)    NULL,
    [restoredtoversion]   NVARCHAR (50)    NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_dbVersionDataRestorationAudit_rowgui] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_dbVersionDataRestorationAudit] PRIMARY KEY CLUSTERED ([restoreID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbVersionDataRestorationAudit] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbVersionDataRestorationAudit] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbVersionDataRestorationAudit] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbVersionDataRestorationAudit] TO [OMSApplicationRole]
    AS [dbo];


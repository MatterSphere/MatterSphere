CREATE TABLE [dbo].[dbPackages] (
    [pkgCode]         [dbo].[uCodeLookup] NOT NULL,
    [pkgExportLoc]    NVARCHAR (256)      NULL,
    [pkgXML]          [dbo].[uXML]        NULL,
    [pkgAuthor]       NVARCHAR (100)      NULL,
    [pkgVersion]      NVARCHAR (10)       NULL,
    [pkgSigned]       UNIQUEIDENTIFIER    NULL,
    [Created]         [dbo].[uCreated]    CONSTRAINT [DF_dbPackages_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       [dbo].[uCreatedBy]  NULL,
    [Updated]         [dbo].[uCreated]    NULL,
    [UpdatedBy]       [dbo].[uCreatedBy]  NULL,
    [pkgExternal]     BIT                 CONSTRAINT [DF_dbPackages_pkgExternal] DEFAULT ((0)) NOT NULL,
    [pkgDependencies] NVARCHAR (1000)     NULL,
    [pkgLicenses]     NVARCHAR (1000)     NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbPackages_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPackages] PRIMARY KEY CLUSTERED ([pkgCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPackages_rowguid]
    ON [dbo].[dbPackages]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrPackagesUpdated]
   ON  [dbo].[dbPackages] 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	exec sprTableMonitorUpdate 'dbPackages'    
END



GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPackages] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPackages] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPackages] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPackages] TO [OMSApplicationRole]
    AS [dbo];


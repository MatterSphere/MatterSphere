CREATE TABLE [dbo].[dbAssembly] (
    [ID]             NVARCHAR (150)      NOT NULL,
    [Assembly]       NVARCHAR (100)      NOT NULL,
    [Attached]       NVARCHAR (MAX)      NOT NULL,
    [SourceFileName] NVARCHAR (250)      NOT NULL,
    [Modified]       DATETIME            NOT NULL,
    [Version]        NVARCHAR (50)       NOT NULL,
    [OMSVersion]     NVARCHAR (50)       NOT NULL,
    [XML]            NVARCHAR (MAX)      NULL,
    [Created]        [dbo].[uCreated]    NULL,
    [CreatedBy]      [dbo].[uCreatedBy]  NULL,
    [Updated]        [dbo].[uCreated]    NULL,
    [UpdatedBy]      [dbo].[uCreatedBy]  NULL,
    [PackageType]    [dbo].[uCodeLookup] NULL,
    [rowguid]        UNIQUEIDENTIFIER    CONSTRAINT [DF_dbAssembly_Rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbAssembly] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAssembly_rowguid]
    ON [dbo].[dbAssembly]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrAssemblyUpdated]
   ON  [dbo].[dbAssembly] 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	exec sprTableMonitorUpdate 'dbAssembly'    
END


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAssembly] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAssembly] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAssembly] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAssembly] TO [OMSApplicationRole]
    AS [dbo];


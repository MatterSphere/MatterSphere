CREATE TABLE [dbo].[dbPackageData] (
    [pkdID]           INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [pkdCode]         NVARCHAR (50)       NOT NULL,
    [pkdSourceType]   [dbo].[uCodeLookup] CONSTRAINT [DF_dbPackageData_pkdSourceType] DEFAULT (N'OMS') NOT NULL,
    [pkdSource]       NVARCHAR (500)      NULL,
    [pkdCall]         NVARCHAR (2500)     NULL,
    [pkdParameters]   [dbo].[uXML]        CONSTRAINT [DF_dbPackageData_pkdParameters] DEFAULT (N'<params></params>') NULL,
    [pkdUpdateSelect] NVARCHAR (2000)     NULL,
    [pkdDataSet]      [dbo].[uXML]        NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbPackageData_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPackageData] PRIMARY KEY CLUSTERED ([pkdID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPackageData_rowguid]
    ON [dbo].[dbPackageData]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPackageData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPackageData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPackageData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPackageData] TO [OMSApplicationRole]
    AS [dbo];


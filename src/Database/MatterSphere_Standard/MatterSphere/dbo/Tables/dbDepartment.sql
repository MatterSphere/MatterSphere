CREATE TABLE [dbo].[dbDepartment] (
    [deptCode]    [dbo].[uCodeLookup] NOT NULL,
    [deptEmail]   [dbo].[uEmail]      NULL,
    [deptActive]  BIT                 CONSTRAINT [DF_dbDepartment_deptActive] DEFAULT ((1)) NOT NULL,
    [deptAccCode] NVARCHAR (16)       NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDepartment_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deptXML]     NVARCHAR (MAX)      CONSTRAINT [DF_dbDepartment_deptXML] DEFAULT ('<config/>') NOT NULL,
    CONSTRAINT [PK_dbDepartment] PRIMARY KEY CLUSTERED ([deptCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDepartment_rowguid]
    ON [dbo].[dbDepartment]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDepartment] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDepartment] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDepartment] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDepartment] TO [OMSApplicationRole]
    AS [dbo];


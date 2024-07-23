CREATE TABLE [dbo].[dbTableMonitor] (
    [TableName]   NVARCHAR (50)    NOT NULL,
    [Category]    NVARCHAR (50)    NOT NULL,
    [LastUpdated] DATETIME         NULL,
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_dbTableMonitor_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbTableMonitor] PRIMARY KEY CLUSTERED ([rowguid] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbTableMonitor]
    ON [dbo].[dbTableMonitor]([TableName] ASC, [Category] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbTableMonitor] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbTableMonitor] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbTableMonitor] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbTableMonitor] TO [OMSApplicationRole]
    AS [dbo];


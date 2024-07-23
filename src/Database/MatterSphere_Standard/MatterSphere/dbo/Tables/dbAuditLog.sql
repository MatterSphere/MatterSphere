CREATE TABLE [dbo].[dbAuditLog] (
    [SerialNo] BIGINT           NOT NULL,
    [License]  NVARCHAR (50)    NOT NULL,
    [Database] NVARCHAR (50)    NULL,
    [Server]   NVARCHAR (50)    NULL,
    [MaxCount] BIGINT           NULL,
    [Month]    TINYINT          NOT NULL,
    [Year]     INT              NOT NULL,
    [Checksum] VARBINARY (50)   NULL,
    [RowGuid]  UNIQUEIDENTIFIER CONSTRAINT [DF_dbAuditLog_RowGuid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbAuditLog] PRIMARY KEY CLUSTERED ([RowGuid] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAuditLog_rowguid]
    ON [dbo].[dbAuditLog]([RowGuid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAuditLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAuditLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAuditLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAuditLog] TO [OMSApplicationRole]
    AS [dbo];


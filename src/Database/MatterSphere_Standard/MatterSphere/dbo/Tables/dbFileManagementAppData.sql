CREATE TABLE [dbo].[dbFileManagementAppData] (
    [fileid]  BIGINT              NOT NULL,
    [appcode] [dbo].[uCodeLookup] NOT NULL,
    [rowguid] UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFileManagementAppData_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFileManagementAppData] PRIMARY KEY CLUSTERED ([fileid] ASC),
    CONSTRAINT [FK_dbFileManagementAppData_dbFile] FOREIGN KEY ([fileid]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFileManagementAppData_dbFileManagementApplication] FOREIGN KEY ([appcode]) REFERENCES [dbo].[dbFileManagementApplication] ([appCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFileManagementAppData_rowguid]
    ON [dbo].[dbFileManagementAppData]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFileManagementAppData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFileManagementAppData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFileManagementAppData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFileManagementAppData] TO [OMSApplicationRole]
    AS [dbo];


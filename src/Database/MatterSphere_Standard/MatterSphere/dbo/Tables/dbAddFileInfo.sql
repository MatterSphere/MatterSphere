CREATE TABLE [dbo].[dbAddFileInfo] (
    [fileID]      BIGINT             NOT NULL,
    [fileField1]  NVARCHAR (60)      NULL,
    [fileField2]  NVARCHAR (60)      NULL,
    [fileField3]  NVARCHAR (60)      NULL,
    [fileField4]  NVARCHAR (60)      NULL,
    [fileField5]  NVARCHAR (60)      NULL,
    [fileField6]  NVARCHAR (60)      NULL,
    [fileField7]  NVARCHAR (60)      NULL,
    [fileField8]  NVARCHAR (60)      NULL,
    [fileField9]  NVARCHAR (60)      NULL,
    [fileField10] NVARCHAR (60)      NULL,
    [fileDate1]   DATETIME           NULL,
    [fileDate2]   DATETIME           NULL,
    [fileDate3]   DATETIME           NULL,
    [fileDate4]   DATETIME           NULL,
    [fileDate5]   DATETIME           NULL,
    [Updated]     [dbo].[uCreated]   NULL,
    [UpdatedBy]   [dbo].[uCreatedBy] NULL,
    [rowguid]     UNIQUEIDENTIFIER   CONSTRAINT [DF_dbAddFileInfo_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbAddFileInfo] PRIMARY KEY CLUSTERED ([fileID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbAddFileInfo_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAddFileInfo_rowguid]
    ON [dbo].[dbAddFileInfo]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAddFileInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAddFileInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAddFileInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAddFileInfo] TO [OMSApplicationRole]
    AS [dbo];


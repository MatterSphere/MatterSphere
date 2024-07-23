CREATE TABLE [dbo].[dbAddClientInfo] (
    [clID]      BIGINT             NOT NULL,
    [clField1]  NVARCHAR (50)      NULL,
    [clField2]  NVARCHAR (50)      NULL,
    [clField3]  NVARCHAR (50)      NULL,
    [clField4]  NVARCHAR (50)      NULL,
    [clField5]  NVARCHAR (50)      NULL,
    [clField6]  NVARCHAR (50)      NULL,
    [clField7]  NVARCHAR (50)      NULL,
    [clField8]  NVARCHAR (50)      NULL,
    [clField9]  NVARCHAR (50)      NULL,
    [clField10] NVARCHAR (50)      NULL,
    [clDate1]   DATETIME           NULL,
    [clDate2]   DATETIME           NULL,
    [clDate3]   DATETIME           NULL,
    [clDate4]   DATETIME           NULL,
    [clDate5]   DATETIME           NULL,
    [Updated]   [dbo].[uCreated]   NULL,
    [UpdatedBy] [dbo].[uCreatedBy] NULL,
    [rowguid]   UNIQUEIDENTIFIER   CONSTRAINT [DF_dbAddClientInfo_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbAddClientInfo] PRIMARY KEY CLUSTERED ([clID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbAddClientInfo_dbClient] FOREIGN KEY ([clID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAddClientInfo_rowguid]
    ON [dbo].[dbAddClientInfo]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAddClientInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAddClientInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAddClientInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAddClientInfo] TO [OMSApplicationRole]
    AS [dbo];


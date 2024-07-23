CREATE TABLE [dbo].[dbContactCompany] (
    [contID]          BIGINT              NOT NULL,
    [contRegoffaddID] BIGINT              NULL,
    [contCoSecretary] NVARCHAR (50)       NULL,
    [contRegCoName]   NVARCHAR (80)       NULL,
    [contRegNo]       NVARCHAR (50)       NULL,
    [contVatRegNo]    NVARCHAR (50)       NULL,
    [contShareCap]    NVARCHAR (50)       NULL,
    [contBusActivity] NVARCHAR (50)       NULL,
    [contNoofEmp]     INT                 NULL,
    [contNoofBranch]  INT                 NULL,
    [contDateofInc]   DATETIME            NULL,
    [contFiscalYE]    DATETIME            NULL,
    [contCorpType]    [dbo].[uCodeLookup] NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactCompany_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbClientCompany] PRIMARY KEY CLUSTERED ([contID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContactCompany_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactCompany_rowguid]
    ON [dbo].[dbContactCompany]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactCompany] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactCompany] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactCompany] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactCompany] TO [OMSApplicationRole]
    AS [dbo];


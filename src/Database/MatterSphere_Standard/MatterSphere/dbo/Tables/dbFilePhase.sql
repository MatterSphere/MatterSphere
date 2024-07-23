CREATE TABLE [dbo].[dbFilePhase] (
    [phID]      BIGINT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]    BIGINT             NOT NULL,
    [phNo]      INT                NULL,
    [phDesc]    NVARCHAR (500)     NULL,
    [Created]   [dbo].[uCreated]   CONSTRAINT [DF_dbFilePhase_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy] [dbo].[uCreatedBy] NULL,
    [Updated]   [dbo].[uCreated]   NULL,
    [UpdatedBy] [dbo].[uCreatedBy] NULL,
    [phActive]  BIT                CONSTRAINT [DF_dbFilePhase_phActive] DEFAULT ((1)) NOT NULL,
    [rowguid]   UNIQUEIDENTIFIER   CONSTRAINT [DF_dbFilePhase_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFilePhase] PRIMARY KEY CLUSTERED ([phID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFilePhase_rowguid]
    ON [dbo].[dbFilePhase]([rowguid] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFilePhase] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFilePhase] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFilePhase] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFilePhase] TO [OMSApplicationRole]
    AS [dbo];


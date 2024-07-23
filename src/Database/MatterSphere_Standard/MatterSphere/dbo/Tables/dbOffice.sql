CREATE TABLE [dbo].[dbOffice] (
    [offID]     INT                IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [brID]      INT                NOT NULL,
    [offCode]   NVARCHAR (15)      NOT NULL,
    [Created]   [dbo].[uCreated]   NOT NULL,
    [CreatedBy] [dbo].[uCreatedBy] NULL,
    [Updated]   [dbo].[uCreated]   NOT NULL,
    [UpdatedBy] [dbo].[uCreatedBy] NULL,
    [rowguid]   UNIQUEIDENTIFIER   CONSTRAINT [DF_dbOffice_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbOffice] PRIMARY KEY CLUSTERED ([offID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbOffice_dbBranch] FOREIGN KEY ([brID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbOffice_rowguid]
    ON [dbo].[dbOffice]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbOffice] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbOffice] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbOffice] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbOffice] TO [OMSApplicationRole]
    AS [dbo];


CREATE TABLE [dbo].[dbContactEmployees] (
    [contCompany]  BIGINT           NOT NULL,
    [contEmployee] BIGINT           NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_dbContactEmployees_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactEmployees] PRIMARY KEY CLUSTERED ([contCompany] ASC, [contEmployee] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContactEmployees_dbContactCompany] FOREIGN KEY ([contCompany]) REFERENCES [dbo].[dbContactCompany] ([contID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbContactEmployees_dbContactIndividual] FOREIGN KEY ([contEmployee]) REFERENCES [dbo].[dbContactIndividual] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactEmployees_rowguid]
    ON [dbo].[dbContactEmployees]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactEmployees] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactEmployees] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactEmployees] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactEmployees] TO [OMSApplicationRole]
    AS [dbo];


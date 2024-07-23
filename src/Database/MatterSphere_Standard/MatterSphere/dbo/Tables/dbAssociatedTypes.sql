CREATE TABLE [dbo].[dbAssociatedTypes] (
    [contType]  [dbo].[uCodeLookup] NOT NULL,
    [assocType] [dbo].[uCodeLookup] NOT NULL,
    [rowguid]   UNIQUEIDENTIFIER    CONSTRAINT [DF_dbAssociatedTypes_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbAssociatedTypes] PRIMARY KEY CLUSTERED ([contType] ASC, [assocType] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbAssociatedTypes_dbAssociateType] FOREIGN KEY ([assocType]) REFERENCES [dbo].[dbAssociateType] ([typeCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAssociatedTypes_rowguid]
    ON [dbo].[dbAssociatedTypes]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAssociatedTypes] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAssociatedTypes] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAssociatedTypes] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAssociatedTypes] TO [OMSApplicationRole]
    AS [dbo];


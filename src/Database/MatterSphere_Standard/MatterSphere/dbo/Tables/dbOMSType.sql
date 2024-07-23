CREATE TABLE [dbo].[dbOMSType] (
    [objCode] [dbo].[uCodeLookup] NOT NULL,
    [objType] NVARCHAR (100)      NOT NULL,
    [rowguid] UNIQUEIDENTIFIER    CONSTRAINT [DF_dbOMSType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbOMSType] PRIMARY KEY CLUSTERED ([objCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbOMSType_rowguid]
    ON [dbo].[dbOMSType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbOMSType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbOMSType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbOMSType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbOMSType] TO [OMSApplicationRole]
    AS [dbo];


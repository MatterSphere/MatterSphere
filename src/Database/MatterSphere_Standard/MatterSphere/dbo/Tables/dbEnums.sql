CREATE TABLE [dbo].[dbEnums] (
    [EnumItem] NVARCHAR (50)    NOT NULL,
    [EnumName] NVARCHAR (50)    NOT NULL,
    [rowguid]  UNIQUEIDENTIFIER CONSTRAINT [DF_dbEnums_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbEnums] PRIMARY KEY CLUSTERED ([EnumItem] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnums_rowguid]
    ON [dbo].[dbEnums]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnums] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnums] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnums] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnums] TO [OMSApplicationRole]
    AS [dbo];


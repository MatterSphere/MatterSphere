CREATE TABLE [dbo].[dbXSD] (
    [xsdCode] [dbo].[uCodeLookup] NOT NULL,
    [xsdText] [dbo].[uXML]        NULL,
    [rowguid] UNIQUEIDENTIFIER    CONSTRAINT [DF_dbXSD_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbXSD] PRIMARY KEY CLUSTERED ([xsdCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbXSD_rowguid]
    ON [dbo].[dbXSD]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbXSD] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbXSD] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbXSD] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbXSD] TO [OMSApplicationRole]
    AS [dbo];


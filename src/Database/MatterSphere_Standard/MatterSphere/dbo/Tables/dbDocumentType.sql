CREATE TABLE [dbo].[dbDocumentType] (
    [typeCode]           [dbo].[uCodeLookup] NOT NULL,
    [typeDefaultStorage] SMALLINT            CONSTRAINT [DF_dbDocumentType_typeDefaultStorage] DEFAULT ((0)) NOT NULL,
    [typeFileExt]        NVARCHAR (15)       NOT NULL,
    [typePrecExt]        NVARCHAR (15)       NOT NULL,
    [typeDefaultApp]     SMALLINT            CONSTRAINT [DF_dbDocumentType_typeDefaultApp] DEFAULT ((0)) NOT NULL,
    [typeXml]            NVARCHAR (MAX)      CONSTRAINT [DF_dbDocumentType_typeXML] DEFAULT (N'<Config/>') NOT NULL,
    [rowguid]            UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDocumentType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [typeDocSupports]    INT                 CONSTRAINT [DF_dbDocumentType_typeDocSupports] DEFAULT ((0)) NOT NULL,
    [typePrecSupports]   INT                 CONSTRAINT [DF_dbDocumentType_typePrecSupports] DEFAULT ((0)) NOT NULL,
    [typeActive]         BIGINT              CONSTRAINT [DF_dbDocumentType_typeActive] DEFAULT ((1)) NOT NULL,
    [typeGlyph]          BIGINT              CONSTRAINT [DF_dbDocumentType_typeGlyph] DEFAULT ((1)) NOT NULL,
    [typeVersion]        BIGINT              CONSTRAINT [DF_dbDocumentType_typeVersion] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_dbDocumentType] PRIMARY KEY CLUSTERED ([typeCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbDocumentType_dbApplication] FOREIGN KEY ([typeDefaultApp]) REFERENCES [dbo].[dbApplication] ([appID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocumentType_dbStorageProvider] FOREIGN KEY ([typeDefaultStorage]) REFERENCES [dbo].[dbStorageProvider] ([spID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocumentType_rowguid]
    ON [dbo].[dbDocumentType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentType] TO [OMSApplicationRole]
    AS [dbo];


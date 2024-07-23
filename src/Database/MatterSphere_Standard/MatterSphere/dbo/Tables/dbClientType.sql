CREATE TABLE [dbo].[dbClientType] (
    [typeCode]    [dbo].[uCodeLookup] NOT NULL,
    [typeVersion] BIGINT              CONSTRAINT [DF_dbClientType_typeVersion] DEFAULT ((0)) NOT NULL,
    [typeXML]     [dbo].[uXML]        CONSTRAINT [DF_dbClientType_typeXML] DEFAULT (N'<Config/>') NOT NULL,
    [typeGlyph]   INT                 CONSTRAINT [DF_dbClientType_typeGlyph] DEFAULT ((-1)) NOT NULL,
    [typeSeed]    [dbo].[uCodeLookup] NULL,
    [typeActive]  BIT                 CONSTRAINT [DF_dbClientType_typeActive] DEFAULT ((1)) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbClientType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbClientType] PRIMARY KEY CLUSTERED ([typeCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbClientType_rowguid]
    ON [dbo].[dbClientType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbClientType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbClientType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbClientType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbClientType] TO [OMSApplicationRole]
    AS [dbo];


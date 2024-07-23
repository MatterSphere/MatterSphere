CREATE TABLE [dbo].[dbContactType] (
    [typeCode]    [dbo].[uCodeLookup] NOT NULL,
    [typeVersion] BIGINT              CONSTRAINT [DF_dbContactType_typeVersion] DEFAULT ((0)) NOT NULL,
    [typeXML]     [dbo].[uXML]        CONSTRAINT [DF_dbContactType_typeXML] DEFAULT (N'<Config/>') NOT NULL,
    [typeGlyph]   INT                 CONSTRAINT [DF_dbContactType_typeGlyph] DEFAULT ((-1)) NOT NULL,
    [typeActive]  BIT                 CONSTRAINT [DF_dbContactType_Active] DEFAULT ((1)) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactType] PRIMARY KEY CLUSTERED ([typeCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactType_rowguid]
    ON [dbo].[dbContactType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactType] TO [OMSApplicationRole]
    AS [dbo];


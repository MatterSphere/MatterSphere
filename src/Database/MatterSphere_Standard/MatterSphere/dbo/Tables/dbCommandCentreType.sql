CREATE TABLE [dbo].[dbCommandCentreType] (
    [typeCode]    [dbo].[uCodeLookup] NOT NULL,
    [typeVersion] BIGINT              CONSTRAINT [DF_dbConfigType_typeVersion] DEFAULT ((0)) NOT NULL,
    [typeXML]     [dbo].[uXML]        CONSTRAINT [DF_dbConfigType_typeXML] DEFAULT (N'<Config/>') NOT NULL,
    [typeGlyph]   INT                 CONSTRAINT [DF_dbConfigType_typeGlyph] DEFAULT ((-1)) NOT NULL,
    [typeActive]  BIT                 CONSTRAINT [DF_dbConfigType_typeActive] DEFAULT ((1)) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbCommandCentreType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCommandCentreType] PRIMARY KEY CLUSTERED ([typeCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCommandCentreType_rowguid]
    ON [dbo].[dbCommandCentreType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCommandCentreType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCommandCentreType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCommandCentreType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCommandCentreType] TO [OMSApplicationRole]
    AS [dbo];


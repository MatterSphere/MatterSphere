CREATE TABLE [dbo].[dbUserType] (
    [typeCode]    [dbo].[uCodeLookup] NOT NULL,
    [typeVersion] BIGINT              CONSTRAINT [DF_dbUserType_typeVersion] DEFAULT ((0)) NOT NULL,
    [typeXML]     [dbo].[uXML]        CONSTRAINT [DF_dbUserType_typeXML] DEFAULT (N'<Config/>') NOT NULL,
    [typeGlyph]   INT                 CONSTRAINT [DF_dbUserType_typeGlyph] DEFAULT ((-1)) NOT NULL,
    [typeActive]  BIT                 CONSTRAINT [DF_dbUserType_typeActive] DEFAULT ((1)) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbUserType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbUserType] PRIMARY KEY CLUSTERED ([typeCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUserType_rowguid]
    ON [dbo].[dbUserType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbUserType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbUserType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbUserType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbUserType] TO [OMSApplicationRole]
    AS [dbo];


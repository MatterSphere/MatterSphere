CREATE TABLE [dbo].[dbAssociateType] (
    [typeCode]    [dbo].[uCodeLookup] NOT NULL,
    [typeVersion] BIGINT              CONSTRAINT [DF_dbAssociateType_typeVersion] DEFAULT ((1)) NOT NULL,
    [typeXML]     [dbo].[uXML]        CONSTRAINT [DF_dbAssociateType_typeXML] DEFAULT (N'<Config/>') NOT NULL,
    [typeGlyph]   INT                 CONSTRAINT [DF_dbAssociateType_typeGlyph] DEFAULT ((-1)) NOT NULL,
    [typeActive]  BIT                 CONSTRAINT [DF_dbAssociateType_typeActive] DEFAULT ((1)) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbAssociateType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbAssociateType] PRIMARY KEY CLUSTERED ([typeCode] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAssociateType_rowguid]
    ON [dbo].[dbAssociateType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAssociateType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAssociateType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAssociateType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAssociateType] TO [OMSApplicationRole]
    AS [dbo];


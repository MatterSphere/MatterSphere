CREATE TABLE [dbo].[dbFeeEarnerType] (
    [typeCode]    [dbo].[uCodeLookup] NOT NULL,
    [typeVersion] BIGINT              CONSTRAINT [DF_dbFeeEarnerType_typeVersion] DEFAULT ((1)) NOT NULL,
    [typeXML]     [dbo].[uXML]        CONSTRAINT [DF_dbFeeEarnerType_typeXML] DEFAULT (N'<Config/>') NOT NULL,
    [typeGlyph]   INT                 CONSTRAINT [DF_dbFeeEarnerType_typeGlyph] DEFAULT ((-1)) NOT NULL,
    [typeActive]  BIT                 CONSTRAINT [DF_dbFeeEarnerType_typeActive] DEFAULT ((1)) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFeeEarnerType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFeeEarnerType] PRIMARY KEY CLUSTERED ([typeCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFeeEarnerType_rowguid]
    ON [dbo].[dbFeeEarnerType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFeeEarnerType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFeeEarnerType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFeeEarnerType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFeeEarnerType] TO [OMSApplicationRole]
    AS [dbo];


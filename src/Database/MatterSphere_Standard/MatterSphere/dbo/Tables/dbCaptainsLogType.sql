CREATE TABLE [dbo].[dbCaptainsLogType] (
    [typeID]       SMALLINT            NOT NULL,
    [typeCode]     [dbo].[uCodeLookup] NOT NULL,
    [typeHelpURL]  [dbo].[uURL]        NULL,
    [typeSeverity] TINYINT             CONSTRAINT [DF_dbCaptainsLogType_typeSeverity] DEFAULT ((10)) NOT NULL,
    [typeGroup]    [dbo].[uCodeLookup] NULL,
    [typeSystem]   BIT                 CONSTRAINT [DF_dbCaptainsLogType_typeSystem] DEFAULT ((0)) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER    CONSTRAINT [DF_dbCaptainsLogType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCaptainsLogType] PRIMARY KEY CLUSTERED ([typeID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCaptainsLogType_rowguid]
    ON [dbo].[dbCaptainsLogType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCaptainsLogType_typeCode]
    ON [dbo].[dbCaptainsLogType]([typeCode] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCaptainsLogType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCaptainsLogType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCaptainsLogType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCaptainsLogType] TO [OMSApplicationRole]
    AS [dbo];


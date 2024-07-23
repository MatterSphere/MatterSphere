CREATE TABLE [config].[SystemPolicy] (
    [ID]        UNIQUEIDENTIFIER    CONSTRAINT [DF_SystemPolicy_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Type]      [dbo].[uCodeLookup] NOT NULL,
    [AllowMask] VARBINARY (32)      NULL,
    [DenyMask]  VARBINARY (32)      NULL,
    [Name]      NVARCHAR (100)      NULL,
    [IsDefault] BIT                 NULL,
    CONSTRAINT [PK_SystemPolicy] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[config].[SystemPolicy] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[SystemPolicy] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[SystemPolicy] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[SystemPolicy] TO [OMSApplicationRole]
    AS [dbo];


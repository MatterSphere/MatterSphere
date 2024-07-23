CREATE TABLE [config].[SystemPolicyConfig] (
    [Number]        SMALLINT            NOT NULL,
    [Byte]          TINYINT             NOT NULL,
    [BitValue]      TINYINT             NOT NULL,
    [SecurableType] [dbo].[uCodeLookup] NULL,
    [Permission]    [dbo].[uCodeLookup] NULL,
    [Temp]          BIT                 NULL,
    [MajorType]     BIT                 NULL,
    [NodeLevel]     TINYINT             NULL,
    [rowguid]       UNIQUEIDENTIFIER    CONSTRAINT [DF_SystemPolicyConfig_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_SystemPolicyConfig] PRIMARY KEY CLUSTERED ([Number] ASC)
);




GO

CREATE NONCLUSTERED INDEX [IX_SystemPolicyConfig_Byte_BitValue] ON [config].[SystemPolicyConfig]
(
	[Byte] ASC,
	[BitValue] ASC,
	[SecurableType] ASC,
	[Permission] ASC
)

GO

GRANT UPDATE
    ON OBJECT::[config].[SystemPolicyConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[SystemPolicyConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[SystemPolicyConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[SystemPolicyConfig] TO [OMSApplicationRole]
    AS [dbo];


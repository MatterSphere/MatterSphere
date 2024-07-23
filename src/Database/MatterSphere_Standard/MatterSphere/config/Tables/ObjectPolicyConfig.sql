CREATE TABLE [config].[ObjectPolicyConfig] (
    [Number]        SMALLINT            NOT NULL,
    [Byte]          TINYINT             NOT NULL,
    [BitValue]      TINYINT             NOT NULL,
    [SecurableType] [dbo].[uCodeLookup] NULL,
    [Permission]    [dbo].[uCodeLookup] NULL,
    [Temp]          BIT                 NULL,
    [MajorType]     BIT                 NULL,
    [NodeLevel]     DECIMAL (3, 2)      NULL,
    [rowguid]       UNIQUEIDENTIFIER    CONSTRAINT [DF_ObjectPolicyConfig_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [SecurityLevel] INT                 NULL,
    CONSTRAINT [PK_ObjectPolicyConfig] PRIMARY KEY CLUSTERED ([Number] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[config].[ObjectPolicyConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ObjectPolicyConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[ObjectPolicyConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[ObjectPolicyConfig] TO [OMSApplicationRole]
    AS [dbo];


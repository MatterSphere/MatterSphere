CREATE TABLE [config].[PolicyType] (
    [PolicyTypeCode]                  [dbo].[uCodeLookup] NOT NULL,
    [CanEdit]                         BIT                 NOT NULL,
    [CanDelete]                       BIT                 NOT NULL,
    [Created]                         DATETIME            NOT NULL,
    [CreatedBy]                       INT                 NOT NULL,
    [IsSystemPolicy]                  BIT                 NOT NULL,
    [IncludeInFilters]                BIT                 NULL,
    [IsConfigurableTypePolicy_DELETE] BIT                 NULL,
    [PolicyConfigurableType_DELETE]   [dbo].[uCodeLookup] NULL,
    [ConfigurableTypeCode_DELETE]     [dbo].[uCodeLookup] NULL,
    [rowguid]                         UNIQUEIDENTIFIER    CONSTRAINT [DF_PolicyType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_PolicyType] PRIMARY KEY CLUSTERED ([PolicyTypeCode] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[config].[PolicyType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[PolicyType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[PolicyType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[PolicyType] TO [OMSApplicationRole]
    AS [dbo];


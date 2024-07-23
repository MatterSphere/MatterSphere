CREATE TABLE [audit].[PolicyType] (
    [ID]               UNIQUEIDENTIFIER    CONSTRAINT [DF_PolicyType_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [AuditEvent]       NVARCHAR (50)       NOT NULL,
    [PolicyTypeCode]   [dbo].[uCodeLookup] NOT NULL,
    [CanEdit]          BIT                 NOT NULL,
    [CanDelete]        BIT                 NOT NULL,
    [Updated]          DATETIME            NOT NULL,
    [UpdatedBy]        INT                 NOT NULL,
    [IsSystemPolicy]   BIT                 NOT NULL,
    [IncludeInFilters] BIT                 NOT NULL,
    CONSTRAINT [PK_PolicyType] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[audit].[PolicyType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[audit].[PolicyType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[audit].[PolicyType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[audit].[PolicyType] TO [OMSApplicationRole]
    AS [dbo];


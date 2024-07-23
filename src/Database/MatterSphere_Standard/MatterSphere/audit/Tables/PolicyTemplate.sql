CREATE TABLE [audit].[PolicyTemplate] (
    [ID]             UNIQUEIDENTIFIER    CONSTRAINT [DF_PolicyTemplate_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [PolicyID]       UNIQUEIDENTIFIER    NOT NULL,
    [PolicyTypeCode] [dbo].[uCodeLookup] NOT NULL,
    [AllowMask]      BINARY (32)         NULL,
    [DenyMask]       BINARY (32)         NULL,
    [Name]           NVARCHAR (100)      NULL,
    [Updated]        DATETIME            NOT NULL,
    [UpdatedBy]      INT                 NOT NULL,
    [AuditEvent]     NVARCHAR (50)       NOT NULL,
    [IsRemote]       BIT                 NULL,
    CONSTRAINT [PK_PolicyTemplate_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[audit].[PolicyTemplate] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[audit].[PolicyTemplate] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[audit].[PolicyTemplate] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[audit].[PolicyTemplate] TO [OMSApplicationRole]
    AS [dbo];


CREATE TABLE [dbo].[dbRiskHeader] (
    [riskID]         BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [riskHeaderID]   BIGINT              NULL,
    [riskCode]       [dbo].[uCodeLookup] NULL,
    [fileID]         BIGINT              NULL,
    [riskCreatedBy]  BIGINT              NULL,
    [riskCreated]    DATETIME            CONSTRAINT [DF_dbRiskHeader_RiskCreated] DEFAULT (getutcdate()) NULL,
    [riskActive]     BIT                 CONSTRAINT [DF_dbRiskHeader_riskActive] DEFAULT ((1)) NOT NULL,
    [riskDocID]      BIGINT              NULL,
    [riskTotalScore] INT                 NULL,
    [rowguid]        UNIQUEIDENTIFIER    CONSTRAINT [DF_dbRiskHeader_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbRiskHeader] PRIMARY KEY CLUSTERED ([riskID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbRiskHeader_rowguid]
    ON [dbo].[dbRiskHeader]([rowguid] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbRiskHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbRiskHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbRiskHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbRiskHeader] TO [OMSApplicationRole]
    AS [dbo];


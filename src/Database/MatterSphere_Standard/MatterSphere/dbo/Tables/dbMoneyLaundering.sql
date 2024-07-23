CREATE TABLE [dbo].[dbMoneyLaundering] (
    [CLID]                BIGINT              NOT NULL,
    [fileID]              BIGINT              NOT NULL,
    [LaunderID]           BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [laNotes]             NVARCHAR (MAX)      NULL,
    [Created]             [dbo].[uCreated]    NOT NULL,
    [CreatedBy]           [dbo].[uCreatedBy]  NOT NULL,
    [laCompanyPosition1]  [dbo].[uCodeLookup] NULL,
    [laIdentity1a]        [dbo].[uCodeLookup] NULL,
    [laIdentity1b]        [dbo].[uCodeLookup] NULL,
    [laCompanyPosition2]  [dbo].[uCodeLookup] NULL,
    [laIdentity2a]        [dbo].[uCodeLookup] NULL,
    [laIdentity2b]        [dbo].[uCodeLookup] NULL,
    [laCashDepositAmount] MONEY               CONSTRAINT [DF_dbo_DBMoneyLaundering_LACashDepositAmount] DEFAULT ((0)) NOT NULL,
    [laCashSource]        NVARCHAR (MAX)      NULL,
    [rowguid]             UNIQUEIDENTIFIER    CONSTRAINT [DF_dbMoneyLaundering_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbo_DBMoneyLaundering] PRIMARY KEY CLUSTERED ([LaunderID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMoneyLaundering_rowguid]
    ON [dbo].[dbMoneyLaundering]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMoneyLaundering] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMoneyLaundering] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMoneyLaundering] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMoneyLaundering] TO [OMSApplicationRole]
    AS [dbo];


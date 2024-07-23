CREATE TABLE [dbo].[dbFinancialLedger] (
    [FinLogID]           BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]             BIGINT              NOT NULL,
    [postID]             INT                 NOT NULL,
    [finEntryID]         INT                 NOT NULL,
    [finItemDate]        DATETIME            NOT NULL,
    [finDesc]            NVARCHAR (50)       NOT NULL,
    [finAssocID]         BIGINT              NULL,
    [finPayName]         NVARCHAR (50)       NULL,
    [finTheirRef]        NVARCHAR (20)       NULL,
    [finValue]           MONEY               CONSTRAINT [DF_dbFinancialLedger_finValue] DEFAULT ((0)) NOT NULL,
    [finVat]             MONEY               CONSTRAINT [DF_dbFinancialLedger_finVat] DEFAULT ((0)) NOT NULL,
    [finGross]           MONEY               CONSTRAINT [DF_dbFinancialLedger_finGross] DEFAULT ((0)) NOT NULL,
    [finAuthByfeeID]     INT                 NULL,
    [finPaid]            BIT                 CONSTRAINT [DF_dbFinancialLedger_finPaid] DEFAULT ((0)) NOT NULL,
    [finExpectedPayment] DATETIME            NULL,
    [finPostRef]         NVARCHAR (20)       NULL,
    [finPayMethod]       [dbo].[uCodeLookup] NULL,
    [finScandocID]       BIGINT              NULL,
    [finTransferred]     DATETIME            NULL,
    [finCurrency]        BIT                 NULL,
    [Created]            [dbo].[uCreated]    NULL,
    [CreatedBy]          [dbo].[uCreatedBy]  NULL,
    [finNeedExport]      BIT                 CONSTRAINT [DF_dbFinancialLedger_finNeedExport] DEFAULT ((0)) NOT NULL,
    [finExported]        DATETIME            NULL,
    [finRequestExportBy] [dbo].[uCreatedBy]  NULL,
    [finAuthorised]      DATETIME            NULL,
    [finFailureCode]     [dbo].[uCodeLookup] NULL,
    [finRequestAuthBy]   INT                 NULL,
    [rowguid]            UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFinancialLedger_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFinancialLedger] PRIMARY KEY CLUSTERED ([FinLogID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbFinancialLedger_dbAssociates] FOREIGN KEY ([finAssocID]) REFERENCES [dbo].[dbAssociates] ([assocID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFinancialLedger_dbFeeEarner] FOREIGN KEY ([finAuthByfeeID]) REFERENCES [dbo].[dbFeeEarner] ([feeusrID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFinancialLedger_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFinancialLedger_dbPostingEntryType] FOREIGN KEY ([finEntryID]) REFERENCES [dbo].[dbPostingEntryType] ([postID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFinancialLedger_dbPostingType] FOREIGN KEY ([postID]) REFERENCES [dbo].[dbPostingType] ([postID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFinancialLedger_rowguid]
    ON [dbo].[dbFinancialLedger]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFinancialLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFinancialLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFinancialLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFinancialLedger] TO [OMSApplicationRole]
    AS [dbo];


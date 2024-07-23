CREATE TABLE [dbo].[dbFundType] (
    [ftCode]            [dbo].[uCodeLookup] NOT NULL,
    [ftcurISOCode]      CHAR (3)            CONSTRAINT [DF_dbFundType_ftcurISOCode] DEFAULT ('GBP') NOT NULL,
    [ftCreditLimit]     MONEY               CONSTRAINT [DF_dbFundType_ftCreditLimit] DEFAULT ((0)) NOT NULL,
    [ftCLCode]          [dbo].[uCodeLookup] NOT NULL,
    [ftRefCode]         [dbo].[uCodeLookup] NOT NULL,
    [ftBand]            TINYINT             CONSTRAINT [DF_dbFundType_ftBand] DEFAULT ((3)) NOT NULL,
    [ftWarningPerc]     TINYINT             CONSTRAINT [DF_dbFundType_ftWarningPerc] DEFAULT ((100)) NOT NULL,
    [ftAgreementCode]   [dbo].[uCodeLookup] NOT NULL,
    [ftLegalAidCharged] BIT                 CONSTRAINT [DF_dbFundType_ftLegalAidCharged] DEFAULT ((0)) NOT NULL,
    [ftAccCode]         NVARCHAR (30)       NULL,
    [ftRatePerUnit]     MONEY               CONSTRAINT [DF_dbFundType_ftRatePerUnit] DEFAULT ((0)) NOT NULL,
    [ftActive]          BIT                 CONSTRAINT [DF_dbFundType_ftActive] DEFAULT ((1)) NULL,
    [rowguid]           UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFundType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFundType] PRIMARY KEY CLUSTERED ([ftCode] ASC, [ftcurISOCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbFundType_dbCurrency] FOREIGN KEY ([ftcurISOCode]) REFERENCES [dbo].[dbCurrency] ([curISOCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFundType_rowguid]
    ON [dbo].[dbFundType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFundType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFundType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFundType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFundType] TO [OMSApplicationRole]
    AS [dbo];


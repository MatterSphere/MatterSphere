CREATE TABLE [dbo].[dbFeeEarner] (
    [feeusrID]           INT                 NOT NULL,
    [feeType]            [dbo].[uCodeLookup] CONSTRAINT [DF_dbFeeEarner_feeType] DEFAULT (N'STANDARD') NOT NULL,
    [feeExtID]           INT                 NULL,
    [feeDepartment]      [dbo].[uCodeLookup] NULL,
    [feeFileType]        [dbo].[uCodeLookup] NULL,
    [feeSignOff]         NVARCHAR (50)       NULL,
    [feeAddRef]          NVARCHAR (5)        NULL,
    [feeAddSignOff]      NVARCHAR (50)       NULL,
    [feeResponsibleTo]   INT                 NULL,
    [feeResponsible]     BIT                 CONSTRAINT [DF_dbFeeEarner_feeResponsible] DEFAULT ((0)) NOT NULL,
    [feecurISOCode]      CHAR (3)            CONSTRAINT [DF_dbFeeEarner_feecurISOCode] DEFAULT ('GBP') NOT NULL,
    [feeCost]            MONEY               CONSTRAINT [DF_dbFeeEarner_feeCost] DEFAULT ((0)) NOT NULL,
    [feeRateBand1]       MONEY               CONSTRAINT [DF_dbFeeEarner_feeRateBand1] DEFAULT ((0)) NOT NULL,
    [feeRateBand2]       MONEY               CONSTRAINT [DF_dbFeeEarner_feeRateBand2] DEFAULT ((0)) NOT NULL,
    [feeRateBand3]       MONEY               CONSTRAINT [DF_dbFeeEarner_feeRateBand3] DEFAULT ((0)) NOT NULL,
    [feeRateBand4]       MONEY               CONSTRAINT [DF_dbFeeEarner_feeRateBand4] DEFAULT ((0)) NOT NULL,
    [feeRateBand5]       MONEY               CONSTRAINT [DF_dbFeeEarner_feeRateBand5] DEFAULT ((0)) NOT NULL,
    [feeTargetHoursWeek] TINYINT             CONSTRAINT [DF_dbFeeEarner_feeTargetHoursWeek] DEFAULT ((0)) NOT NULL,
    [feeTargetHoursDay]  TINYINT             CONSTRAINT [DF_dbFeeEarner_feeTargetHoursDay] DEFAULT ((0)) NOT NULL,
    [feeCVFile]          [dbo].[uFilePath]   NULL,
    [feeAssistant]       NVARCHAR (30)       NULL,
    [feeActive]          BIT                 CONSTRAINT [DF_omsFeeEarner_feeActive] DEFAULT ((0)) NOT NULL,
    [feeLAGrade]         TINYINT             NULL,
    [feeLARef]           NVARCHAR (50)       NULL,
    [feeCDSStartNum]     INT                 NULL,
    [feeSignature]       VARBINARY (MAX)     NULL,
    [Created]            [dbo].[uCreated]    CONSTRAINT [DF_dbFeeEarner_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy]          [dbo].[uCreatedBy]  NULL,
    [Updated]            [dbo].[uCreated]    CONSTRAINT [DF_dbFeeEarner_Updated] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]          [dbo].[uCreatedBy]  NULL,
    [rowguid]            UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFeeEarner_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [feeNeedsExport]     BIT                 CONSTRAINT [DF_dbFeeEarner_feeNeedsExport] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_dbFeeEarner] PRIMARY KEY CLUSTERED ([feeusrID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbFeeEarner_dbCurrency] FOREIGN KEY ([feecurISOCode]) REFERENCES [dbo].[dbCurrency] ([curISOCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFeeEarner_dbFeeEarner] FOREIGN KEY ([feeResponsibleTo]) REFERENCES [dbo].[dbFeeEarner] ([feeusrID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFeeEarner_dbFeeEarnerType] FOREIGN KEY ([feeType]) REFERENCES [dbo].[dbFeeEarnerType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFeeEarner_dbFileType] FOREIGN KEY ([feeFileType]) REFERENCES [dbo].[dbFileType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbFeeEarner_dbUser] FOREIGN KEY ([feeusrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFeeEarner_rowguid]
    ON [dbo].[dbFeeEarner]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrFlagFeeEarnerForExport] ON [dbo].[dbFeeEarner]
FOR  UPDATE NOT FOR REPLICATION
AS
IF NOT UPDATE (feeNeedsExport)
BEGIN
	UPDATE F SET F.feeNeedsExport = 1
	FROM dbo.dbFeeEarner F JOIN Inserted I ON I.feeusrID = F.feeusrID
END

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFeeEarner] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFeeEarner] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFeeEarner] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFeeEarner] TO [OMSApplicationRole]
    AS [dbo];


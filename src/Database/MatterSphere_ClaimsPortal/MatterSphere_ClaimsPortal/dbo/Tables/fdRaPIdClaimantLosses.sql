CREATE TABLE [dbo].[fdRaPIdClaimantLosses](
	[rpdCLID] [bigint] IDENTITY(1,1) NOT NULL,
	[fileID] [bigint] NULL,
	[rpdCLComments] [nvarchar](500) NULL,
	[rpdCLEvidenceAttached] [bit] NULL,
	[rpdCLGrossValueClaimed] [money] NULL,
	[rpdCLLossType] [int] NULL,
	[rpdCLPercCtribNegDeduct] [decimal](18, 2) NULL,
	[rpdCLValClaimedAfterCtrib] [money] NULL,
	[rpdCLPercInterestRate] [decimal](18, 2) NULL,
	[Created] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NULL CONSTRAINT [DF_fdRaPIdClaimantLosses_rowguid]  DEFAULT (newid()),
	[rpdCLAddedAtInterim] [bit] NULL,
	[rpdCLStage] [nvarchar](15) NULL,
	[rpdCLGrossAmountAgreed] [nvarchar](15) NULL,
	[rpdCLDataFromComp] [bit] NULL,
	[Updated] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[rpdCLUploaded] [bit] NULL,
	[rpdCLLossesSeed] [int] NULL,
	rpdCLItemBeingPursued BIT NULL,
	rpdCLInterest DECIMAL(10, 2) NULL,
	[rpdCLTariffType] INT NULL, 
    [rpdCLSelectDurationOfTheInjury] INT NULL, 
    [rpdCLExcepCircumstancesUplift] BIT NULL, 
    [rpdCLExcepCircumstancesUpliftP] INT NULL, 
    [rpdCLExcepCircumstancesUpliftNote] NVARCHAR(255) NULL, 
    CONSTRAINT [PK_fdRaPIdClaimantLosses] PRIMARY KEY CLUSTERED 
(
	[rpdCLID] ASC
) 
) 
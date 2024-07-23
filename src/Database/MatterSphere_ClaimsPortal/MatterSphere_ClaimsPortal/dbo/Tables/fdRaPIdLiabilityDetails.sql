CREATE TABLE [dbo].[fdRaPIdLiabilityDetails](
	[fileID] [bigint] NOT NULL,
	[rpdLiabDefResponsibility] [nvarchar](MAX) NULL,
	[rpdLiabFundingUndertaken] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabFundingUndertaken]  DEFAULT (0),
	[rpdLiabInsCompName] [nvarchar](100) NULL,
	[rpdLiabInsCompAddress] [nvarchar](200) NULL,
	[rpdLiabPolicyNumber] [nvarchar](50) NULL,
	[rpdLiabLevelOfCover] [nvarchar](50) NULL,
	[rpdLiabIncreasingPoint] [nvarchar](100) NULL,
	[rpdLiabOrganisationName] [nvarchar](100) NULL,
	[rpdLiabOtherDetails] [nvarchar](500) NULL,
	[rpdLiabComments] [nvarchar](MAX) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rowguid]  DEFAULT (newid()),
	[rpdLiabOP1Responsibility] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabOP1Responsibility]  DEFAULT (0),
	[rpdLiabOP2Responsibility] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabOP2Responsibility]  DEFAULT (0),
	[rpdLiabOP3Responsibility] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabOP3Responsibility]  DEFAULT (0),
	[rpdLiabOP4Responsibility] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabOP4Responsibility]  DEFAULT (0),
	[rpdLiabOP5Responsibility] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabOP5Responsibility]  DEFAULT (0),
	[rpdLiabOP6Responsibility] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabOP6Responsibility]  DEFAULT (0),
	[rpdLiabSection58] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabSection58]  DEFAULT (0),
	[rpdLiabConditionalFeeDate] [datetime] NULL,
	[rpdLiabSection29] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabSection29]  DEFAULT (0),
	[rpdLiabPolicyDate] [datetime] NULL,
	[rpdLiabPremiumStaged] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabPremiumStaged]  DEFAULT (0),
	[rpdLiabMembershipOrg] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabMembershipOrg]  DEFAULT (0),
	[rpdLiabAgreementDate] [datetime] NULL,
	[rpdLiabOther] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabOther]  DEFAULT (0),
	[rpdLiabConsFreeLglExplns] [bit] NULL CONSTRAINT [DF_fdRaPIdLiabilityDetails_rpdLiabConsFreeLglExplns]  DEFAULT (0),
 CONSTRAINT [PK_fdRaPIdLiabilityDetails] PRIMARY KEY CLUSTERED 
(
	[fileID] ASC
)
)
CREATE TABLE [dbo].[fdRaPIdDefendantResponse](
		[id] [bigint] IDENTITY(1,1) NOT NULL,
		[fileId] [bigint] NOT NULL,
		[rpdDefAgreedInS2Sp] [nvarchar](max) NULL,
		[rpdDefLossType] [nvarchar](max) NULL,
		[rpdDefComments] [nvarchar](max) NULL,
		[rpdDefGrossValueOffered] [decimal](18, 0) NULL,
		[rpdDefIsGrossAmountAgreed] [nvarchar](max) NULL,
		[rpdDefPercContribNegDeductions] [decimal](18, 0) NULL,
		[rpdDefValueOfferedAfterContrib] [decimal](18, 0) NULL,
		[rpdDefInterest] [decimal](18, 0) NULL,
		[rpdDefInterimPaymentNumber] [nvarchar](max) NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NULL CONSTRAINT [DF_fdRaPIdDefendantResponse_rowguid]  DEFAULT (newid()),
	 CONSTRAINT [PK_fdRaPIdDefendantResponse] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

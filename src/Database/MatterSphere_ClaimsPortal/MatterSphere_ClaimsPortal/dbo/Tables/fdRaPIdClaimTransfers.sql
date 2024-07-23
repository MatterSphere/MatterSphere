CREATE TABLE [dbo].[fdRaPIdClaimTransfers](
		[rpdctID] [bigint] IDENTITY(1,1) NOT NULL,
		[rpdctClaimID] [nvarchar](20) NULL,
		[rpdctProcessType] [nvarchar](10) NULL,
		[rpdctSourceCode] [nvarchar](100) NULL,
		[rpdctSourceName] [nvarchar](100) NULL,
		[rpdctDestinationCode] [nvarchar](100) NULL,
		[rpdctDestinationName] [nvarchar](100) NULL,
		[rpdctTransferDate] [datetime] NULL,
		[rpdctFullNotification] [nvarchar](500) NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdClaimTransfers_rowguid]  DEFAULT (newid()),
	 CONSTRAINT [PK_fdRaPIdClaimTransfers] PRIMARY KEY CLUSTERED 
	(
		[rpdctID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

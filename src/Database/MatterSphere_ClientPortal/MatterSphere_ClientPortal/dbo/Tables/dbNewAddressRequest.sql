CREATE TABLE [dbo].[dbNewAddressRequest](
	[addID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[addExistingId] [bigint] NOT NULL  CONSTRAINT [DF_dbNewAddressRequest_addExistingId]  DEFAULT (0),
	[addLine1] [dbo].[uAddressLine] NULL,
	[addLine2] [dbo].[uAddressLine] NULL,
	[addLine3] [dbo].[uAddressLine] NULL,
	[addLine4] [dbo].[uAddressLine] NULL,
	[addLine5] [dbo].[uAddressLine] NULL,
	[addPostcode] [dbo].[uPostcode] NULL,
	[addCountry] [dbo].[uCountry] NULL,
	[addDXCode] [nvarchar](80) NULL,
	[Created] [dbo].[uCreated] NULL CONSTRAINT [DF_dbNewAddressRequest_Created]  DEFAULT (getutcdate()),
	[CreatedBy] [dbo].[uCreatedBy] NULL,
	[Updated] [dbo].[uCreated] NULL CONSTRAINT [DF_dbNewAddressRequest_Updated]  DEFAULT (getutcdate()),
	[UpdatedBy] [dbo].[uCreatedBy] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_dbNewAddressRequest_rowguid]  DEFAULT (newid()),
	[addExtTxtID] [nvarchar](20) NULL,
	CONSTRAINT [PK_dbNewAddressRequest] PRIMARY KEY CLUSTERED 
(
	[addID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
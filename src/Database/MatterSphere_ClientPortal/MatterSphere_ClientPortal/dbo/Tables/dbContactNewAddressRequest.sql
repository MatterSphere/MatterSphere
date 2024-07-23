CREATE TABLE [dbo].[dbContactNewAddressRequest](
	[contID] [bigint] NOT NULL,
	[contaddID] [bigint] NOT NULL,
	[contCode] [dbo].[uCodeLookup] NOT NULL,
	[contMakeDefault] [bit] NOT NULL,	
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL  CONSTRAINT [DF_dbContactNewAddressRequest_rowguid]  DEFAULT (newid()),
	CONSTRAINT [PK_dbContactNewAddressRequest_Code] PRIMARY KEY CLUSTERED 
(
	[contID] ASC,
	[contaddID] ASC,
	[contCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

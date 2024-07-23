
CREATE TABLE [dbo].[dbNetDocumentsTokenAccess](
	[refreshToken] [nvarchar](max) NULL,
	[accessToken] [nvarchar](max) NULL,
	[usedBy] [bigint] NULL,
	[inUseDate] [datetime] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[dbNetDocumentsTokenAccess] ADD  CONSTRAINT [DF_dbNetDocumentsTokenAcess_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO



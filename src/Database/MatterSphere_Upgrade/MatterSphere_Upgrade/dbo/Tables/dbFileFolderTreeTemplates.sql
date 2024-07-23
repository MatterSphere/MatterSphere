CREATE TABLE [dbo].[dbFileFolderTreeTemplates](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[templatedesc] [nvarchar](15) NOT NULL,
	[treeXML] [varchar](max) NOT NULL,
	[created] [datetime] NULL,
	[createdby] [int] NULL,
	[updated] [datetime] NULL,
	[updatedby] [int] NULL,
	[rowguid] [uniqueidentifier] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[dbFileFolderTreeTemplates] ADD  CONSTRAINT [DF_dbFileFolderTreeTemplates_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO

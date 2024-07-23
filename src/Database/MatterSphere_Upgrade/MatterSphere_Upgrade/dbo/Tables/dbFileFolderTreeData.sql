﻿CREATE TABLE [dbo].[dbFileFolderTreeData](
	[id] [bigint] NOT NULL,
	[treeXML] [varchar](max) NOT NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_dbFileFolderTree_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_dbFileFolderTree_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

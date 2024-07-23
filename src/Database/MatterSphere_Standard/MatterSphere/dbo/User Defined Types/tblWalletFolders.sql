CREATE TYPE [dbo].[tblWalletFolders] AS TABLE(
	[id] [int] NOT NULL,
	[FolderCode] [nvarchar](15) NULL,
	[FolderGUID] [uniqueidentifier] NULL,
	PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TYPE [dbo].[tblFolderData] AS TABLE(
	[FolderCode] [nvarchar](15) NULL,
	[FolderGUID] [uniqueidentifier] NULL,
	[FolderDescription] [nvarchar](100) NULL
)
GO

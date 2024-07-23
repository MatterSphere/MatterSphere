CREATE TABLE [dbo].[fdRaPIdDocumentDownloads](
	[rpdDocDID] [bigint] IDENTITY(1,1) NOT NULL,
	[fileID] [bigint] NULL,
	[DocID] [bigint] NULL,
	[rpdDocDFilename] [nvarchar](200) NULL,
	[rpdDocDTitle] [nvarchar](200) NULL,
	[rpdDocDDescription] [nvarchar](200) NULL,
	[rpdDocDDownloaded] [datetime] NULL,
	[rpdDocDDownloadedBy] [int] NULL,
	[rpdDocDAttachmentGUID] [nvarchar](50) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdDocumentDownloads_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_fdRaPIdDocumentDownloads] PRIMARY KEY CLUSTERED 
(
	[rpdDocDID] ASC
)
)
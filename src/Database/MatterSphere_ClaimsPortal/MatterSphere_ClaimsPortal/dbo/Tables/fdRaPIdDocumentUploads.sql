CREATE TABLE [dbo].[fdRaPIdDocumentUploads](
	[rpdDocID] [bigint] IDENTITY(1,1) NOT NULL,
	[fileID] [bigint] NULL,
	[DocID] [bigint] NULL,
	[rpdDocFilename] [nvarchar](200) NULL,
	[rpdDocTitle] [nvarchar](200) NULL,
	[rpdDocDescription] [nvarchar](200) NULL,
	[rpdDocFilePath] [nvarchar](250) NULL,
	[rpdDocUploaded] [datetime] NULL,
	[rpdDocUploadedBy] [int] NULL,
	[rpdDocReturnGUID] [nvarchar](50) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdDocumentUploads_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_fdRaPIdDocumentUploads] PRIMARY KEY CLUSTERED 
(
	[rpdDocID] ASC
)
) 
CREATE TABLE [dbo].[fdRaPIdClaim](
	[rpdClmAppID] [nvarchar](50) NOT NULL,
	[rpdClmActivityGuid] [nvarchar](50) NULL,
	[rpdClmAppRef] [nvarchar](200) NULL,
	[rpdClmAttachmentsCount] [int] NULL,
	[rpdClmCreationTime] [datetime] NULL,
	[rpdClmCurrentUserID] [nvarchar](50) NULL,
	[rpdClmLockStatus] [int] NULL,
	[rpdClmLockUserID] [nvarchar](50) NULL,
	[rpdClmPhaseCacheID] [nvarchar](50) NULL,
	[rpdClmPhaseCacheName] [nvarchar](250) NULL,
	[rpdClmPrintDocCount] [int] NULL,
	[rpdClmVerMajor] [int] NULL,
	[rpdClmVerMinor] [int] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NULL CONSTRAINT [DF_fdRaPIdClaims_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_fdRaPIdClaims] PRIMARY KEY CLUSTERED 
(
	[rpdClmAppID] ASC
) 
) 

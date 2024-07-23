CREATE TABLE [dbo].[fdRaPIdNotificationList](
	[rpdNotfID] [bigint] IDENTITY(1,1) NOT NULL,
	[rpdNotfAppID] [nvarchar](50) NULL,
	[rpdNotfFormattedDate] [datetime] NULL,
	[rpdNotfDateTime] [datetime] NULL,
	[rpdNotfGuid] [nvarchar](50) NULL,
	[rpdNotfMessage] [nvarchar](500) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdNotificationList_rowguid]  DEFAULT (newid()),
	[rpdNotfClaimXml] [xml] NULL,
	rpdNotfAcknowledged DATETIME NULL,
	rpdNotfAcknowledgedBy INT NULL,
 CONSTRAINT [PK_fdRaPIdNotificationList] PRIMARY KEY CLUSTERED 
(
	[rpdNotfID] ASC
)
)
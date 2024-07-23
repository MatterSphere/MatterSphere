CREATE TYPE [dbo].[tblClaimsPortalClaimsList] AS TABLE(
	[applicationId]	[nvarchar](50) NOT NULL,
	[activityEngineGuid] [nvarchar](50) NULL,
	[applicationReferences]	[nvarchar](200) NULL,
	[attachmentsCount]	[int] NULL,
	[creationTime]	[datetime] NULL,
	[currentUserID]	[nvarchar](50) NULL,
	[lockStatus]	[int] NULL,
	[lockUserId]	[nvarchar](50) NULL,
	[phaseCacheId]	[nvarchar](50) NULL,
	[phaseCacheName]	[nvarchar](250) NULL,
	[printableDocumentsCount]	[int] NULL,
	[versionMajor]	[int] NULL,
	[versionMinor]	[int] NULL
)
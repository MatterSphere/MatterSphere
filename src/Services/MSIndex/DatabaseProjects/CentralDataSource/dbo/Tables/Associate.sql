CREATE TABLE [dbo].[Associate]
(
	[mattersphereid] BIGINT
	, [file-id] BIGINT
	, [contact-id] BIGINT
	, [associateType] NVARCHAR(1000)
	, [assocHeading] NVARCHAR(255)
	, [assocSalut]  NVARCHAR(100)
	, [assocAddressee] NVARCHAR(100)
	, [assocNotes] NVARCHAR(200)
	, [modifieddate] DATETIME
	, [assocSearch] AS CONCAT([assocHeading], ' ', [assocSalut], ' ', [assocAddressee], ' ', [assocNotes])
	, CONSTRAINT PK_Associate PRIMARY KEY ([mattersphereid])
)
GO
CREATE INDEX IX_Associate_fileid ON [dbo].[Associate] ([file-id])
GO
CREATE INDEX IX_Associate_contactid ON [dbo].[Associate] ([contact-id])
GO

CREATE FULLTEXT INDEX ON [dbo].[Associate] ([assocSearch])
KEY INDEX PK_Associate
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO
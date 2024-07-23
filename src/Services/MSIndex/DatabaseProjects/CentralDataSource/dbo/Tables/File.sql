CREATE TABLE [dbo].[File]
(
	[mattersphereid] BIGINT
	, [client-id] BIGINT
	, [fileType] NVARCHAR(1000)
	, [fileStatus] NVARCHAR(1000)
	, [fileDesc] NVARCHAR(255)
	, [fileNotes] NVARCHAR(MAX)
	, [modifieddate] DATETIME
	, [fileSearch] AS CONCAT([fileDesc], ' ', [fileNotes])
	, CONSTRAINT PK_File PRIMARY KEY ([mattersphereid])
)
GO
CREATE INDEX IX_File_clientid ON [dbo].[File] ([client-id])
GO

CREATE FULLTEXT INDEX ON [dbo].[File] ([fileSearch])
KEY INDEX PK_File
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO
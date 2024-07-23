CREATE TABLE [dbo].[Document]
(
	[mattersphereid] BIGINT
	, [associate-id] BIGINT
	, [client-id] BIGINT
	, [file-id] BIGINT
	, [docDeleted] BIT
	, [documentExtension] NVARCHAR(15)
	, [documentType] NVARCHAR(1000)
	, [docDesc] NVARCHAR(150)
	, [usrFullName] NVARCHAR(50)
	, [modifieddate] DATETIME
	, [docSearch] AS CONCAT([docDesc], ' ', [usrFullName])
	, CONSTRAINT PK_Document PRIMARY KEY ([mattersphereid])
)
GO
CREATE INDEX IX_Document_associateid ON [dbo].[Document] ([associate-id])
GO
CREATE INDEX IX_Document_clientid ON [dbo].[Document] ([client-id])
GO
CREATE INDEX IX_Document_fileid ON [dbo].[Document] ([file-id])
GO

CREATE FULLTEXT INDEX ON [dbo].[Document] ([docSearch])
KEY INDEX PK_Document
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO
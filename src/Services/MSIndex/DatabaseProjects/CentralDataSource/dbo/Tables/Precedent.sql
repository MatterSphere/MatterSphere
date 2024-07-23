CREATE TABLE [dbo].[Precedent]
(
	[mattersphereid] BIGINT
	, [precedentType] NVARCHAR(1000)
	, [precLibrary] NVARCHAR(1000)
	, [precCategory] NVARCHAR(1000)
	, [precSubCategory] NVARCHAR(1000)
	, [precDeleted] BIT
	, [precedentExtension] NVARCHAR(15)
	, [precDesc] NVARCHAR(100)
	, [precTitle] NVARCHAR(50)
	, [modifieddate] DATETIME
	, [precSearch] AS CONCAT([precTitle], ' ', [precDesc])
	, CONSTRAINT PK_Precedent PRIMARY KEY ([mattersphereid])
)
GO

CREATE FULLTEXT INDEX ON [dbo].[Precedent] ([precSearch])
KEY INDEX PK_Precedent
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO

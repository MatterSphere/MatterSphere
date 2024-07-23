CREATE TABLE [dbo].[Address]
(
	[mattersphereid] BIGINT 
	, [sc] NVARCHAR(MAX)
	, [modifieddate] DATETIME
	, CONSTRAINT PK_Address PRIMARY KEY ([mattersphereid])
)
GO

CREATE FULLTEXT INDEX ON [dbo].[Address] ([sc])
KEY INDEX PK_Address
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO
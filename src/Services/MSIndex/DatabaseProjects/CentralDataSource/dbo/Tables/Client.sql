CREATE TABLE [dbo].[Client]
(
	[mattersphereid] BIGINT
	, [address-id] BIGINT
	, [clientType] NVARCHAR(1000)
	, [clName] NVARCHAR(128)
	, [clNo] NVARCHAR(12)
	, [clNotes] NVARCHAR(MAX)
	, [modifieddate] DATETIME
	, [clSearch] AS CONCAT([clNo], ' ', [clName], ' ', [clNotes])
	, CONSTRAINT PK_Client PRIMARY KEY ([mattersphereid])
)
GO
CREATE INDEX IX_Client_addressid ON [dbo].[Client] ([address-id])
GO

CREATE FULLTEXT INDEX ON [dbo].[Client] ([clSearch])
KEY INDEX PK_Client
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO
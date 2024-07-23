CREATE TABLE [dbo].[Contact]
(
	[mattersphereid] BIGINT
	, [address-id] BIGINT
	, [contactType] NVARCHAR(1000)
	, [contName] NVARCHAR(128)
	, [contNotes] NVARCHAR(4000)
	, [modifieddate] DATETIME
	, [contSearch] AS CONCAT([contName], ' ', [contNotes])
	, CONSTRAINT PK_Contact PRIMARY KEY ([mattersphereid])
)
GO
CREATE INDEX IX_Contact_addressid ON [dbo].[Contact] ([address-id])
GO

CREATE FULLTEXT INDEX ON [dbo].[Contact] ([contSearch])
KEY INDEX PK_Contact
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO
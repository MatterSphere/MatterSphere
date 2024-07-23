CREATE TYPE [dbo].[PrecedentMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [precedentType] NVARCHAR(1000)
	, [precCategory] NVARCHAR(1000)	
	, [precDeleted] BIT
	, [precedentExtension] NVARCHAR(15)
	, [precDesc] NVARCHAR(100)
	, [precTitle] NVARCHAR(50)
	, [precLibrary] NVARCHAR(1000)
	, [precSubCategory] NVARCHAR(1000)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)

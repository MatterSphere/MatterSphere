CREATE TABLE [dbo].[Users]
(
	[mattersphereid] INT 
	, [usrinits] NVARCHAR(30)
	, [usralias] NVARCHAR(36)
	, [usrad] NVARCHAR(50)
	, [usrsql] NVARCHAR(50)
	, [usrfullname] NVARCHAR(50)
	, [usractive]  NVARCHAR(10)
	, [modifieddate] DATETIME
	, [usrAccessList]  NVARCHAR(MAX)
	, CONSTRAINT PK_Users PRIMARY KEY ([mattersphereid])
)

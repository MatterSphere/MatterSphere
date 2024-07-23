CREATE TABLE dbo.UserGroup_File
(
	UserGroupID UNIQUEIDENTIFIER
	, mattersphereid BIGINT
	, gDeny BIT
	, CONSTRAINT PK_UserGroup_File PRIMARY KEY (mattersphereid, UserGroupID)
	, CONSTRAINT UC_UserGroup_File UNIQUE (UserGroupID, mattersphereid)
)

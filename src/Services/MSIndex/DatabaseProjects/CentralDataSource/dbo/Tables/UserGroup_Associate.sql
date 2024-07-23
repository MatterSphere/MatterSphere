CREATE TABLE dbo.UserGroup_Associate
(
	UserGroupID UNIQUEIDENTIFIER
	, mattersphereid BIGINT
	, gDeny BIT
	, CONSTRAINT PK_UserGroup_Associate PRIMARY KEY (mattersphereid, UserGroupID)
	, CONSTRAINT UC_UserGroup_Associate UNIQUE (UserGroupID, mattersphereid)
)

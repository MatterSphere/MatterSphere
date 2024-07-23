CREATE TABLE dbo.UserGroup_Document
(
	UserGroupID UNIQUEIDENTIFIER
	, mattersphereid BIGINT
	, gDeny BIT
	, CONSTRAINT PK_UserGroup_Document PRIMARY KEY (mattersphereid, UserGroupID)
	, CONSTRAINT UC_UserGroup_Document UNIQUE (UserGroupID, mattersphereid)
)

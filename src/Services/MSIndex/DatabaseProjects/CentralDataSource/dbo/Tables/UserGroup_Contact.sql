CREATE TABLE dbo.UserGroup_Contact
(
	UserGroupID UNIQUEIDENTIFIER
	, mattersphereid BIGINT
	, gDeny BIT
	, CONSTRAINT PK_UserGroup_Contact PRIMARY KEY (mattersphereid, UserGroupID)
	, CONSTRAINT UC_UserGroup_Contact UNIQUE (UserGroupID, mattersphereid)
)

CREATE TABLE dbo.UserGroup_Client
(
	UserGroupID UNIQUEIDENTIFIER
	, mattersphereid BIGINT
	, gDeny BIT
	, CONSTRAINT PK_UserGroup_Client PRIMARY KEY (mattersphereid, UserGroupID)
	, CONSTRAINT UC_UserGroup_Client UNIQUE (UserGroupID, mattersphereid)
)

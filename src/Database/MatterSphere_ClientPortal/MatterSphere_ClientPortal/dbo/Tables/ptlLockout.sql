CREATE TABLE [dbo].[ptlLockout](
	[UserID] [int] NOT NULL,
	[Counter] [int] NOT NULL DEFAULT 0,	
	[Date] [datetime] NULL	
, CONSTRAINT PK_ptlLockout_UserID PRIMARY KEY (UserID)
--, CONSTRAINT FK_ptlLockout_dbUser FOREIGN KEY(UserID) REFERENCES dbo.[dbUser] (usrID) NOT FOR REPLICATION 
)
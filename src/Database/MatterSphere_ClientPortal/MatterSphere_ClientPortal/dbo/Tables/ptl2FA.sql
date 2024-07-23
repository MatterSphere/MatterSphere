CREATE TABLE [dbo].[ptl2FA](
	[UserID] [int] NOT NULL,
	[SecretKey] [nvarchar](36) NULL,	
	[IsAttached] [bit] NOT NULL	
, CONSTRAINT PK_ptl2FA_UserID PRIMARY KEY (UserID)
--, CONSTRAINT FK_ptl2FA_dbUser FOREIGN KEY(UserID) REFERENCES dbo.[dbUser] (usrID) NOT FOR REPLICATION 
)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'trgdbUserPasswordUpdated' AND parent_id = OBJECT_ID('dbo.dbUser'))
DROP TRIGGER dbo.trgdbUserPasswordUpdated 
GO

CREATE TRIGGER dbo.trgdbUserPasswordUpdated ON dbo.dbUser 
FOR UPDATE NOT FOR REPLICATION
AS
IF @@ROWCOUNT = 0
	RETURN;


WITH PCh AS
(
SELECT usrID, usrPassword
FROM deleted 
EXCEPT
SELECT usrID, usrPassword
FROM inserted
)
UPDATE d
SET [Counter] = 0
	, [Date] = NULL 
FROM dbo.ptlLockout AS d
	INNER JOIN Pch ON PCh.usrID = d.UserID
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ptlLockout_dbUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[ptlLockout]'))
	ALTER TABLE [dbo].[ptlLockout] ADD CONSTRAINT FK_ptlLockout_dbUser FOREIGN KEY(UserID) REFERENCES dbo.[dbUser] (usrID) NOT FOR REPLICATION 
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ptl2FA_dbUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[ptl2FA]'))
	ALTER TABLE [dbo].[ptl2FA] ADD CONSTRAINT FK_ptl2FA_dbUser FOREIGN KEY(UserID) REFERENCES dbo.[dbUser] (usrID) NOT FOR REPLICATION 
GO
insert into ptl2FA (UserID, SecretKey, IsAttached)
	select dbUser.usrID, NEWID(), 0
	from dbUser
	where dbUser.usrType = 'CLIENT'
	and not exists (select 1 from ptl2FA p where p.UserID = dbUser.usrID)
GO
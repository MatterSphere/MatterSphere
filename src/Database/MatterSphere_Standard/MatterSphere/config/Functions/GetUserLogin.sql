

CREATE FUNCTION [config].[GetUserLogin] ( )
RETURNS nvarchar(128)

AS
BEGIN
	DECLARE @user nvarchar(128)

	SELECT @user = (SELECT Convert(nvarchar(128),context_info) FROM master.dbo.sysprocesses WHERE spid = @@SPID)
	SELECT @user = nullif(@user,'')
	SELECT @user = Coalesce(@user,Suser_sname()	) 
	SELECT @user =(Select usrADID from dbuser where usrADID = @user and usrActive = 1)
	RETURN @user
END


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetUserLogin] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetUserLogin] TO [OMSAdminRole]
    AS [dbo];


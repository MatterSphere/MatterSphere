

CREATE FUNCTION [dbo].[GetUser] 
(
	@USRID bigint,
	@FIELD nvarchar(40) = null
)  
RETURNS nvarchar(40)
AS  

BEGIN 
	DECLARE @TMP nvarchar(100)

	if upper(@FIELD) ='USRFULLNAME'
		select @TMP = usrFullName from dbuser where usrid = @USRID
	else
		select @TMP = usrInits from dbuser where usrid = @USRID
	
	return @TMP

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUser] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUser] TO [OMSAdminRole]
    AS [dbo];


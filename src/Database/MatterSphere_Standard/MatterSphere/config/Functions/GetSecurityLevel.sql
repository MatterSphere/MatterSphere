

CREATE FUNCTION [config].[GetSecurityLevel] ()
RETURNS int

AS
BEGIN
	DECLARE @securityLevel int
	SET @securityLevel = ( SELECT spData FROM dbo.dbSpecificData WHERE spLookup = 'SECLEVEL' AND brID = 1)
	IF @securityLevel IS NULL
		SET @securityLevel  = 1920
	RETURN @securityLevel
END


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetSecurityLevel] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetSecurityLevel] TO [OMSAdminRole]
    AS [dbo];


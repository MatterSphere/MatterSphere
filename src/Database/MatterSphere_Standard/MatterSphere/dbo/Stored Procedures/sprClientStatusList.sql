

CREATE PROCEDURE [dbo].[sprClientStatusList]
(@STATUS NVARCHAR(15) = NULL)
AS
	BEGIN
		SELECT 
			csCode
			,csTimeEntry
			,csFileCreation
			,csAccCode
		FROM 
			dbClientStatus
		WHERE
			csCode = ISNULL(@STATUS, csCode)
END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientStatusList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientStatusList] TO [OMSAdminRole]
    AS [dbo];


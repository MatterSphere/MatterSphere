

CREATE PROCEDURE [dbo].[sprClientStatusRecord]
(@CLID BIGINT = NULL)
AS
	BEGIN

	SELECT 
		C.clID
		,C.clStatus
		,CS.csTimeEntry
		,CS.csFileCreation
		,CS.csAccCode
	FROM 
		DBClient C
		LEFT JOIN dbClientStatus CS ON CS.csCode = C.clStatus
	WHERE
		C.Clid = ISNULL(@CLID, C.Clid)
END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientStatusRecord] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientStatusRecord] TO [OMSAdminRole]
    AS [dbo];


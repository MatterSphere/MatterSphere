

CREATE PROCEDURE [dbo].[sprFileStatusList]
(@STATUS NVARCHAR(15) = NULL)
AS
	BEGIN
		SELECT 
			fsCode
			, fsAccCode
			, fsTimeEntry
			, fsDocModification
			, fsAppCreation
			, fsAssocCreation
			, fsTaskFlowEdit
			, fsAlertLevel
		FROM 
			dbFileStatus
		WHERE
			fsCode = ISNULL(@STATUS, fsCode)
END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFileStatusList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFileStatusList] TO [OMSAdminRole]
    AS [dbo];


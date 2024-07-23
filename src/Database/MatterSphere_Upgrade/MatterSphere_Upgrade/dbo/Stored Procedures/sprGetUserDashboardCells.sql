CREATE PROCEDURE dbo.sprGetUserDashboardCells (@usrId INT, @dshObjCode [dbo].[uCodeLookup], @isConfigurationMode BIT)
AS
SET NOCOUNT ON;
IF (@isConfigurationMode = 0 AND EXISTS(SELECT 1 FROM dbo.dbUserDashboards WITH(NOLOCK) WHERE usrID = @usrID AND dshObjCode = @dshObjCode))
    SELECT dshConfig FROM dbo.dbUserDashboards WHERE usrID = @usrID AND dshObjCode = @dshObjCode
ELSE 
    SELECT dshConfig FROM dbo.dbDashboards WHERE dshObjCode = @dshObjCode
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetUserDashboardCells] TO [OMSRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetUserDashboardCells] TO [OMSAdminRole]
    AS [dbo];

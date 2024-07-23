CREATE PROCEDURE dbo.sprUpdateUserDashboards
    @usrID INT
    ,@dshObjCode uCodeLookup
    ,@dshConfig dbo.uXML
    ,@isConfigurationMode BIT
AS SET NOCOUNT ON
    IF @isConfigurationMode = 1
        UPDATE dbo.dbDashboards SET dshConfig = @dshConfig WHERE dshObjCode = @dshObjCode
    ELSE
        IF EXISTS(SELECT 1 FROM dbo.dbUserDashboards WITH(NOLOCK) WHERE usrID = @usrID AND dshObjCode = @dshObjCode)
            IF ISNULL(@dshConfig, '') = ''
                DELETE dbo.dbUserDashboards WHERE usrID = @usrID AND dshObjCode = @dshObjCode
            ELSE 
                UPDATE dbo.dbUserDashboards SET dshConfig = @dshConfig WHERE usrID = @usrID AND dshObjCode = @dshObjCode
        ELSE
            IF ISNULL(@dshConfig, '') <> ''
                INSERT INTO dbo.dbUserDashboards([usrID],[dshObjCode],[dshConfig])
                VALUES(@usrID, @dshObjCode, @dshConfig)

GO
GRANT EXECUTE
    ON OBJECT::dbo.sprUpdateUserDashboards TO [OMSRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::dbo.sprUpdateUserDashboards TO [OMSAdminRole]
    AS [dbo];


CREATE PROCEDURE dbo.sprAddDeleteUserDashboardCells (
	@usrID BIGINT
	, @dshbType NVARCHAR(50)
	, @objCode dbo.uCodeLookup
	, @posRow SMALLINT
	, @posColumn SMALLINT
	, @dshbID INT OUTPUT
)
AS
SET NOCOUNT ON;
IF @dshbID IS NOT NULL
	DELETE dbo.dbDashboardCells WHERE dshbID = @dshbID
ELSE
BEGIN
	INSERT INTO dbo.dbDashboardCells(usrID, dshbType, objCode, posRow, posColumn)
	VALUES (@usrID, @dshbType, @objCode, @posRow, @posColumn)

	SET @dshbID =  SCOPE_IDENTITY()
END

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddDeleteUserDashboardCells] TO [OMSRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddDeleteUserDashboardCells] TO [OMSAdminRole]
    AS [dbo];
CREATE PROCEDURE dbo.sprCheckForDuplicateTeam
(
	@tmID INT = NULL,
	@tmCode NVARCHAR(15)
)
AS
SET NOCOUNT ON;
SELECT tmId 
	, tmCode
FROM dbo.dbTeam
WHERE tmCode = @tmCode
	AND (tmID <> @tmID OR @tmID IS NULL)
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[sprCheckForDuplicateTeam] TO [OMSRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCheckForDuplicateTeam] TO [OMSAdminRole]
    AS [dbo];

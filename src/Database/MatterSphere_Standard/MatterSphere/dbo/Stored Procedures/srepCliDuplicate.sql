

CREATE PROCEDURE [dbo].[srepCliDuplicate]

AS 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	COUNT(*) AS Duplicates,
	CL.clName
FROM
	dbo.dbClient CL
GROUP BY
	CL.clName
HAVING
	COUNT(*) > 1

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliDuplicate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliDuplicate] TO [OMSAdminRole]
    AS [dbo];


CREATE PROCEDURE dbo.fdSCHCONLINKADM
(
	@ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH LINK AS (
	SELECT contID
		 , SUM(CASE WHEN contid = contlinkid THEN 0 ELSE 1 END) AS Links
		 , MAX(CASE WHEN contid = contlinkid THEN Created END) AS Created
		 , MAX(CASE WHEN contid = contlinkid THEN CreatedBy END) AS CreatedBy
	FROM dbo.dbContactLinks 
	GROUP BY contID
	HAVING MAX(CASE WHEN contid <> contlinkid THEN 0 ELSE 1 END) = 1
	)
, Res AS(
	SELECT 
		CONT.contID
		, CONT.contName AS Name
		, LINK.Created
		, U.usrFullName AS CreatedBy
		, LINK.Links
		, 70 AS Image
	FROM LINK
		INNER JOIN dbContact CONT ON CONT.contID = LINK.contID
		INNER JOIN dbo.dbUser U ON U.usrID = LINK.CreatedBy
)
SELECT
	contID
	, Name
	, Created
	, CreatedBy
	, Links
	, [Image]
FROM Res
'
IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY contID'
ELSE 
	IF @ORDERBY NOT LIKE '%contID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', contID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY
--PRINT @Select
EXEC sp_executesql @Select

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdSCHCONLINKADM] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdSCHCONLINKADM] TO [OMSAdminRole]
    AS [dbo];

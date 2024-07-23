CREATE PROCEDURE dbo.SCHCONLINKS
(
	@UI uUICultureInfo = '{default}'
	, @CONTID BIGINT
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Res AS (
SELECT
	CL.*
	, C.contName
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF( CL.contLinkCode, '''') + ''~'') AS linkDesc
	, U.usrFullName
FROM dbo.dbContactLinks CL
	INNER JOIN dbContact C ON C.contID = CL.contLinkID
	INNER JOIN dbo.dbUser U ON U.usrID = CL.CreatedBy
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''CONTLINK'', @UI) CL1 ON CL1.cdCode = CL.contLinkCode
WHERE CL.contID = @CONTID
)
SELECT *
FROM Res
'
IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY contLinkID'
ELSE 
	IF @ORDERBY NOT LIKE '%contLinkID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', contLinkID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY
--PRINT @Select
EXEC sp_executesql @Select,  N'@UI uUICultureInfo, @CONTID BIGINT', @UI, @CONTID 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCONLINKS] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCONLINKS] TO [OMSAdminRole]
    AS [dbo];

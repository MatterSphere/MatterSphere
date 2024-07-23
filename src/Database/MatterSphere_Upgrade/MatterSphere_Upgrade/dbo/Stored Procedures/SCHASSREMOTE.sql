CREATE PROCEDURE dbo.SCHASSREMOTE
(
	@FILEID BIGINT
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Res AS (
SELECT     
	dbo.dbFile.fileID
	, COALESCE(dbo.dbAssociates.assocAddressee, dbo.dbContact.contName) AS contName
	, dbo.dbAssociates.assocID
	, dbo.dbAssociates.assocEmail
	, dbo.dbAssociates.assocMobile
	, dbo.dbInteractiveProfile.proEmail
	, dbo.dbInteractiveFileProfile.ID
FROM dbo.dbInteractiveProfile 
	INNER JOIN dbo.dbInteractiveFileProfile ON dbo.dbInteractiveProfile.contID = dbo.dbInteractiveFileProfile.contID 
	RIGHT OUTER JOIN dbo.dbFile 
	INNER JOIN dbo.dbAssociates ON dbo.dbFile.fileID = dbo.dbAssociates.fileID 
	INNER JOIN dbo.dbContact ON dbo.dbAssociates.contID = dbo.dbContact.contID ON dbo.dbInteractiveFileProfile.fileid = dbo.dbFile.fileID 
	AND dbo.dbInteractiveProfile.contID = dbo.dbContact.contID
WHERE dbo.dbFile.fileID = @FILEID 
	 AND dbo.dbInteractiveFileProfile.ID IS NULL
)
SELECT 
	fileID
	, contName
	, assocID
	, assocEmail
	, assocMobile
	, proEmail
	, ID
FROM Res
'
IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY assocID'
ELSE 
	IF @ORDERBY NOT LIKE '%assocID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', assocID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY
--PRINT @Select
EXEC sp_executesql @Select,  N'@FILEID BIGINT', @FILEID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHASSREMOTE] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHASSREMOTE] TO [OMSAdminRole]
    AS [dbo];
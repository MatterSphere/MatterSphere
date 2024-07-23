CREATE PROCEDURE dbo.SCHCLICOMPLAINT
(
	@UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 50
	, @CLID BIGINT
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Comp AS (
SELECT
	C.*
	, U.usrFullName as CreatedByUser
	, dbo.GetFileRef(CL.clNo, FL.fileNo) as [fileRef]
	, FL.fileNo
	, UA.usrFullName as AuthorisedBy
	, FL.fileID as linkedFileID
FROM dbo.dbComplaints C
	INNER JOIN dbo.dbClient CL ON CL.clID = C.clID
	LEFT OUTER JOIN dbo.dbFile FL ON FL.fileID = C.fileID
	LEFT OUTER JOIN dbo.dbUser U ON U.usrID = C.CreatedBy
	INNER JOIN dbo.dbUser UA ON UA.usrID = C.compFeeID
WHERE C.clID = @CLID
)
'
IF @MAX_RECORDS > 0
	SET @Select =  @Select + N'
SELECT TOP (@MAX_RECORDS) *
FROM Comp
'
ELSE
	SET @Select =  @Select + N'
SELECT *
FROM Comp
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY compID DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%compID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', compID DESC'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @Select,  N'@MAX_RECORDS INT,@CLID BIGINT', @MAX_RECORDS, @CLID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCLICOMPLAINT] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCLICOMPLAINT] TO [OMSAdminRole]
    AS [dbo];
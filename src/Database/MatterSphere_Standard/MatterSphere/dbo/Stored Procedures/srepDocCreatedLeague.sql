

CREATE PROCEDURE [dbo].[srepDocCreatedLeague]
(
	@UI uUICultureInfo = '{default}'
	, @DEPARTMENT nvarchar(15) = NULL
	, @USER int = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(1850)
DECLARE @GROUPBY nvarchar(150)


--- BUILD THE SELECT CLAUSE
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	DT.cdDesc AS DocTypeDesc
	, DP.cdDesc AS fileDepartment
	, U.usrFullName
	, COUNT_BIG(D.docID) AS Total
FROM         
	dbo.dbDocument D
INNER JOIN
	dbo.dbFile F ON D.fileID = F.fileID
INNER JOIN
	dbUser U ON U.usrID = D.CreatedBy
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''DOCTYPE'', @UI) AS DT ON DT.cdCode = d.docType
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''DEPT'', @UI) AS DP ON DP.cdCode = f.fileDepartment '

---- SET THE WHERE CLAUSE
SET @WHERE = ''

--- DEPARTMENT CLAUSE
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' F.fileDepartment = @DEPARTMENT '
END

--- DOCUMENT CREATED BY CLAUSE
IF(@USER IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND D.CreatedBy = @USER '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' D.CreatedBy = @USER '
	END
END

--- DOCUMENT CREATED START DATE AND END DATE CLAUSE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (D.Created >= @STARTDATE AND D.Created < @ENDDATE) '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (D.Created >= @STARTDATE AND D.Created < @ENDDATE) '
	END
END  

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- SET THE GROUP BY CLAUSE
SET @GROUPBY = '
GROUP BY
	DT.cdDesc
	, DP.cdDesc
	, U.usrFullName
ORDER BY 4 Desc '

DECLARE @SQL nvarchar(4000)
--- ADD CLAUSES TOGETHER
SET @SQL = + Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@GROUPBY)

--- DEBUG PRINT
--- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @DEPARTMENT nvarchar(15)
	, @USER int
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @DEPARTMENT
	, @USER
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocCreatedLeague] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocCreatedLeague] TO [OMSAdminRole]
    AS [dbo];


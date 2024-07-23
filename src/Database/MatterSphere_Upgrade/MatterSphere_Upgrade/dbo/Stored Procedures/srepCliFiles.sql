

CREATE PROCEDURE [dbo].[srepCliFiles]
(
	@UI uUICultureInfo = '{default}'
	, @CLNAME nvarchar(128) = NULL
	, @CLNO nvarchar(20) = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nvarchar(2250)
DECLARE @WHERE nvarchar(1250)
DECLARE @ORDERBY nvarchar(500)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	CL.clNo
	, F.fileNo
	, REPLACE(F.fileDesc, char(13) + char(10), '', '') as fileDesc
	, F.Created
	, F.fileClosed
	, F.fileType
    , U.usrInits AS FeeInits
	, A.cdDesc as FileTypeDesc
	, CL.clName
    , U.usrFullName
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''FILETYPE'' , @UI ) A ON A.cdCode = F.FileType '

--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- CLIENT NUMBER CLAUSE
IF(@CLNO IS NOT NULL)
BEGIN
		SET @WHERE = @WHERE + ' CL.clNo = @CLNO '
END

--- CLIENT NAME CLAUSE
IF(@CLNAME IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND CL.clName LIKE @CLNAME + ''%'' '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' CL.clName LIKE @CLNAME + ''%'' '
	END
END

--- FILE CREATED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
	END
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- SET THE ORDER BY CLAUSE
SET @ORDERBY = '
ORDER BY
	F.Created '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @CLNAME nvarchar(128)
	, @CLNO nvarchar(20)
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @CLNAME
	, @CLNO
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliFiles] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliFiles] TO [OMSAdminRole]
    AS [dbo];


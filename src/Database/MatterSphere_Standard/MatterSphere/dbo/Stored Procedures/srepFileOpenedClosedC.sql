

CREATE PROCEDURE [dbo].[srepFileOpenedClosedC]
(
	@UI uUICultureInfo = '{default}'
	, @BRANCH int = NULL
	, @LACAT nvarchar(15) = NULL
	, @FEEEARNER int = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
	, @OPENED bit = 1
)

AS 

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(2000)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	CL.clNo + ''/'' + F.fileNo AS Ref
	, U.usrInits
	, F.fileType
	, F.fileprincipleid
	, X.[cdDesc] as FileTypeDesc
	, CL.clName
	, replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc
	, F.Created
	, F.fileClosed
	, LACAT.LegAidDesc
	, F.brID
	, LACAT.LegAidCategory
	, U.usrfullname
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID 
LEFT OUTER JOIN
	dbo.dbLegalAidCategory LACAT ON F.fileLACategory = LACAT.LegAidCategory
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''FILETYPE'' , @UI ) X ON X.[cdCode] = F.fileType '

---- SET THE WHERE STATEMENT
SET @WHERE = ''

--- BRANCH CLAUSE
IF(@BRANCH IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' F.brID = @BRANCH '
END

--- LEGAL AID CATEGORY CLAUSE
IF(@LACAT IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileLACategory = @LACAT '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileLACategory = @LACAT '
	END
END

--- FEE EARNER CLAUSE
IF(@FEEEARNER IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.filePrincipleID = @FEEEARNER '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.filePrincipleID = @FEEEARNER '
	END
END

--- FILE CREATED START DATE AND END DATE
IF(@OPENED = 1)
BEGIN
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
END
ELSE
BEGIN
	IF(@STARTDATE IS NOT NULL)
	BEGIN
		IF(@WHERE <> '')
		BEGIN
			SET @WHERE = @WHERE + ' AND (F.fileClosed >= @STARTDATE AND F.fileClosed < @ENDDATE) '
		END
		ELSE
		BEGIN
			SET @WHERE = @WHERE + ' (F.fileClosed >= @STARTDATE AND f.fileClosed < @ENDDATE) '
		END
	END
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

--- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @BRANCH int 
	, @LACAT nvarchar(15)
	, @FEEEARNER int
	, @STARTDATE datetime
	, @ENDDATE datetime '
	, @UI
	, @BRANCH
	, @LACAT
	, @FEEEARNER
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileOpenedClosedC] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileOpenedClosedC] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[srepFilInactivityTime]
(
	@UI uUICultureInfo='{default}',
	@PERIODINACT int = 30,
	@FILETYPE nvarchar(20) = null,
	@DEPARTMENT nvarchar(20) = null,
	@FUNDTYPE nvarchar(20) = null,
	@LACAT nvarchar(20) = null,
	@FEEEARNER int = null,
	@FILESTATUS nvarchar(20) = null
)

AS 

DECLARE @SQL nvarchar(4000)
DECLARE @Select nvarchar(2000)
DECLARE @Where nvarchar(1000)
DECLARE @OrderBy nvarchar(100)

--- SET THE SELECT STATEMENT
SET @Select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	U.usrFullName, 
	CL.clNo + ''/'' + F.fileNo as Ref, 
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	F.fileLACategory, 
	T.TimeRecorded, 
	DATEDIFF(d, T.TimeRecorded, GETUTCDATE()) AS DaysDiff, 
	X.cdDesc as DeptDesc,
	Y.cdDesc as FileTypeDesc,	
	Z.cdDesc as FundTypeDesc
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
INNER JOIN
	(	SELECT
			fileID,
			Max(timeRecorded) as TimeRecorded
		FROM
			dbo.[dbTimeLedger] 
		GROUP BY
			fileID
		HAVING
			Max(timeRecorded) < GETUTCDATE() - @PERIODINACT
	) T ON F.fileID = T.fileID
LEFT OUTER JOIN
	dbo.GetCodeLookupDescription ( ''DEPT'' , @UI ) X ON X.cdCode = F.fileDepartment
LEFT OUTER JOIN
	dbo.GetCodeLookupDescription ( ''FILETYPE'' , @UI ) Y ON Y.cdCode = F.FileType
LEFT OUTER JOIN
	dbo.GetCodeLookupDescription ( ''FUNDTYPE'' , @UI ) Z ON Z.cdCode = F.FileFundCode'

-- Where clause
SET @Where = ''

--- FEE EARNER CLAUSE
IF(@FEEEARNER IS NOT NULL)
BEGIN
	SET @Where = @Where + ' F.filePrincipleID = @FEEEARNER'
END

--- FILE DEPARTMENT CLAUSE
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	IF(@Where <> '')
	BEGIN
		SET @Where = @Where + ' AND F.fileDepartment = @DEPARTMENT'
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' F.fileDepartment = @DEPARTMENT'
	END
END

-- File Type
IF (@FILETYPE IS NOT NULL)
BEGIN
	IF(@Where <> '')
	BEGIN
		SET @Where = @Where + ' AND F.fileType = @FILETYPE'
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' F.fileType = @FILETYPE'
	END
END

--- FUNDING TYPE CLAUSE
IF(@FUNDTYPE IS NOT NULL)
BEGIN
	IF(@Where <> '')
	BEGIN
		SET @Where = @Where + ' AND F.fileFundCode = @FUNDTYPE'
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' F.fileFundCode = @FUNDTYPE'
	END
END

--- LEGAL AID CATEGORY CLAUSE
IF(@LACAT IS NOT NULL)
BEGIN
	IF(@Where <> '')
	BEGIN
		SET @Where = @Where + ' AND F.fileLACategory = @LACAT'
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' F.fileLACategory = @LACAT'
	END
END

--- STATUS CLAUSE
IF(@FILESTATUS IS NOT NULL)
BEGIN
	IF(@Where <> '')
	BEGIN
		SET @Where = @Where + ' AND F.FileStatus = @FILESTATUS'
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' F.FileStatus = @FILESTATUS'
	END
END

-- finish the where clause when appropriate
IF (@Where <> '')
BEGIN
	SET @where = N' WHERE ' + @Where
END

-- Order By Clause
SET @OrderBy = N' order by U.usrFullName, Z.cdDesc, DaysDiff DESC'

--- ADD THE CLAUSES TOGETHER
SET @SQL = @Select + @Where + @OrderBy

--- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @PERIODINACT int
	, @FILETYPE nvarchar(20)
	, @DEPARTMENT nvarchar(20)
	, @FUNDTYPE nvarchar(20)
	, @LACAT nvarchar(20)
	, @FEEEARNER int
	, @FILESTATUS nvarchar(20)'
	, @UI
	, @PERIODINACT
	, @FILETYPE
	, @DEPARTMENT
	, @FUNDTYPE
	, @LACAT
	, @FEEEARNER
	, @FILESTATUS

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilInactivityTime] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilInactivityTime] TO [OMSAdminRole]
    AS [dbo];


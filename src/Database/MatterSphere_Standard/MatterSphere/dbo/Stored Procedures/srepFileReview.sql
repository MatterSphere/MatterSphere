

CREATE PROCEDURE [dbo].[srepFileReview]
(
	@UI uUICultureInfo = '{default}',
	@FEEUSRID bigint = null,
	@FILESTATUS nvarchar(30) = null,
	@DEPT nvarchar(30) = null,
	@FILETYPE nvarchar(30) = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null
)

AS

declare @Select nvarchar(1900)
declare @orderby nvarchar(500)
declare @Where nvarchar(2000)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT 
	C.clName,
	F.filedesc, 
	F.filestatus,
	U.usrfullname,
	F.fileReviewDate,
	C.clNo + ''/'' + F.fileNo AS clfileno,
	X.[cdDesc] as DeptDesc,
	Y.[cdDesc] as FileTypeDesc,
	Z.[cdDesc] as FileStatusDesc
FROM
    dbo.dbClient C
INNER JOIN
	dbo.dbFile F ON C.clID = F.clID
INNER JOIN
	dbo.dbuser U ON F.fileprincipleid = U.usrid
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''DEPT'' , @UI ) X ON X.[cdCode] = F.[fileDepartment] 
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''FILETYPE'' , @UI ) Y ON Y.[cdCode] = F.[fileType] 
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''FILESTATUS'' , @UI ) Z ON Z.[cdCode] = F.[fileStatus] '

-- Now set up the WHERE clause
set @where = N''

-- Fee Earner Filter
IF (@FEEUSRID IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' F.fileprincipleID = @FEEUSRID '
END

-- File Status Filter
IF (@FILESTATUS IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.filestatus = @FILESTATUS '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.filestatus = @FILESTATUS '
	END
END

-- Date Filters
IF (@STARTDATE IS NOT NULL)
BEGIN
    IF (@WHERE <> '')
	BEGIN
	    set @WHERE = @WHERE + ' AND (F.fileReviewDate >= @STARTDATE AND F.fileReviewDate < @ENDDATE) '
	END
	ELSE
	BEGIN
	    set @where = @where + ' (F.fileReviewDate >= @STARTDATE AND F.fileReviewDate < @ENDDATE) '
	END
END  

-- Department Filter
IF (@DEPT IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPT '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileDepartment = @DEPT '
	END
END

-- FileType Filter
IF (@FILETYPE IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileType = @FILETYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileType = @FILETYPE '
	END
END

-- Complete the WHERE clause
IF (@WHERE <> '')
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

-- ORDERBY clause
SET @orderby = N' ORDER BY U.usrFullName, C.clName, F.fileReviewDate '

declare @sql nvarchar(4000)
set @sql = Rtrim(@select) + Rtrim(@where) + Rtrim(@orderby)

exec sp_executesql @sql,
 N'
	@UI uUICultureInfo,
	@FEEUSRID bigint,
	@FILESTATUS nvarchar(30),
	@DEPT nvarchar(30),
	@FILETYPE nvarchar(30),
	@STARTDATE DateTime,
	@ENDDATE DateTime',
	@UI,
	@FEEUSRID,
	@FILESTATUS,
	@DEPT,
	@FILETYPE,
	@STARTDATE,
	@ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileReview] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileReview] TO [OMSAdminRole]
    AS [dbo];


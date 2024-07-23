CREATE PROCEDURE [dbo].[srepFilOpenedClosed]
(
	@UI uUICultureInfo='{default}',
	@BRANCH nvarchar(50)= null,
	@LACAT nvarchar(50) = null,
	@FEEEARNER int = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null,
	@CLOSED bit = 0
)

AS 

DECLARE @SELECT nvarchar(2400)
DECLARE @WHERE nvarchar(1500)
DECLARE @ORDERBY nvarchar(100)

--- Select Statement for the base Query
SET @SELECT = N'
SELECT     
	CL.clNo, 
	F.fileNo, 
	U.usrInits, 
        F.fileType, 
	F.fileprincipleid, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc, 
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
        F.Created, 
	F.fileClosed, 
	LACAT.LegAidDesc, 
	F.brID, 
	LACAT.LegAidCategory,
	U.usrfullname
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID 
LEFT OUTER JOIN
	dbo.dbLegalAidCategory LACAT ON F.fileLACategory = LACAT.LegAidCategory
LEFT OUTER JOIN
	dbo.GetCodeLookupDescription ( ''FILETYPE'' , @UI ) CL1 ON CL1.cdCode = F.fileType'

SET @WHERE = N'
WHERE
	F.brID = COALESCE(@BRANCH, F.brID) AND
	COALESCE(F.fileLACategory, '''') = COALESCE(@LACAT, F.fileLACategory, '''') AND
	F.filePrincipleID = COALESCE(@FEEEARNER, F.filePrincipleID) AND
	F.fileStatus NOT LIKE ''LIVEPREINST'' '

--- Build Where Clause
--- SET @WHERE = ''

IF @CLOSED = 1
BEGIN
	IF @WHERE <> ''
		BEGIN
			SET @WHERE = @WHERE + ' AND (F.fileClosed BETWEEN COALESCE(dbo.GetStartDate(@STARTDATE), F.fileClosed) AND COALESCE(dbo.GetEndDate(@ENDDATE), F.fileClosed))'
		END
	ELSE
		BEGIN
			SET @WHERE = @WHERE + ' (F.fileClosed BETWEEN COALESCE(dbo.GetStartDate(@STARTDATE), F.fileClosed) AND COALESCE(dbo.GetEndDate(@ENDDATE), F.fileClosed))'
		END
END
ELSE
BEGIN
	IF @WHERE <> ''
		BEGIN
			SET @WHERE = @WHERE + ' AND (F.Created BETWEEN COALESCE(dbo.GetStartDate(@STARTDATE), F.Created) AND COALESCE(dbo.GetEndDate(@ENDDATE), F.Created))'
		END
	ELSE
		BEGIN
			SET @WHERE = @WHERE + ' (F.Created BETWEEN COALESCE(dbo.GetStartDate(@STARTDATE), F.Created) AND COALESCE(dbo.GetEndDate(@ENDDATE), F.Created))'
		END
END

--- Build OrderBy Clause
SET @ORDERBY = N''

DECLARE @SQL nvarchar(4000)
--- Add Statements together
SET @SQL = @SELECT + @WHERE + @ORDERBY

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @BRANCH nvarchar(50), @LACAT nvarchar(50), @FEEEARNER int,	@STARTDATE datetime, @ENDDATE datetime, @CLOSED bit', @UI, @BRANCH, @LACAT, @FEEEARNER, @STARTDATE, @ENDDATE, @CLOSED

CREATE PROCEDURE [dbo].[srepFilesforClient]
(@UI uUICultureInfo='{default}',
@Clno nvarchar(50) = null,
@filestatus nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     
	CL.clNo, 
	F.fileNo, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	F.Created, 
	F.fileClosed, 
	F.fileType, 
    U.usrInits AS FeeInits, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc, 
	CL.clName, 
    U.usrFullName
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID  
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] = F.fileType'

--- Build Where Clause
SET @WHERE = ''

if coalesce(@clno, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	     set @where = @where + ' AND CL.clno = '''  + @clno + ''''	
	END
   ELSE
	BEGIN
	     set @where = @where + ' CL.clno = '''  + @clno + ''''
	END
END

if @where <>'' set @where = @where + ' AND '
set @where = @where + ' F.fileStatus like ''%LIVE%'''

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N' order by F.Created'

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI


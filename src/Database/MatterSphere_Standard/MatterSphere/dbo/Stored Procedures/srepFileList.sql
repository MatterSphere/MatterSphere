

CREATE PROCEDURE [dbo].[srepFileList]
(@UI uUICultureInfo='{default}',
@FileType nvarchar(50) = null,
@FeeEarner nvarchar(50) = null,
@Department nvarchar(50) = null,
@FundingType nvarchar(50) = null,
@LACat nvarchar(50) = null,
@BrID nvarchar(50) = null,
@FileStat nvarchar(50) = null,
@Interactive nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)

AS 

declare @Select nvarchar(max)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbUser.usrInits AS FeeInits, dbo.dbUser.usrFullName, dbo.dbClient.clNo, dbo.dbClient.clName, dbo.dbFile.fileNo, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, 
                      dbo.dbFile.fileType, dbo.dbFile.fileLACategory, dbo.dbFile.fileAllowExternal, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbFile.fileType, '''') + ''~'') 
                      AS FileTypeDesc, dbo.dbFile.fileFundCode, dbo.dbFile.fileDepartment, COALESCE(CL2.cdDesc, ''~'' + NULLIF(dbFile.fileDepartment, '''') + ''~'')
                      AS DeptDesc
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID
			LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] = dbFile.fileType
			LEFT JOIN dbo.GetCodeLookupDescription ( ''DEPT'', @UI ) CL2 ON CL2.[cdCode] = dbFile.fileDepartment'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@filetype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.filetype = ''' + @filetype + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.filetype = ''' + @filetype + ''''
	END
END

if coalesce(@feeearner, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbuser.usrid = ''' + @feeearner + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbuser.usrid = ''' + @feeearner + ''''
	END
END

if coalesce(@department, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.filedepartment = ''' + @department + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.filedepartment = ''' + @department + ''''
	END
END

if coalesce(@fundingtype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.filefundcode = ''' + @fundingtype + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.filefundcode = ''' + @fundingtype + ''''
	END
END

if coalesce(@LACat, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.filelacategory = ''' + @lacat + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.filelacategory = ''' + @lacat + ''''
	END
END

if coalesce(@brid, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.brid = ''' + @brid + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.brid = ''' + @brid + ''''
	END
END

if coalesce(@filestat, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.filestatus = ''' + @filestat + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.filestatus = ''' + @filestat + ''''
	END
END

if coalesce(@interactive, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.fileallowexternal = ''' + @interactive + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.fileallowexternal = ''' + @interactive + ''''
	END
END


print @BEGINDATE
Print @ENDDATE

if coalesce(@BEGINDATE, '') <> ''
BEGIN
    IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		BEGIN
		    set @where = @where + ' dbo.dbfile.created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' dbo.dbfile.created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND dbo.dbfile.created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND dbo.dbfile.created BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
END  


--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

---- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileList] TO [OMSAdminRole]
    AS [dbo];


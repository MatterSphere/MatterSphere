

CREATE PROCEDURE [dbo].[srepFileAnalysis]
(@UI uUICultureInfo='{default}',
@FileType nvarchar(50) = null,
@FileDepartment nvarchar(50) = null,
@brID nvarchar(50)= null,
@FundingType nvarchar(50) = null,
@LACategory nvarchar(50) = null,
@feeEarnerID int = null,
@FileStat nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)
AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbFile.fileStatus, dbo.dbFile.fileDepartment, 
                      dbo.dbFile.fileType, dbo.dbUser.usrInits AS FeeInits, dbo.dbFile.fileNo, dbo.dbFile.fileLACategory, dbo.dbFile.brID, 
                      dbo.dbFile.fileFundCode, dbo.dbFile.Created, dbo.dbUser.usrFullName
FROM         dbo.dbFile INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@filetype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.Filetype = ''' + @filetype + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.Filetype = ''' + @filetype + ''''
	END
END

if coalesce(@filedepartment, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.Filedepartment = ''' + @filedepartment + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.FileDepartment = ''' + @filedepartment + ''''
	END
END

if coalesce(@brID, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.brID = ' + @brID 
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.brID = ' + @brID 
	END
END

if coalesce(@FundingType, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.fileFundCode = ''' + @FundingType + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.fileFundCode = ''' + @FundingType + ''''
	END
END  

if coalesce(@FileStat, '') <> ''
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


if coalesce(@LAcategory, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.fileLACategory = ''' + @lacategory + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.fileLACategory = ''' + @lacategory + ''''
	END
END

if not @FeeEarnerID is null
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.filePrincipleID = ' + convert(nvarchar, @FeeEarnerID)
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.filePrincipleID = ' + convert(nvarchar, @FeeEarnerID)
	END
END  

print @BEGINDATE
Print @ENDDATE

if coalesce(@BEGINDATE, '') <> ''
BEGIN
   IF @where is null
	BEGIN
	    if coalesce(@ENDDATE, '') <> ''
		BEGIN
		    set @where = @where + ' AND dbo.dbfile.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND dbo.dbfile.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND dbo.dbfile.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND dbo.dbfile.Created BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
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
    ON OBJECT::[dbo].[srepFileAnalysis] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileAnalysis] TO [OMSAdminRole]
    AS [dbo];


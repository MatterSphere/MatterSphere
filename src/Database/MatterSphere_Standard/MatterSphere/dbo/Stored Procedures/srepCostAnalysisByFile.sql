

CREATE PROCEDURE [dbo].[srepCostAnalysisByFile]
(@UI uUICultureInfo='{default}',
@FileType nvarchar(50) = null,
@FeeEarner nvarchar(50) = null,
@Department nvarchar(50) = null,
@FundingType nvarchar(50) = null,
@LACat nvarchar(50) = null,
@FileStat nvarchar(50) = null,
@LAContract nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)
AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbUser.usrInits, dbo.dbUser.usrFullName, dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbFile.fileID, dbo.dbFile.fileFundRef AS CertNo, 
                      dbo.dbFile.Created, dbo.dbFile.fileDesc, dbo.dbFile.fileDepartment, dbo.dbFile.fileType, dbo.dbFile.fileFundCode, 
                      COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbFile.fileFundCode, '''') + ''~'') AS FundTypeDesc, dbo.dbFile.fileLACategory, dbo.dbClient.clName, 
                      dbo.dbFile.fileStatus, dbo.dbFileLegal.MatLAContract
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID LEFT OUTER JOIN
                      dbo.dbFileLegal ON dbo.dbFile.fileID = dbo.dbFileLegal.fileID
			LEFT JOIN dbo.GetCodeLookupDescription ( ''FUNDTYPE'', @UI ) CL1 ON CL1.[cdCode] = dbFile.fileFundCode'

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

if coalesce(@LAContract, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfilelegal.matLAContract = ''' + @LAContract + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfilelegal.matLAContract = ''' + @LAContract + ''''
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
    ON OBJECT::[dbo].[srepCostAnalysisByFile] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCostAnalysisByFile] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[srepFileOpenedClosed]
(@UI uUICultureInfo='{default}',
@brID nvarchar(50)= null,
@LACategory nvarchar(50) = null,
@feePrincipleID int = null,
@begindate datetime=null,
@enddate datetime=null)
AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbUser.usrInits, 
                      dbo.dbFile.fileType, dbo.dbfile.fileprincipleid, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbFile.fileType, '''') + ''~'') AS FileTypeDesc, dbo.dbClient.clName, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, 
                      dbo.dbFile.Created, dbo.dbFile.fileClosed, dbo.dbLegalAidCategory.LegAidDesc, dbo.dbFile.brID, dbo.dbLegalAidCategory.LegAidCategory, dbo.dbuser.usrfullname
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID LEFT OUTER JOIN
                      dbo.dbLegalAidCategory ON dbo.dbFile.fileLACategory = dbo.dbLegalAidCategory.LegAidCategory
			LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] = dbFile.fileType'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

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

if not @FeePrincipleID is null
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.filePrincipleID = ' + convert(nvarchar, @FeePrincipleID)
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.filePrincipleID = ' + convert(nvarchar, @FeePrincipleID)
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
		    set @where = @where + ' dbo.dbfile.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' dbo.dbfile.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
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
    ON OBJECT::[dbo].[srepFileOpenedClosed] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileOpenedClosed] TO [OMSAdminRole]
    AS [dbo];


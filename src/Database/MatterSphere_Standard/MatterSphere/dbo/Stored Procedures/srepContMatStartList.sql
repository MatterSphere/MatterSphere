

CREATE PROCEDURE [dbo].[srepContMatStartList]
(@UI uUICultureInfo='{default}',
@ContractName nvarchar(50) = null,
@LACategory nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT
	dbo.dbLegalAidContract.LAContractName, 
	dbo.dbLegalAidContract.LAContractCode, 
	dbo.dbLegalAidContract.LAContractRef, 
	dbo.dbFile.Created, 
	dbo.dbFileLegal.MatLAMatType, 
	dbo.dbClient.clNo, 
	dbo.dbFile.fileNo, 
	dbo.dbFile.filePrincipleID, 
	dbo.dbUser.usrInits, 
	dbo.dbUser.usrFullName, 
	replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	dbo.dbClient.clName, 
	dbo.dbFile.fileLACategory
FROM    
	dbo.dbClient 
INNER JOIN
	dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID 
INNER JOIN
	dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID 
INNER JOIN
	dbo.dbFileLegal ON dbFile.fileid = dbFileLegal.fileid
inner join  
	dbo.dbLegalAidContract on dbo.dbLegalAidContract.LAContractCode = dbo.dbFileLegal.MatLAContract '

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@ContractName, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfilelegal.matlacontract = ''' + @contractname + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfilelegal.matlacontract = ''' + @contractname + ''''
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
    ON OBJECT::[dbo].[srepContMatStartList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContMatStartList] TO [OMSAdminRole]
    AS [dbo];


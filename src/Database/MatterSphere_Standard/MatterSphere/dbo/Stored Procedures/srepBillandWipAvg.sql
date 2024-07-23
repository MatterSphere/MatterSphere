

CREATE PROCEDURE [dbo].[srepBillandWipAvg]
(@UI uUICultureInfo='{default}',
@FileType nvarchar(50) = null,
@FileDepartment nvarchar(50) = null,
@brID nvarchar(50)= null,
@FileStat nvarchar(50) = null,
@FundingType nvarchar(50) = null,
@LACategory nvarchar(50) = null,
@feePrincipleID int = null,
@begindate datetime=null,
@enddate datetime=null)

AS 

declare @Select nvarchar(1800)
declare @Where nvarchar(1500)
declare @orderby nvarchar(100)
declare @groupby nvarchar(600)

--- Select Statement for the base Query
set @select = N'SELECT     COUNT(dbo.dbFile.fileID) AS FileIDCount, dbo.dbFile.fileFundCode, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbFile.fileFundCode, '''') + ''~'') 
                      AS FundTypeDesc, dbo.dbFile.fileType, COALESCE(CL2.cdDesc, ''~'' + NULLIF(dbFile.fileType, '''') + ''~'') AS FileTypeDesc, dbo.dbFile.fileWIP, 
                      SUM(dbo.dbBillInfo.billProCosts) AS BillProCostsSum, dbo.dbBillInfo.billProCosts, dbo.dbFile.fileID, dbo.dbUser.usrInits, dbo.dbUser.usrFullName, 
                      dbo.dbFile.fileDepartment, dbo.dbFile.brID, dbo.dbFile.fileLACategory, dbo.dbFile.fileStatus, dbo.dbFile.Created
FROM         dbo.dbFile INNER JOIN
                      dbo.dbBillInfo ON dbo.dbFile.fileID = dbo.dbBillInfo.fileID INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID
			LEFT JOIN dbo.GetCodeLookupDescription ( ''FUNDTYPE'', @UI ) CL1 ON CL1.[cdCode] =  dbFile.fileFundCode
			LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL2 ON CL2.[cdCode] =  dbFile.fileType'

--- Set Groupby Clause
set @groupby = N' GROUP BY dbo.dbFile.fileFundCode, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbFile.fileFundCode, '''') + ''~''), dbo.dbFile.fileType, 
                      COALESCE(CL2.cdDesc, ''~'' + NULLIF(dbFile.fileType, '''') + ''~''), dbo.dbFile.fileWIP, dbo.dbBillInfo.billProCosts, dbo.dbFile.fileID, 
                      dbo.dbUser.usrInits, dbo.dbUser.usrFullName, dbo.dbFile.fileDepartment, dbo.dbFile.brID, dbo.dbFile.fileLACategory, dbo.dbFile.fileStatus, 
                      dbo.dbFile.Created'

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

if coalesce(@filestat, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.Filestatus = ''' + @filestat + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.Filestatus = ''' + @filestat+ ''''
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
set @sql = @select + @where + @orderby + @groupby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepBillandWipAvg] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepBillandWipAvg] TO [OMSAdminRole]
    AS [dbo];


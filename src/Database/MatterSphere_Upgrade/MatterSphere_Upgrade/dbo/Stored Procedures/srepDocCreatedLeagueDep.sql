CREATE PROCEDURE [dbo].[srepDocCreatedLeagueDep]
(@UI uUICultureInfo='{default}',
@UserName nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)
AS 

declare @Select nvarchar(MAX)
declare @Where nvarchar(1800)
declare @orderby nvarchar(100)
declare @groupby nvarchar(300)

--- Select Statement for the base Query
set @select = N'SELECT     
	DOC.Createdby, 
	COUNT_BIG(DOC.docID) AS Total, 
	DOC.docType, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(DOC.docType, '''') + ''~'') AS DocTypeDesc, 
	U.usrFullName, 
	F.fileDepartment
FROM         
	dbo.dbDocument DOC
INNER JOIN
	dbo.dbUser U ON DOC.Createdby = U.usrID 
LEFT OUTER JOIN
	dbo.dbFile F ON DOC.fileID = F.fileID
LEFT JOIN dbo.GetCodeLookupDescription ( ''DOCTYPE'', @UI ) CL1 ON CL1.[cdCode] = DOC.docType'

--- Build the GroupBy clause
set @groupby = N' GROUP BY DOC.CreatedBy, DOC.docType , U.usrFullName, F.fileDepartment, COALESCE(CL1.cdDesc, ''~'' + NULLIF(DOC.docType, '''') + ''~'')'

--- Build Where Clause
SET @WHERE = ''

if coalesce(@UserName, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND U.usrID = ''' + @username + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' U.usrid = ''' + @username + ''''
	END
END

if coalesce(@BEGINDATE, '') <> ''
BEGIN
   IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		
		BEGIN
		  print 'where=:' + @where + ': enddate=:' + convert(nvarchar(20),@enddate) + ':'
		    set @where = @where + ' DOC.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' DOC.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND DOC.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND DOC.Created BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
END  

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby + @Groupby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

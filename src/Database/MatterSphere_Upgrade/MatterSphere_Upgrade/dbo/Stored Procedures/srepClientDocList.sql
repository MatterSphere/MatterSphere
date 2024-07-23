CREATE PROCEDURE [dbo].[srepClientDocList]
(@UI uUICultureInfo='{default}',
@clno nvarchar(50) = null,
@fileno nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	COALESCE(D.docAuthored, D.Created) AS Created, 
	D.Createdby, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	CL.clNo, 
	F.fileNo, 
	U.usrInits, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(D.docType, '''') + ''~'') AS DocTypeDesc,
	D.docDirection, 
	CL.clName, 
	U.usrFullName, 
	D.docID
FROM    
	dbo.dbClient CL 
INNER JOIN
        dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
        dbo.dbDocument D ON F.fileID = D.fileID 
INNER JOIN
        dbo.dbUser U ON D.Createdby = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''DOCTYPE'', @UI ) CL1 ON CL1.[cdCode] = D.docType'

--- Build Where Clause
SET @WHERE = ''

if coalesce(@clno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND CL.clno = ''' + @clno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' CL.clno = ''' + @clno + ''''
	END
END

if coalesce(@fileno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND F.fileno = ''' + @fileno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' F.fileno = ''' + @fileno + ''''
	END
END

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N' ORDER BY Created'

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

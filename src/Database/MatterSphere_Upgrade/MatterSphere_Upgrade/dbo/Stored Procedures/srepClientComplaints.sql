CREATE PROCEDURE [dbo].[srepClientComplaints]
(@UI uUICultureInfo='{default}',
@clno nvarchar(50) = null,
@compactive nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	CL.clNo, 
	CL.clName, 
	C.compType, 
	C.compDesc, 
	C.compRef, 
        C.compEstCompDate, 
	C.CreatedBy, 
	C.Created, 
	C.compCompleted, 
        C.compNote, 
	U.usrInits, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(C.compType, '''') + ''~'') AS Expr1
FROM    
	dbo.dbComplaints C
INNER JOIN
        dbo.dbClient CL ON C.clID = CL.clID 
LEFT OUTER JOIN
        dbo.dbUser U ON C.CreatedBy = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''COMPLAINT'', @UI ) CL1 ON CL1.[cdCode] =  C.compType'

--- Build Where Clause
SET @WHERE = 'C.compActive = 1'

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

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

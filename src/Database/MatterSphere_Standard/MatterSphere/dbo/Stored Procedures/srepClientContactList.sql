

CREATE PROCEDURE [dbo].[srepClientContactList]
(@UI uUICultureInfo='{default}',
@clno nvarchar(50) = null,
@contactive nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	CL.clno, 
	CL.clname, 
	C.contName, 
	A.addLine1, 
	A.addLine2, 
	A.addLine3, 
	A.addLine4, 
        A.addLine5, 
	A.addPostcode, 
	C.contXMASCard, 
	C.contApproved, 
	C.contGrade, 
        CLU.cdType, 
	C.contAddFilter, 
	C.contTypeCode, 
	CLU.cdDesc, 
	CLU.cdCode, 
        CL.clNo
FROM    
	dbo.dbContact C
INNER JOIN
        dbo.dbCodeLookup CLU ON C.contTypeCode = CLU.cdCode
INNER JOIN
        dbo.dbAddress A ON C.contDefaultAddress = A.addID 
INNER JOIN
        dbo.dbClientContacts CC ON C.contID = CC.contID
INNER JOIN
        dbo.dbClient CL ON CC.clID = CL.clID'

--- Build Where Clause
SET @WHERE = 'CLU.cdType = N''CONTTYPE'' AND CC.clactive = ''1'''

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
set @orderby = N' ORDER BY C.contname'

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientContactList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientContactList] TO [OMSAdminRole]
    AS [dbo];


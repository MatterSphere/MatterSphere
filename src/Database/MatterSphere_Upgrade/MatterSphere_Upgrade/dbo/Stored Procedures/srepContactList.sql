

CREATE PROCEDURE [dbo].[srepContactList]
(@UI uUICultureInfo='{default}',
@ContType nvarchar(50) = null,
@SubContType nvarchar(50) = null,
@ContGrade int = null,
@Location nvarchar(64) = null,
@PostCode nvarchar(20) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     
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
	CLU.cdCode
FROM    
	dbo.dbContact C 
INNER JOIN
	dbo.dbCodeLookup CLU ON C.contTypeCode = CLU.cdCode  
INNER JOIN
	dbo.dbAddress A ON C.contDefaultAddress = A.addID'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ' CLU.cdtype = N''CONTTYPE'''

if coalesce(@conttype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND CLU.cddesc = ''' + @conttype + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' CLU.cddesc = ''' + @conttype + ''''
	END
END

if coalesce(@subconttype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND CLU.cdcode = ''' + @subconttype + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' CLU.cdcode = ''' + @subconttype + ''''
	END
END

if coalesce(@contgrade, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND C.contgrade = ' + @contgrade + '''' 
	END
   ELSE
	BEGIN
	    set @where = @where + ' C.contgrade = ' + @contgrade + ''''
	END
END

if coalesce(@location, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND A.addline1 + A.addline2 + A.addline3 + A.addline4 + A.addline5 LIKE ''' + '%' + @location + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' A.addline1 + A.addline2 + A.addline3 + A.addline4 + A.addline5 LIKE ''' + '%' + @location + ''''
	END
END  

if coalesce(@PostCode, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND A.addpostcode LIKE ''' + '%' + @postcode + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' A.addpostcode LIKE ''' + '%' + @postcode + ''''
	END
END 

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

---- Build OrderBy Clause
set @orderby = N' ORDER BY C.contname'

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContactList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContactList] TO [OMSAdminRole]
    AS [dbo];


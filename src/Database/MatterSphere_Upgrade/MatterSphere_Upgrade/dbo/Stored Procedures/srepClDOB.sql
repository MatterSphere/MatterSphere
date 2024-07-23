

CREATE PROCEDURE [dbo].[srepClDOB]
(@UI uUICultureInfo='{default}',
@ClientType nvarchar(50) = null,
@FirmContact nvarchar(50) = null,
@ClientName nvarchar(128) = null,
@begindate datetime=null,
@enddate datetime=null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	CL.clNo, 
	CL.clName, 
	CI.contDOB, 
	A.addLine1, 
	C.contName, 
	DATEDIFF(yyyy, CI.contDOB, GETDATE()) AS Age, 
	DATEPART(d, CI.contDOB) AS Day, 
	CL.feeusrID, 
        U.usrInits AS FirmContInits, 
	U.usrFullName AS FirmContName
FROM    
	dbo.dbContact C
INNER JOIN
        dbo.dbClientContacts CC ON C.contID = CC.contID 
INNER JOIN
        dbo.dbClient CL ON CC.clID = CL.clID 
INNER JOIN
        dbo.dbUser U ON CL.feeusrID = U.usrID 
LEFT OUTER JOIN
        dbo.dbContactIndividual CI ON C.contID = CI.contID 
LEFT OUTER JOIN
        dbo.dbAddress A ON C.contDefaultAddress = A.addID'

--- Build Where Clause
SET @WHERE = 'CC.clActive = 1'

if coalesce(@clienttype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND CL.cltypecode = ''' + @clienttype + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' CL.cltypecode = ''' + @clienttype + ''''
	END
END

if coalesce(@firmcontact, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND CL.feeusrid = ''' + @firmcontact + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' CL.feeusrid = ''' + @firmcontact + ''''
	END
END

if coalesce(@ClientName, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND CL.clname LIKE ''' + '%' + @ClientName + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' CL.clname LIKE ''' + '%' +  @ClientName + ''''
	END
END

print @BEGINDATE
Print @ENDDATE

if coalesce(@begindate, '') <> ''
BEGIN
    IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		BEGIN
		    set @where = @where + ' CI.contDOB BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' CI.contDOB BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND CI.contDOB BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND CI.contDOB BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
END  
ELSE
BEGIN
IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND (NOT (CI.contdob IS NULL)) '
	END
   ELSE
	BEGIN
	     set @where = @where + ' (NOT (CI.contdob IS NULL)) '
	END
END
--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N' ORDER BY CL.clname'

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClDOB] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClDOB] TO [OMSAdminRole]
    AS [dbo];


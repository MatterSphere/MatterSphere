CREATE PROCEDURE [dbo].[srepAppointmentByDate]
(@UI uUICultureInfo='{default}',
@AppType nvarchar(50) = null,
@AppLoc nvarchar(50) = null,
@ClNo nvarchar(50) = null,
@feePrincipleID int = null,
@AppDesc nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	A.appDate, 
	A.appLocation, 
	A.appDesc, 
	A.appType, 
        A.appLocation, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.appType, '''') + ''~'') AS AppTypeDesc, 
	CL.clNo, 
	F.fileNo, 
	U.usrInits, 
	U.usrFullName, 
        A.feeusrID, 
	U1.usrInits AS InitsWith, 
	U1.usrFullName AS FullNameWith
FROM    
	dbo.dbClient CL
INNER JOIN
        dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
        dbo.dbUser U ON F.filePrincipleID = U.usrID 
INNER JOIN
        dbo.dbAppointments A ON F.fileID = A.fileID 
INNER JOIN
        dbUser U1 ON A.feeusrID = U1.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''APPTYPE'', @UI ) CL1 ON CL1.[cdCode] =  A.appType'

---- Build Where Clause
SET @WHERE = ''

if coalesce(@apptype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND A.apptype = ''' + @apptype + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' A.apptype = ''' + @apptype + ''''
	END
END

if coalesce(@apploc, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND A.applocation LIKE ''' + '%' + @apploc + '%' + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' A.applocation LIKE ''' + '%' + @apploc + '%' + ''''
	END
END

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

if not @FeePrincipleID is null
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND U1.usrid = ' + convert(nvarchar, @FeePrincipleID)
	END
   ELSE
	BEGIN
	    set @where = @where + ' U1.usrid = ' + convert(nvarchar, @FeePrincipleID)
	END
END  

if coalesce(@AppDesc, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND A.appDesc LIKE ''' + '%' + @AppDesc + '%' + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' A.appDesc LIKE ''' + '%' + @AppDesc + '%' + ''''
	END
END

if coalesce(@BEGINDATE, '') <> ''
BEGIN
   IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		BEGIN
		    set @where = @where + ' A.appdate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' A.appdate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND A.appdate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND A.appdate BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
END  

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N' ORDER BY A.appDate ASC'

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

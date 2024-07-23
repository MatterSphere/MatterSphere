

CREATE PROCEDURE [dbo].[srepPreConvDate]
(@UI uUICultureInfo='{default}',
@FirmCont nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbClient.clPreClientConvDate, dbo.dbClient.Created, DATEDIFF(d, dbo.dbClient.created, 
                      dbo.dbClient.clPreClientConvDate) AS daysdiff, dbo.dbClient.clName, dbo.dbClient.clDefaultContact, dbo.dbUser.usrInits AS FirmContInits, 
                      dbo.dbUser.usrFullName AS FirmContName, dbo.dbClient.clPreClient
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID AND dbo.dbClient.clPreclientconvdate > 0 LEFT OUTER JOIN
                      dbo.dbUser ON dbo.dbClient.feeusrID = dbo.dbUser.usrID'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@firmcont, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbclient.feeusrid = ''' + @firmcont + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbclient.feeusrid = ''' + @firmcont + ''''
	END
END

if coalesce(@BEGINDATE, '') <> ''
BEGIN
    IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		BEGIN
		    set @where = @where + ' dbo.dbclient.clpreclientconvdate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' dbo.dbclient.clpreclientconvdate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND dbo.dbclient.clpreclientconvdate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND dbo.dbclient.clpreclientconvdate BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
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
    ON OBJECT::[dbo].[srepPreConvDate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPreConvDate] TO [OMSAdminRole]
    AS [dbo];


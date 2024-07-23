

CREATE PROCEDURE [dbo].[srepSourceIntroTop40]
(@UI uUICultureInfo='{default}',
@FeeEarner nvarchar(50) = null,
@Classification nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)
AS 

declare @Select nvarchar(1700)
declare @Where nvarchar(1700)
declare @groupby nvarchar(500)
declare @orderby nvarchar(50)
declare @having nvarchar(50)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbCodeLookup.cdDesc, dbo.dbContact.contName, dbo.dbClient.Created, 
                      dbo.dbCodeLookup.cdType, dbo.dbClient.clPreClient, dbo.dbUser.usrInits, dbo.dbUser.usrFullName, dbo.dbfeeearner.feeusrid
FROM         dbo.dbClient INNER JOIN
                      dbo.dbCodeLookup ON dbo.dbClient.clSource = dbo.dbCodeLookup.cdCode AND dbo.dbcodelookup.CDTYPE = ''SOURCE'' INNER JOIN
                      dbo.dbContact ON dbo.dbClient.clDefaultContact = dbo.dbContact.contID INNER JOIN
                      dbo.dbFeeEarner ON dbo.dbClient.feeusrID = dbo.dbFeeEarner.feeusrID INNER JOIN
                      dbo.dbUser ON dbo.dbFeeEarner.feeusrID = dbo.dbUser.usrID '

set @groupby = N' GROUP BY dbo.dbCodeLookup.cdDesc, dbo.dbContact.contName, dbo.dbClient.Created, dbo.dbCodeLookup.cdType, dbo.dbClient.clPreClient, 
                      dbo.dbUser.usrInits, dbo.dbUser.usrFullName, dbo.dbfeeearner.feeusrid'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''
SET @HAVING = ''

if coalesce(@FeeEarner, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfeeearner.feeusrid = ''' + @feeearner + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfeeearner.feeusrid = ''' + @feeearner + ''''
	END
END

if coalesce(@classification, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbclient.clpreclient = ''' + @classification + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbclient.clpreclient = ''' + @classification + ''''
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
		    set @where = @where + ' dbo.dbclient.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' dbo.dbclient.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND dbo.dbclient.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND dbo.dbclient.Created BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
END  

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

if @having <> ''
BEGIN
	set @having = N' HAVING ' + @having
END

---- Build OrderBy Clause
set @orderby = N' ORDER BY CDDESC '

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @having + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepSourceIntroTop40] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepSourceIntroTop40] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[srepLAExBar]
(@UI uUICultureInfo='{default}',
@begindate datetime=null,
@enddate datetime=null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbContact.contTypeCode, dbo.dbContact.contName, dbo.dbContact.contApproved, dbo.dbContact.contApprRevokedOn, dbo.dbContact.Created, 
                      dbo.dbContactAddresses.contaddID, dbo.dbAddress.addLine1, dbo.dbAddress.addLine2, dbo.dbAddress.addLine3
FROM         dbo.dbContact INNER JOIN
                      dbo.dbContactAddresses ON dbo.dbContact.contID = dbo.dbContactAddresses.contID AND dbo.dbContact.contTypeCode = N''BARRISTER'' OR 
                      dbo.dbContact.contTypeCode = N''EXPERT'' AND dbo.dbContact.contApproved = 1 INNER JOIN
                      dbo.dbAddress ON dbo.dbContactAddresses.contaddID = dbo.dbAddress.addID'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

print @BEGINDATE
Print @ENDDATE

if coalesce(@BEGINDATE, '') <> ''
BEGIN
    IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		BEGIN
		    set @where = @where + ' dbo.dbcontact.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' dbo.dbcontact.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND dbo.dbcontact.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND dbo.dbcontact.Created BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
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
    ON OBJECT::[dbo].[srepLAExBar] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLAExBar] TO [OMSAdminRole]
    AS [dbo];


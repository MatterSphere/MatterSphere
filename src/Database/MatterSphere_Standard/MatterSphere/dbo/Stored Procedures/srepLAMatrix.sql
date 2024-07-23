

CREATE PROCEDURE [dbo].[srepLAMatrix]
(@UI uUICultureInfo='{default}',
@LAGrade nvarchar(50) = null)

AS 

declare @Select nvarchar(MAX)
declare @Where nvarchar(1400)
declare @orderby nvarchar(100)
declare @groupby nvarchar(400)
declare @having nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbLegalAidActivities.ActivityCode, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbo.dbLegalAidActivities.ActivityCode, '''') + ''~'') AS ActivityDesc, 
                      dbo.dbLegalAidActivities.ActivityLegalAidCat, dbo.dbLegalAidActivities.ActivityCharge, 
                      dbo.dbLegalAidActivities.ActivityLegalAidGrade
FROM         dbo.dbActivities INNER JOIN
                      dbo.dbLegalAidActivities ON dbo.dbActivities.actCode = dbo.dbLegalAidActivities.ActivityCode INNER JOIN
                      dbo.dbLegalAidCategory ON dbo.dbLegalAidActivities.ActivityLegalAidCat = dbo.dbLegalAidCategory.LegAidCategory
			LEFT JOIN dbo.GetCodeLookupDescription ( ''TIMEACTCODE'' , @UI ) CL1 ON CL1.cdCode =  dbo.dbLegalAidActivities.ActivityCode'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''
SET @HAVING = ''

if coalesce(@LAGrade, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbLegalAidActivities.ActivityLegalAidGrade = ''' + @lagrade + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbLegalAidActivities.ActivityLegalAidGrade = ''' + @lagrade + ''''
	END
END
ELSE
BEGIN
	SET @LAGRADE = 1
	IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbLegalAidActivities.ActivityLegalAidGrade = ''' + @lagrade + ''''
	END
   	ELSE
		BEGIN
	     set @where = @where + ' dbo.dbLegalAidActivities.ActivityLegalAidGrade = ''' + @lagrade + ''''
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
    ON OBJECT::[dbo].[srepLAMatrix] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLAMatrix] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[srepComplaints]
(@UI uUICultureInfo='{default}',
@clno nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbClient.clName, dbo.dbComplaints.compType, dbo.dbComplaints.compDesc, dbo.dbComplaints.compRef, 
                      dbo.dbComplaints.compEstCompDate, dbo.dbComplaints.CreatedBy, dbo.dbComplaints.Created, dbo.dbComplaints.compCompleted, 
                      dbo.dbComplaints.compNote, dbo.dbUser.usrInits, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbComplaints.compType, '''') + ''~'') AS Expr1
FROM         dbo.dbComplaints INNER JOIN
                      dbo.dbClient ON dbo.dbComplaints.clID = dbo.dbClient.clID LEFT OUTER JOIN
                      dbo.dbUser ON dbo.dbComplaints.CreatedBy = dbo.dbUser.usrID
			LEFT JOIN dbo.GetCodeLookupDescription ( ''COMPLAINT'', @UI ) CL1 ON CL1.[cdCode] = dbComplaints.compType'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@clno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbclient.clno = ''' + @clno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbclient.clno = ''' + @clno + ''''
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
    ON OBJECT::[dbo].[srepComplaints] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepComplaints] TO [OMSAdminRole]
    AS [dbo];


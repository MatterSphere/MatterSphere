CREATE  PROCEDURE [dbo].[srepComplaintNote]
(@UI uUICultureInfo='{default}',
@Clno nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbComplaints.compID, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbComplaints.compType, '''') + ''~'') AS CompTypeDesc, dbo.dbClient.clNo, 
                      dbo.dbClient.clName, dbo.dbComplaints.compType, dbo.dbComplaints.compDesc, dbo.dbComplaints.compRef, dbo.dbComplaints.compEstCompDate, 
                      dbo.dbComplaints.CreatedBy, dbo.dbComplaints.Created, dbo.dbComplaints.compCompleted, dbo.dbComplaints.compNote, 
                      dbo.dbUser.usrInits AS CreatedByInits, dbo.dbUser.usrFullName AS CreatedByName
FROM         dbo.dbComplaints INNER JOIN
                      dbo.dbClient ON dbo.dbComplaints.clID = dbo.dbClient.clID LEFT OUTER JOIN
                      dbo.dbUser ON dbo.dbComplaints.CreatedBy = dbo.dbUser.usrID
			LEFT JOIN dbo.GetCodeLookupDescription ( ''COMPLAINT'', @UI ) CL1 ON CL1.[cdCode] = dbComplaints.compType'

---- Build Where Clause
SET @WHERE = ''

if coalesce(@clno, '') <> ''
BEGIN
   IF @where is null
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


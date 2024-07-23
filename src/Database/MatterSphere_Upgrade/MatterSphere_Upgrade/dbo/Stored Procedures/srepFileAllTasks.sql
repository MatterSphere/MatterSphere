CREATE PROCEDURE [dbo].[srepFileAllTasks]
(@UI uUICultureInfo='{default}',
@clno nvarchar(50) = null,
@fileno nvarchar(50) = null)

AS 

declare @Select nvarchar(max)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	dbo.dbTasks.tskType, 
	dbo.dbTasks.tskDesc, 
	dbo.dbTasks.tskDue, 
	dbo.dbTasks.tskNotes, 
	dbo.dbTasks.tskCompleted, 
	dbo.dbTasks.tskComplete, 
        dbo.dbTasks.tskReminder, 
	dbo.dbTasks.tskActive, 
	dbo.dbTasks.Created, 
	dbo.dbTasks.Updated, 
	dbo.dbUser.usrInits, 
	dbo.dbUser.usrFullName, 
    COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbtasks.tskType, '''') + ''~'')  as TaskType, 
	dbo.dbUser.usrID, 
	dbo.dbClient.clNo, 
	dbo.dbFile.fileNo, 
	dbo.dbClient.clName, 
	dbo.dbFile.fileDesc
FROM         
	dbo.dbTasks 
INNER JOIN
	dbo.dbUser ON dbo.dbTasks.feeusrID = dbo.dbUser.usrID 
INNER JOIN
	dbo.dbFile ON dbo.dbTasks.fileID = dbo.dbFile.fileID 
INNER JOIN
	dbo.dbClient ON dbo.dbFile.clID = dbo.dbClient.clID
LEFT JOIN dbo.GetCodeLookupDescription ( ''TASKTYPE'', @UI ) CL1 ON CL1.[cdCode] = dbtasks.tskType'

---- Build Where Clause
SET @WHERE = ' dbo.dbTasks.tskActive = 1'

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

if coalesce(@fileno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.fileno = ''' + @fileno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.fileno = ''' + @fileno + ''''
	END
END

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

---- Build OrderBy Clause
set @orderby = N' order by dbo.dbtasks.tskDue'

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI


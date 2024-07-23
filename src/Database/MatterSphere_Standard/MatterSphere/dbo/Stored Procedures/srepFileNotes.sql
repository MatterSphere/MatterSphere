

CREATE PROCEDURE [dbo].[srepFileNotes]
(@UI uUICultureInfo='{default}',
@fileid nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	U.usrInits AS CreatedInits, 
	U1.usrInits AS UpdatedInits,
	U2.usrFullName AS FileHandler,
	F.Created, 
	F.Updated, 
	CL.clNo, 
	F.fileNo, 
        F.fileNotes, 
	F.fileExternalNotes,
	F.fileDesc
FROM    
	dbo.dbClient CL
INNER JOIN
        dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
        dbo.dbUser U1 ON F.Updatedby = U1.usrID 
INNER JOIN
        dbo.dbUser U ON F.CreatedBy = U.usrID
INNER JOIN
	dbo.dbUser U2 ON F.filePrincipleID = U2.usrID'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@fileID, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	     set @where = @where + ' AND F.fileid = ''' + @fileid + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' F.fileid = ''' + @fileid + ''''
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
    ON OBJECT::[dbo].[srepFileNotes] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileNotes] TO [OMSAdminRole]
    AS [dbo];


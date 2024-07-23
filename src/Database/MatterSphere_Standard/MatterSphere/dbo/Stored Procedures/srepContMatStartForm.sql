

CREATE PROCEDURE [dbo].[srepContMatStartForm]
(@UI uUICultureInfo='{default}',
@ContractName nvarchar(50) = null,
@month datetime = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(1400)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbFileLegal.MatLAContract, dbo.dbFile.fileClosed, dbo.dbFile.Created, dbo.dbFile.fileNo, dbo.dbCodeLookup.cdDesc
FROM         dbo.dbFile INNER JOIN
                      dbo.dbContact ON dbo.dbFile.fileSourceContact = dbo.dbContact.contID INNER JOIN
                      dbo.dbCodeLookup ON dbo.dbContact.contTypeCode = dbo.dbCodeLookup.cdDesc LEFT OUTER JOIN
                      dbo.dbFileLegal ON dbo.dbFile.fileID = dbo.dbFileLegal.fileID'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@ContractName, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfilelegal.matlacontract = ''' + @ContractName + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfilelegal.matlacontract = ''' + @ContractName + ''''
	END
END

if coalesce(@Month, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND DATEPART(m, dbo.dbfile.filecreated) = ''' + @Month + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' DATEPART(m, dbo.dbfile.filecreated) = ''' + @Month + ''''
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
    ON OBJECT::[dbo].[srepContMatStartForm] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContMatStartForm] TO [OMSAdminRole]
    AS [dbo];


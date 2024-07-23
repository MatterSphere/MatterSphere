

CREATE PROCEDURE [dbo].[srepContsAssocFile]
(@UI uUICultureInfo='{default}',
@ClNo nvarchar (50) = null,
@FileNo nvarchar (50) = null,
@ClientName nvarchar(128) = null,
@FileDesc nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     
	CL.clNo, 
	F.fileNo, 
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	C.contTypeCode, 
	C.contName, 
        A.addLine1, 
	ASS.assocType, 
	ASS.assocRef, 
	ASS.assocSalut, 
        CN.contNumber
FROM         
	dbo.dbFile F
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID 
INNER JOIN
	dbo.dbContact C ON CL.clDefaultContact = C.contID 
INNER JOIN
	dbo.dbAssociates ASS ON F.fileID = ASS.fileID 
LEFT OUTER JOIN
	dbo.dbContactNumbers CN ON C.contID = CN.contID 
LEFT OUTER JOIN
	dbo.dbAddress A ON C.contDefaultAddress = A.addID'

--- Build Where Clause
SET @WHERE = ''

if coalesce(@clno, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	     set @where = @where + ' AND cl.clno = ''' + @clno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' cl.clno = ''' + @clno + ''''
	END
END

if coalesce(@fileno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND f.fileno = ''' + @fileno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' f.Fileno = ''' + @fileno + ''''
	END
END

if coalesce(@ClientName, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND cl.clname = ''' + @ClientName + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' cl.clname = ''' + @ClientName + ''''
	END
END

if coalesce(@FileDesc, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND f.filedesc = ''' + @FileDesc + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' f.filedesc = ''' + @FileDesc + ''''
	END
END

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContsAssocFile] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContsAssocFile] TO [OMSAdminRole]
    AS [dbo];


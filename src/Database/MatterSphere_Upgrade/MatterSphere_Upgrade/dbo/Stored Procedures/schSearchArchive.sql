CREATE PROCEDURE [dbo].[schSearchArchive]
(
	@UI uUICultureInfo = '{default}', 
	@CLID bigint = 0,
	@MAX_RECORDS int = 50,  
	@ARCHDESC nvarchar(100) = '',
	@ARCHREF nvarchar(50) = '',
	@ARCHTYPE uCodeLookup = null,
	@ARCHSTORAGELOC uCodeLookup = null
)  

AS

if @CLID is null set @CLID = 0

declare @Select nvarchar(1900)
declare @Top nvarchar(10)
declare @Where nvarchar(2000)

if @MAX_RECORDS > 0
	set @Top = N'TOP ' + Convert(nvarchar, @MAX_RECORDS)
else
	set @Top = N''

set @select = N'SELECT  ' + @Top + '
	A.*,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.archType, '''') + ''~'') AS archTypeDesc,
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(A.archStorageLoc, '''') + ''~'') AS archStorageLocDesc,
	CL.clNo
FROM
	dbArchive A
INNER JOIN
	dbClient CL ON A.clID = CL.clID
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''ARCHTYPE'', @ui ) CL1 ON CL1.[cdCode] = A.archType
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''LOCTYPE'', @ui ) CL2 ON CL2.[cdCode] = A.archStorageLoc'

set @where = N' WHERE A.clID = @CLID'

IF @ARCHDESC <> '' and not @ARCHDESC is null
	BEGIN 
		set @where = @where + N' AND A.archDesc LIKE @ARCHDESC + ''%'''
	END

IF @ARCHREF <> '' and not @ARCHDESC is null
	BEGIN 
		set @where = @where + N' AND A.archRef LIKE @ARCHREF + ''%'''
	END

if not @ARCHTYPE is null
	set @where = @where + N' AND A.archType = @ARCHTYPE'

if not @ARCHSTORAGELOC is null
	set @where = @where + N' AND A.archStorageLoc = @ARCHSTORAGELOC'

declare @sql nvarchar(4000)
set @sql = @select + @where + ' ORDER BY A.Created DESC'
print @sql

exec sp_executesql @sql,  N'@UI uUICultureInfo, @CLID bigint, @MAX_RECORDS int, @ARCHDESC nvarchar(100), @ARCHREF nvarchar(50), @ARCHTYPE uCodeLookup, @ARCHSTORAGELOC uCodeLookup', @UI, @CLID, @MAX_RECORDS, @ARCHDESC, @ARCHREF, @ARCHTYPE, @ARCHSTORAGELOC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchArchive] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchArchive] TO [OMSAdminRole]
    AS [dbo];


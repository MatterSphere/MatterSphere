

CREATE PROCEDURE [dbo].[schSearchArchiveAll]
(
	@UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 50
	, @ARCHDESC NVARCHAR(100) = ''
	, @ARCHREF NVARCHAR(50) = ''
	, @ARCHTYPE uCodeLookup = NULL
	, @ARCHSTORAGELOC uCodeLookup = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)  

AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)
	, @Where NVARCHAR(MAX)

SET @Select = N'
WITH Arch AS(
SELECT  
	A.*,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.archType, '''') + ''~'') AS archTypeDesc,
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(A.archStorageLoc, '''') + ''~'') AS archStorageLocDesc,
	CL.clNo,CL.CLName
FROM dbo.dbArchive A
	INNER JOIN dbo.dbClient CL ON A.clID = CL.clID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''ARCHTYPE'', @UI) CL1 ON CL1.cdCode = A.archType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''LOCTYPE'', @UI) CL2 ON CL2.cdCode = A.archStorageLoc
WHERE A.archactive = 1 
'
IF ISNULL(@ARCHDESC, '') <> ''
	SET @Select = @Select + N'
	AND a.archdesc LIKE @ARCHDESC + ''%''
'
IF ISNULL(@ARCHREF, '') <> ''
	SET @Select = @Select + N'
	AND a.archref LIKE @ARCHREF + ''%''
'
IF ISNULL(@ARCHTYPE, '') <> ''
	SET @Select = @Select + N'
	AND a.archtype = @ARCHTYPE
'
IF ISNULL(@ARCHSTORAGELOC, '') <> ''
	SET @Select = @Select + N'
	AND a.archstorageloc = @ARCHSTORAGELOC
'
SET @Select =  @Select + N'
)
'

IF @MAX_RECORDS > 0
	SET @Select =  @Select + N'
SELECT TOP (@MAX_RECORDS) *
FROM Arch
'
ELSE
	SET @Select =  @Select + N'
SELECT *
FROM Arch
'
IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY Created DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%Created%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', Created DESC'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

PRINT @Select

EXEC sp_executesql @Select,  N'@UI uUICultureInfo, @MAX_RECORDS INT, @ARCHDESC NVARCHAR(100), @ARCHREF NVARCHAR(50), @ARCHTYPE uCodeLookup, @ARCHSTORAGELOC uCodeLookup', @UI, @MAX_RECORDS, @ARCHDESC, @ARCHREF, @ARCHTYPE, @ARCHSTORAGELOC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchArchiveAll] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchArchiveAll] TO [OMSAdminRole]
    AS [dbo];


CREATE PROCEDURE dbo.SCHCOMARCHALL
(
	@UI uUICultureInfo = '{default}'
	, @ARCHDESC NVARCHAR(100) = NULL
	, @ARCHREF NVARCHAR(50) = NULL
	, @ARCHTYPE uCodeLookup = NULL
	, @ARCHSTORAGELOC uCodeLookup = NULL
	, @ARCHIVIST INT = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)  

AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @SELECT NVARCHAR(MAX)

SET @Select = N'
WITH Arch AS(
SELECT  
	A.*
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.archType, '''') + ''~'') AS archTypeDesc
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(A.archStorageLoc, '''') + ''~'') AS archStorageLocDesc
	, CL.clNo
	, CL.CLName
	, U.usrFullName
	, F.fileNo
FROM dbo.dbArchive A
	INNER JOIN dbo.dbClient CL ON A.clID = CL.clID
	INNER JOIN dbo.dbUser U ON U.usrID = A.CreatedBy
	LEFT JOIN dbo.dbFile F ON F.fileID = A.fileID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''ARCHTYPE'', @UI) CL1 ON CL1.cdCode = A.archType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''LOCTYPE'', @UI) CL2 ON CL2.cdCode = A.archStorageLoc
WHERE 1 = 1
'
IF ISNULL(@ARCHDESC, '') <> ''
	SET @SELECT = @SELECT + N'
	AND A.archdesc LIKE ''%'' + @ARCHDESC + ''%''
'
IF ISNULL(@ARCHREF, '') <> ''
	SET @SELECT = @SELECT + N'
	AND A.archref LIKE ''%'' + @ARCHREF + ''%''
'
IF ISNULL(@ARCHTYPE, '') <> ''
	SET @SELECT = @SELECT + N'
	AND A.archtype = @ARCHTYPE
'

IF ISNULL(@ARCHSTORAGELOC, '') <> ''
	SET @SELECT = @SELECT + N'
	AND A.archStorageLoc = @ARCHSTORAGELOC
'

IF @ARCHIVIST IS NOT NULL
	SET @SELECT = @SELECT + N'
	AND A.CreatedBy = @ARCHIVIST
'
SET @SELECT = @SELECT + N'
)
SELECT *
FROM Arch
'
IF @ORDERBY IS NULL
	SET @SELECT = @SELECT + N'ORDER BY Created DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%Created%'
		SET @SELECT = @SELECT + N'ORDER BY ' + @ORDERBY  + N', Created DESC'
	ELSE 
		SET @SELECT = @SELECT + N'ORDER BY ' + @ORDERBY

PRINT @Select

EXEC sp_executesql @Select,  N'@UI uUICultureInfo, @ARCHIVIST INT, @ARCHDESC NVARCHAR(100), @ARCHREF NVARCHAR(50), @ARCHTYPE uCodeLookup, @ARCHSTORAGELOC uCodeLookup', @UI, @ARCHIVIST, @ARCHDESC, @ARCHREF, @ARCHTYPE, @ARCHSTORAGELOC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCOMARCHALL] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCOMARCHALL] TO [OMSAdminRole]
    AS [dbo];

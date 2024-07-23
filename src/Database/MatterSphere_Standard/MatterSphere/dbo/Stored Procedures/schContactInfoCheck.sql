CREATE PROCEDURE dbo.schContactInfoCheck(
	@contid BIGINT
	, @addid BIGINT = NULL
	, @tel NVARCHAR(25) = NULL
	, @fax NVARCHAR(25) = NULL
	, @mob NVARCHAR(25) = NULL
	, @email NVARCHAR(255) = NULL
	, @UI uUICultureInfo = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH res AS(
SELECT 
	A.assocID
	, dbo.GetFileRef(C.clno, F.fileno) AS fileRef
	, C.clname
	, F.filedesc
	, COALESCE(CL.cdDesc, ''~'' +  NULLIF(A.assoctype, '''') + ''~'') as assoctypedesc
	, A.assocactive
FROM dbassociates A
	INNER JOIN dbFile F ON A.fileid = F.fileid
	INNER JOIN dbclient C ON F.clid = C.clid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription( ''SUBASSOC'', @ui ) CL ON CL.[cdCode] = A.assoctype
WHERE A.contid = @contid
	AND (
	A.assocdefaultaddid = @addid or
	A.assocemail = @email or
	A.assocddi = @tel or
	A.assocfax = @fax or
	A.assocmobile = @mob
	)
)
SELECT 
	assocID
	, fileRef
	, clname
	, filedesc
	, assoctypedesc
	, assocactive
FROM Res
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY assocID'
ELSE 
	IF @ORDERBY NOT LIKE '%assocID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', assocID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @Select,  N'@contid BIGINT, @addid BIGINT, @tel NVARCHAR(25), @fax NVARCHAR(25), @mob NVARCHAR(25), @email NVARCHAR(255), @UI uUICultureInfo', @contid, @addid, @tel, @fax, @mob, @email, @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schContactInfoCheck] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schContactInfoCheck] TO [OMSAdminRole]
    AS [dbo];


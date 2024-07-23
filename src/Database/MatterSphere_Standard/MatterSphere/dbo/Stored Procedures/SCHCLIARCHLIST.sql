CREATE PROCEDURE dbo.SCHCLIARCHLIST
(
	@UI uUICultureInfo = '{default}'
	, @CLID BIGINT
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
	A.archID
	, A.clID
	, A.fileID
	, A.archRef
	, A.archDesc
	, A.archInStorage
	, A.archStorageDate
	, A.archActive
	, A.archDeleted
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(A.archStorageLoc, '''') + ''~'') AS archStorageLoc
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.archType, '''') + ''~'') AS Type
	, (
		SELECT TOP 1 
			CASE WHEN trackcheckedin IS NULL THEN ''Out'' 
				WHEN trackcheckedin IS NOT NULL THEN ''In'' 
			END
		FROM dbo.dbtracking t 
		WHERE t.logID = A.archID 
		ORDER BY t.trackCheckedIn
		) AS chkinout
	, Contact.contName
	, Contact.contID
	, Contact.trackCheckedOut
	, F.fileNo
	, U.usrFullName
FROM dbo.dbArchive A
	LEFT OUTER JOIN dbo.dbTracking T on A.archID = T.logID
	LEFT OUTER JOIN dbo.dbContact C on C.contID = T.trackIssuedTo
	LEFT OUTER JOIN dbo.dbFile F on F.fileID = A.fileID
	LEFT OUTER JOIN dbo.dbUser U on U.usrID = A.CreatedBy
	OUTER APPLY
		(SELECT a1.archID
			, t.trackCheckedOut
			, c.contName
			, c.contID
		FROM dbo.dbTracking t
		INNER JOIN dbo.dbArchive a1 on a1.archID = t.logID
		LEFT OUTER JOIN dbo.dbContact c on c.contID = t.trackIssuedTo
		WHERE t.trackCheckedOut IS NOT NULL
			AND t.trackCheckedIn IS NULL 
			AND  a1.archid = A.archid
		) Contact
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''ARCHTYPE'', @UI) CL1 ON CL1.cdCode = A.archType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''LOCTYPE'', @UI) CL2 ON CL2.cdCode = A.archStorageLoc
WHERE A.clid = @CLID
)
SELECT DISTINCT *
FROM Arch
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY archID'
ELSE 
	IF @ORDERBY NOT LIKE '%archID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', archID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @Select,  N'@UI uUICultureInfo, @CLID BIGINT', @UI, @CLID 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCLIARCHLIST] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCLIARCHLIST] TO [OMSAdminRole]
    AS [dbo];
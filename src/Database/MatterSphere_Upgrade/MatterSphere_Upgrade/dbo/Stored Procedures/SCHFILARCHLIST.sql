CREATE PROCEDURE dbo.SCHFILARCHLIST
(
	@CLID BIGINT
	, @UI uUICultureInfo = '{default}'
	, @fileID BIGINT
	, @ORDERBY NVARCHAR(MAX) = NULL
)  

AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @SELECT NVARCHAR(MAX)

SET @Select = N'
WITH Arch AS(
SELECT
	A.archid
	, A.clid
	, A.fileid
	, A.archref
	, A.archdesc
	, A.archinstorage
	, A.created
	, U.usrfullname AS [archived by]
	, A.archactive
	, A.archdeleted
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(A.archStorageLoc, '''') + ''~'') AS StorageLoc
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.archType, '''') + ''~'') AS Type
	, (
		SELECT TOP 1 
			CASE 
				WHEN trackcheckedin IS NULL THEN ''Out'' 
				ELSE ''In'' 
			END
		FROM dbo.dbtracking t1 
			INNER JOIN dbo.dbarchive a1 on a1.archid = t1.logid 
		WHERE t1.logid = a.archid 
		ORDER BY t1.trackcheckedin
		) AS chkinout
	, Contact.contName
	, Contact.contID
	, Contact.trackCheckedOut
FROM dbo.dbArchive A
	LEFT OUTER JOIN dbo.dbTracking T ON T.logID = A.archID
	LEFT OUTER JOIN dbo.dbContact C ON C.contID = T.trackIssuedTo
	LEFT OUTER JOIN dbo.dbUser U ON U.usrid = A.createdby
	OUTER APPLY(
		SELECT a1.archID
		, t.trackCheckedOut
		, c.contName
		, c.contID
	FROM dbo.dbTracking t
		INNER JOIN dbo.dbArchive a1 on a1.archID = t.logID
		LEFT OUTER JOIN dbo.dbContact c on c.contID = t.trackIssuedTo
	WHERE t.trackCheckedOut IS NOT NULL
		AND t.trackCheckedIn IS NULL 
		AND a1.archid = a.archid
	) Contact
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''ARCHTYPE'', @UI) CL1 ON CL1.cdCode = A.archType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''LOCTYPE'', @UI) CL2 ON CL2.cdCode = A.archStorageLoc
WHERE A.clid = @CLID
	AND A.fileID = @fileID
)
SELECT DISTINCT *
FROM Arch
'
IF @ORDERBY IS NULL
	SET @SELECT = @SELECT + N'ORDER BY archid'
ELSE 
	IF @ORDERBY NOT LIKE '%archid%'
		SET @SELECT = @SELECT + N'ORDER BY ' + @ORDERBY  + N', archid'
	ELSE 
		SET @SELECT = @SELECT + N'ORDER BY ' + @ORDERBY

PRINT @Select

EXEC sp_executesql @Select,  N'@CLID BIGINT, @UI uUICultureInfo, @fileID BIGINT', @CLID, @UI, @fileID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILARCHLIST] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILARCHLIST] TO [OMSAdminRole]
    AS [dbo];

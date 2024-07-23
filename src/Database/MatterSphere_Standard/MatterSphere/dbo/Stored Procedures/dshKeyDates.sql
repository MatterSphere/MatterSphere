CREATE PROCEDURE dbo.dshKeyDates(@UI uUICultureInfo = '{default}', @MAX_RECORDS INT = 50, @PageNo INT = 1, @Query NVARCHAR(MAX) = NULL)
AS
SET NOCOUNT ON
SET TRAN ISOLATION LEVEL READ UNCOMMITTED

DECLARE @Res TABLE(
	ID INT IDENTITY(1, 1) PRIMARY KEY
	, kdID INT 
	, fileID BIGINT 
	, clNo NVARCHAR(12) 
	, fileNo NVARCHAR(20) 
	, kdDesc NVARCHAR(300)
	, kdType dbo.uCodeLookup
	, kdDate DATETIME
	) 


DECLARE @USERID NVARCHAR(200) = (SELECT [config].[GetUserLogin]())
	, @usrID INT 
	, @Total BIGINT
	, @OFFSET INT = 0
	, @TOP INT

IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo- 1)

SET @usrID = (SELECT usrID FROM dbo.dbUser WHERE usrADID = @USERID)

INSERT INTO @Res(kdID, fileID, clNo, fileNo, kdDesc, kdType, kdDate)
SELECT
	kd.kdID
	, f.fileID
	, cl.clNo
	, f.fileNo
	, kd.kdDesc
	, kd.kdType
	, kd.kdDate
FROM dbo.dbKeyDates kd 
	INNER JOIN dbo.dbFile f ON f.fileID = kd.fileID 
	INNER JOIN dbo.dbClient cl ON cl.clID = f.clID 
WHERE kd.kdactive = 1
	AND f.filePrincipleID = @usrID
	AND (@Query IS NULL OR kd.kdDesc LIKE @Query)
ORDER BY kd.kdDate

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	res.kdID
	, res.fileID
	, res.clNo
	, res.fileNo
	, res.kdDesc
	, COALESCE(cl.cdDesc, '~' + NULLIF(res.kdType, '') + '~') AS kdTypeDesc
	, res.kdDate
	, @Total AS Total
FROM @Res res
	LEFT OUTER JOIN dbo.GetCodeLookupDescription ( 'DATEWIZTYPES' , @UI ) cl ON cl.cdCode = res.kdType
WHERE ID > @OFFSET
ORDER BY ID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshKeyDates] TO [OMSRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshKeyDates] TO [OMSAdminRole]
    AS [dbo];

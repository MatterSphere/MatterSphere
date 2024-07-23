CREATE PROCEDURE dbo.SCHFILASSOCCOPY
(
	@UI uUICultureInfo = '{default}'
	, @CLID BIGINT 
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @SELECT NVARCHAR(MAX)

SET @SELECT = N'
WITH Res AS
(
SELECT
	F.fileType
	, F.fileID
	, F.fileFundCode
	, F.clID
	, F.fileDesc
	, F.Created
	, F.fileStatus
	, F.fileNo
	, F.fileAccCode
	, F.fileCreditLimit
	, F.filecurISOCode
	, COALESCE((SELECT SUM(timeCharge) FROM dbo.dbTimeLedger WHERE fileID = F.fileID AND timeBilled = 0), 0) AS filewip
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileFundCode, '''') + ''~'') AS FundTypeDesc
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') AS FileStatusDesc
	, F.fileNo + '' : '' + F.fileDesc AS fileJointDesc
	, F.fileDepartment
	, COALESCE(CL4.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS FileDepartmentDesc
	, F.fileManagerID
	, F.fileResponsibleID
	, F.filePrincipleID
	, fee.usrFullName AS FilePrincipleUser
	, F.fileTeam
	, (SELECT COUNT(A.assocID) FROM dbAssociates A WHERE A.fileID = F.fileID AND assocActive = 1) AS assoc_count
FROM dbo.dbFile F 
	LEFT OUTER JOIN dbo.dbUser fee ON fee.usrID = F.filePrincipleID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FUNDTYPE'', @UI) CL1 ON CL1.cdCode = F.fileFundCode
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILETYPE'', @UI) CL2 ON CL2.cdCode = F.fileType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILESTATUS'', @UI) CL3 ON CL3.cdCode = F.fileStatus
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''DEPT'', @UI) CL4 ON CL4.cdCode = F.fileDepartment
WHERE F.clID = @CLID 
	AND F.filestatus LIKE ''LIVE%'' 
)
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY Created'
ELSE 
	IF @ORDERBY NOT LIKE '%Created%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', Created'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @CLID BIGINT', 
	@UI, @CLID

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILASSOCCOPY] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILASSOCCOPY] TO [OMSAdminRole]
    AS [dbo];
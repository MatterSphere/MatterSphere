CREATE PROCEDURE dbo.SCHFILCOURTPART
(
	@FILEID BIGINT 
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
	CASE WHEN ISNULL(CI.contchristiannames, '''') <> '''' THEN CONCAT(CI.contchristiannames, '' '', CI.contsurname) ELSE CONT.contname END AS contname
	, ASSOC.assoctype
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(ASSOC.assoctype, '''') + ''~'') AS AssocDesc
FROM dbcontact CONT
	LEFT OUTER JOIN dbo.dbContactIndividual CI ON CONT.contid = CI.contid
	INNER JOIN dbAssociates ASSOC ON ASSOC.contid = CONT.contid 
	INNER JOIN (
		SELECT * 
		FROM dbo.GetCodeLookupDescription (''SUBASSOC'', NULL)
		WHERE (cdDesc LIKE ''Claimant%'' OR cdDesc LIKE ''Respondent%'' OR cdDesc LIKE ''Applicant%'' OR cdDesc LIKE ''Defendant%'') 
			AND (ISNUMERIC(RIGHT(cdDesc, 1)) = 1 OR CHARINDEX(cdDesc, '' '') = 0)
			AND LEN(cdDesc) < 15
		) CL1 ON CL1.cdCode = ASSOC.assoctype
WHERE ASSOC.fileid = @FILEID 
	AND ASSOC.assocactive = 1
)
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY contname'
ELSE 
	IF @ORDERBY NOT LIKE '%contname%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', contname'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@FILEID BIGINT', @FILEID

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILCOURTPART] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILCOURTPART] TO [OMSAdminRole]
    AS [dbo];
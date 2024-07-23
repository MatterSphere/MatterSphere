CREATE PROCEDURE dbo.SCHFEERSKMANAGE
(
	@TOTALSCORE INT = NULL
	, @RISKCODE uCodeLookup = NULL
	, @WARNINGSCORE INT = 50
	, @ORDERBY NVARCHAR(MAX) = NULL
)

AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)
SET @SELECT = N'
WITH Res AS
(
SELECT
	F.fileID
	, dbo.GetFileRefByID(F.fileid) AS OurRef
	, C.clName
	, F.fileDesc
	, RC.riskDescription
	, RH.riskCreatedBy
	, F.filePrincipleID
	, RH.riskTotalScore
	, RH.RiskCreated
	, RH.RiskActive
	, CASE WHEN RH.riskTotalScore >= @WARNINGSCORE THEN 13 ELSE 0 END AS Warning
	, U.usrFullName AS CreatedByUser
	, FE.usrFullName AS FilePrincipleUser
FROM dbo.dbRiskHeader RH
	INNER JOIN dbo.dbRiskConfig RC ON RC.riskCode = RH.riskCode
	INNER JOIN dbo.dbFile F ON F.fileID = RH.fileID
	INNER JOIN dbo.dbClient C ON C.clID = F.clID
	LEFT OUTER JOIN dbo.dbUser U ON U.usrID = RH.riskCreatedBy
	LEFT OUTER JOIN dbo.dbUser FE ON FE.usrID = F.filePrincipleID
WHERE RH.riskActive = 1
	AND RH.riskTotalScore >= COALESCE(@TOTALSCORE,0)
	'
IF(@RISKCODE IS NOT NULL)
	SET @SELECT = @SELECT + 'AND RH.riskCode = @RISKCODE
	'
SET @SELECT = @SELECT + N'
)
SELECT *
FROM Res
'
IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY riskTotalScore'
ELSE 
	IF @ORDERBY NOT LIKE '%riskTotalScore%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', riskTotalScore'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@TOTALSCORE INT, @RISKCODE uCodeLookup, @WARNINGSCORE INT', @TOTALSCORE, @RISKCODE, @WARNINGSCORE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFEERSKMANAGE] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFEERSKMANAGE] TO [OMSAdminRole]
    AS [dbo];

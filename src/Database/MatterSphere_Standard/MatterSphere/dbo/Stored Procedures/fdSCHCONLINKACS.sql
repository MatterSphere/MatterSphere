CREATE PROCEDURE dbo.fdSCHCONLINKACS
(
	@UI uUICultureInfo = '{default}'
	, @ORDERBY NVARCHAR(MAX) = NULL
)

AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

--- SET THE SELECT CLAUSE
SET @SELECT = N'
DECLARE @CL TABLE (clID BIGINT PRIMARY KEY)

INSERT INTO @CL(clID)
SELECT clID
FROM dbClient CL
WHERE CL.cltypecode = ''ENT'';

WITH F AS (
	SELECT clId
		, COUNT(*) AS NumAllFiles
		, SUM(CASE WHEN filestatus LIKE ''LIVE%'' THEN 1 ELSE 0 END) AS NumLiveFiles
	FROM dbfile
	GROUP BY clId 
)
, Res AS(
SELECT 
	CL.clID
	, CL.clNo
	, CL.clSearch1
	, CL.clSearch2
	, CL.clSearch3
	, CL.clSearch4
	, CL.clSearch5
	, CL.clName
	, ADR.addLine1
	, ADR.addLine2
	, ADR.addLine3
	, ADR.addLine4
	, ADR.addLine5
	, ADR.addPostcode
	, CONT.contName
	, U.usrInits
	, F.NumAllFiles
	, F.NumLiveFiles
	, CT.typeGlyph
	, REPLACE(REPLACE(COALESCE(ADR.addLine1, '''') + '', '' + COALESCE(ADR.addLine2, '''') + '', '' + COALESCE(ADR.addLine3, '''') + '', '' + COALESCE(ADR.addLine4,'''') + '', '' + COALESCE(ADR.addLine5,'''') + '', '' + COALESCE(ADR.addPostCode,'''') ,'', , ''  ,  '', '') ,'', , ''  ,  '', '') AS ConcatAddress
	, CL.created
FROM @CL C
	INNER JOIN config.dbClient CL ON CL.clID = C.clID
	INNER JOIN dbContact CONT ON CL.clDefaultContact = CONT.contID
	LEFT OUTER JOIN dbo.dbAddress ADR ON cont.contDefaultAddress = ADR.addID 
	INNER JOIN dbo.dbClientType CT ON CT.typeCode = CL.clTypeCode
	LEFT OUTER JOIN dbo.dbUser U on U.usrid = CL.feeusrid
	LEFT OUTER JOIN F ON F.clId = CL.clid
--WHERE CL.cltypecode = ''ENT''
)
SELECT * 
FROM Res
'
IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY clName'
ELSE 
	IF @ORDERBY NOT LIKE '%clName%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', clName'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdSCHCONLINKACS] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdSCHCONLINKACS] TO [OMSAdminRole]
    AS [dbo];


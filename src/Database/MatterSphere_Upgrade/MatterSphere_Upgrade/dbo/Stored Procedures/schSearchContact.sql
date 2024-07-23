CREATE PROCEDURE [dbo].[schSearchContact] 
(
	@UI uUICultureInfo = '{default}'
	, @MAX_RECORDS int = 50
	, @CONTNAME NVARCHAR(128) = NULL
	, @CONTTYPE uCodeLookup = NULL
	, @CONTSUBTYPE uCodeLookup = NULL
	, @ADDRESS NVARCHAR(150) = NULL
	, @EMAIL NVARCHAR(50) = NULL
	, @PageNo INT = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)

AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;
DECLARE @SELECT NVARCHAR(MAX)
	, @WHERE NVARCHAR(MAX)

-- SET THE TEXT FIELDS TO NULL IF THEY ARE ZERO LENGTH STRING
IF(@ADDRESS = '')
	SET @ADDRESS = NULL
IF(@CONTNAME = '')
	SET @CONTNAME = NULL
IF(@EMAIL = '')
	SET @EMAIL = NULL

SET @SELECT = N'
DECLARE @OFFSET INT = 0
	, @TOP INT
	, @Total INT
	
IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo- 1)

DECLARE @Res TABLE(
	Id INT IDENTITY(1, 1) PRIMARY KEY
	, contID BIGINT
);
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'
WITH Res AS
(
SELECT
	CONT.contid
FROM dbo.dbContact CONT 
'
ELSE 
BEGIN
	SET  @SELECT =  @SELECT + N'
WITH Res AS
(
SELECT
	CONT.contid
	, CONT.contname
	, CONT.contTypeCode
	, CONT.contgroup as [congroupdesc]
	, CONT.contIsClient
	, CONT.contApproved
'
	
	IF @ORDERBY LIKE '%assocCount%'
		SET  @SELECT =  @SELECT + N'
	, (SELECT (COUNT(contID)) FROM config.dbassociates WHERE contID = CONT.contID) AS assocCount
'
	IF @ORDERBY LIKE '%CONTTYPEDESC%'
		SET  @SELECT =  @SELECT + N'
	, CL_CONTTYPE.CDDESC AS [CONTTYPEDESC]
'
	IF @ORDERBY LIKE '%contDOB%'
		SET  @SELECT =  @SELECT + N'
	, CI.contDOB
'
	IF @ORDERBY LIKE '%ConcatAddress%'
		SET  @SELECT =  @SELECT + N'
	, dbo.GetAddress(coalesce(CA.contaddid, CONT.contdefaultaddress),'','',null) as ConcatAddress
'

	IF @ORDERBY LIKE '%typeGlyph%'
		SET  @SELECT =  @SELECT + N'
	, CT.typeGlyph
'

	SET  @SELECT =  @SELECT + N'
FROM dbContact CONT
'
	
	IF @ORDERBY LIKE '%typeGlyph%'
		SET  @SELECT =  @SELECT + N'
	INNER JOIN DBO.dbContactType CT on CT.typeCode = CONT.contTypeCode
	'

	IF @ORDERBY LIKE '%contDOB%'
		SET  @SELECT =  @SELECT + N'
	LEFT OUTER JOIN dbo.dbContactIndividual CI ON CI.contID = CONT.contID 
'

	IF @ORDERBY LIKE '%CONTTYPEDESC%'
		SET  @SELECT =  @SELECT + N'
	LEFT OUTER JOIN DBO.GETCODELOOKUPDESCRIPTION ( ''CONTTYPE'' , @UI ) CL_CONTTYPE ON CONT.CONTTYPECODE = CL_CONTTYPE.CDCODE
'
	IF @ORDERBY LIKE '%ConcatAddress%'
		SET  @SELECT =  @SELECT + N'
	OUTER APPLY (SELECT TOP 1 contaddid FROM dbo.dbcontactaddresses ca WHERE ca.contid = CONT.contid AND ca.contactive = 1 AND ca.contcode = ''PRINCIPLE'') CA 
'


END

-- SET THE WHERE CLAUSE
SET @WHERE = 'WHERE 1 = 1 '

-- CONTACT NAME CLAUSE
IF(@CONTNAME IS NOT NULL OR @CONTNAME <> '')
BEGIN
	SET @WHERE = @WHERE + 'AND CONT.contName LIKE ''%'' + @CONTNAME + ''%'' '
END

--- CONTACT TYPE CLAUSE
IF(@CONTTYPE <> '' OR @CONTTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND CONT.contTypeCode = @CONTTYPE '
END

-- SUBCONTACT TYPE CLAUSE
IF(@CONTSUBTYPE <> '' OR @CONTSUBTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND CONT.contAddFilter = @CONTSUBTYPE '
END

-- CONTACT ADDRESS CLAUSE
IF(@ADDRESS IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND CONT.contID IN (
		   SELECT DISTINCT contID
			FROM dbContactAddresses ca
			INNER JOIN dbAddress a ON ca.contaddID = a.addID
			WHERE
		   (a.addLine1 LIKE ''%'' + @ADDRESS + ''%'') OR 
		   (a.addLine2 LIKE ''%'' + @ADDRESS + ''%'') OR
		   (a.addLine3 LIKE ''%'' + @ADDRESS + ''%'') OR
		   (a.addLine4 LIKE ''%'' + @ADDRESS + ''%'') OR
		   (a.addLine5 LIKE ''%'' + @ADDRESS + ''%'') OR
		   (a.addPostCode LIKE ''%'' + @ADDRESS + ''%'') ) '
END

-- CONTACT EMAIL CLAUSE
IF(@EMAIL IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND CONT.contID IN (SELECT contID FROM dbContactEmails WHERE contEmail LIKE ''%'' + @EMAIL + ''%'') '
END

--- ADD THE CLAUSES TOGETHER
SET @SELECT = @SELECT + @WHERE + N'
) 
INSERT INTO @Res (CONT.contID)
SELECT contID
FROM Res
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY contid'
ELSE 
	IF @ORDERBY NOT LIKE '%contid%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', contid DESC'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

SET @SELECT = @SELECT +  N'

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	CONT.contid,
	CONT.contname,
	CONT.contTypeCode,
	(SELECT (COUNT(contID)) FROM config.dbassociates WHERE contID = CONT.contID) AS assocCount,
	CL_CONTTYPE.CDDESC AS [CONTTYPEDESC],
	CONT.contgroup as [congroupdesc],
	CONT.contIsClient,
	CONT.contApproved,
	CI.contDOB,
	@Total as Total,
	dbo.GetAddress(coalesce(CA.contaddid, CONT.contdefaultaddress),'','',null) as ConcatAddress, 
	CT.typeGlyph
FROM @Res res
	INNER JOIN config.dbContact CONT on res.contID = CONT.contID
	INNER JOIN dbo.dbContactType CT on CT.typeCode = CONT.contTypeCode
	LEFT OUTER JOIN dbo.dbContactIndividual CI ON CI.contID = CONT.contID 
	LEFT OUTER JOIN DBO.GETCODELOOKUPDESCRIPTION ( ''CONTTYPE'' , @UI ) CL_CONTTYPE ON CONT.CONTTYPECODE = CL_CONTTYPE.CDCODE
	OUTER APPLY (SELECT TOP 1 contaddid FROM dbo.dbcontactaddresses ca WHERE ca.contid = CONT.contid AND ca.contactive = 1 AND ca.contcode = ''PRINCIPLE'') CA 
WHERE res.Id > @OFFSET 
ORDER BY res.Id 
'

--- DEBUG PRINT
PRINT @SELECT

EXEC sp_executesql @SELECT, 
N'
	@UI uUICultureInfo
	, @CONTNAME nvarchar(128)
	, @ADDRESS nvarchar(150)
	, @CONTTYPE uCodeLookup
	, @CONTSUBTYPE uCodeLookup
	, @EMAIL nvarchar(50)
	, @MAX_RECORDS INT
	, @PageNo INT'

	, @UI
	, @CONTNAME
	, @ADDRESS
	, @CONTTYPE
	, @CONTSUBTYPE
	, @EMAIL
	, @MAX_RECORDS
	, @PageNo

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchContact] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchContact] TO [OMSAdminRole]
    AS [dbo];


CREATE PROCEDURE dbo.fdsprContactLinks_Main
(	
	@CONTID AS BIGINT
	, @UI uUICultureInfo
	, @CONTLINKCODE AS NVARCHAR(20)
	, @CREATEDBY AS BIGINT
	, @MASTERONLY AS BIT
	, @ORDERBY NVARCHAR(MAX) = NULL
)
AS
DECLARE @SELECT NVARCHAR(MAX)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
WITH Res AS(
SELECT 
	CONT.CONTID
	, CONT.CONTTYPECODE
	, COALESCE(CL.cdDesc, ''~'' +  NULLIF(CONT.contTypeCode, '''') + ''~'') AS contactType
	, LINK.contLinkCode
	, COALESCE(CL1.cdDesc, ''~'' +  NULLIF(LINK.contLinkCode, '''') + ''~'') as LinkDesc
	, CONT.contName
	, LINK.Created
	, U.usrFullName
	, MC.Links
	, CASE MC.[isMaster] WHEN 1 THEN 70 ELSE 67 END AS Image
	, LINK.contLinkID
	, LINK.contID as [parentID]
	, CASE WHEN MC.[isMaster] IS NULL THEN 0 ELSE MC.[isMaster]	END AS isMaster
	, dbo.GetAddress(coalesce((SELECT TOP 1 contaddid FROM dbo.dbcontactaddresses WHERE contid = CONT.contid AND contactive = 1 and contcode = ''PRINCIPLE''), contdefaultaddress),'','',NULL) AS ConcatAddress
FROM dbContact CONT
	INNER JOIN dbo.dbContactLinks LINK ON CONT.contID = LINK.contLINKID
	INNER JOIN dbUser U ON U.usrID = LINK.CreatedBy
	OUTER APPLY	(
		SELECT
			contid
			, SUM(CASE WHEN contlinkcode = ''MASTER'' THEN 1 END) AS [isMaster]
			, SUM(CASE WHEN  CL.contlinkid != CL.CONTID THEN 1 END) AS [Links]
		FROM dbo.dbcontactlinks CL
		WHERE CL.contId = CONT.contId 
		GROUP BY contid
		)  MC
	LEFT JOIN dbo.GetCodeLookupDescription ( ''CONTTYPE'', @UI ) CL ON CL.[cdCode] = CONT.contTypeCode
	LEFT JOIN dbo.GetCodeLookupDescription ( ''CONTLINK'', @UI ) CL1 ON CL1.[cdCode] =  LINK.contLinkCode
WHERE LINK.contID = @CONTID AND LINK.contID != LINK.contLinkID
'

IF (@CONTLINKCODE IS NOT NULL)
	SET @SELECT = @SELECT + N'	AND LINK.CONTLINKCODE = @CONTLINKCODE
'

IF (@CREATEDBY IS NOT NULL)
	SET @SELECT = @SELECT + N'	AND LINK.CreatedBy = @CREATEDBY
'

IF (@MASTERONLY IS NOT NULL AND @MASTERONLY = 1)
	SET @SELECT = @SELECT + N'	AND MC.isMaster = 1
'

SET @SELECT = @SELECT + N'
)
SELECT * FROM Res
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY Created'
ELSE 
	IF @ORDERBY NOT LIKE '%Created%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', Created'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, 
N'
	@CONTID AS BIGINT
	,@UI AS uUICultureInfo 
	,@CONTLINKCODE AS NVARCHAR(20)
	,@CREATEDBY AS BIGINT
	,@MASTERONLY AS bit'
	,@CONTID
	,@UI 
	,@CONTLINKCODE
	,@CREATEDBY
	,@MASTERONLY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_Main] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_Main] TO [OMSAdminRole]
    AS [dbo];

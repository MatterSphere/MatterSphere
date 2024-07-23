CREATE PROCEDURE dbo.fdsprContactLinks_List
(	
	@MASTERID AS BIGINT
	, @UI AS uUICultureInfo 
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
		CONT.CONTID AS [contID]
		, LINK.CONTLINKID AS [contLinkID]
		, LINK.CONTID AS [masterID]
		, CONT.CONTNAME AS [Name]
		, LINK.Created AS [Created]
		, U.usrFullName AS [CreatedBy]
		, LINK.contLinkCode AS [contLinkCode]
		, MC.Links
		, COALESCE(CL.cdDesc, ''~'' +  NULLIF(LINK.contLinkCode, '''') + ''~'') AS LinkDesc
		, ISNULL(MC.isMaster, 0) AS isMaster
		, CASE MC.isMaster WHEN 1 THEN 70 ELSE 67 END AS Image
		, LINK.CONTID as [parentID]
	FROM dbContact CONT
		INNER JOIN dbo.dbContactLinks LINK ON CONT.contID = LINK.contLINKID
		INNER JOIN dbo.dbUser U ON U.usrID = LINK.CreatedBy
		LEFT JOIN dbo.GetCodeLookupDescription (''CONTLINK'', @UI ) CL ON CL.cdCode = LINK.contLinkCode
		OUTER APPLY	(
			SELECT
				contid
				, SUM(CASE WHEN contlinkcode = ''MASTER'' THEN 1 END) AS isMaster
				, SUM(CASE WHEN  CL.contlinkid != CL.CONTID THEN 1 END) AS Links
			FROM dbo.dbcontactlinks CL
			WHERE CL.contId = CONT.contId 
			GROUP BY contid
		)  MC
	WHERE LINK.contID = @MASTERID
		AND	LINK.contID != LINK.contLinkID
)
SELECT * 
FROM Res
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY CONTID'
ELSE 
	IF @ORDERBY NOT LIKE '%CONTID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', CONTID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@MASTERID AS BIGINT, @UI AS uUICultureInfo', @MASTERID, @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_List] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_List] TO [OMSAdminRole]
    AS [dbo];


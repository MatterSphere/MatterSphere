CREATE PROCEDURE [dbo].[sprAssocList] 
(
	@FILEID BIGINT
	, @ADDRESSEE uCodeLookup = NULL
	, @ASSOCTYPE uCodeLookup = NULL
	, @UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 0
	, @PageNo INT = NULL
	, @ACTIVE BIT = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

-- Get Associate List including Address Type
IF @ADDRESSEE = '' OR @ADDRESSEE = '%' OR @ADDRESSEE = '*'
	SET @ADDRESSEE = null
 
IF @ASSOCTYPE = '' OR @ASSOCTYPE = '%' OR @ASSOCTYPE = '*'
	SET @ASSOCTYPE = null

DECLARE @SELECT NVARCHAR(MAX)

SET @SELECT = N'
DECLARE @OFFSET INT = 0
	, @TOP INT
	, @Total INT

DECLARE @AddressTypes NVARCHAR(1000) = dbo.GetCodeLookupDesc(''RESOURCE'', ''DEFAULT'', @UI);
	
IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo- 1)

DECLARE @Res TABLE(
	Id BIGINT PRIMARY KEY
	, assocID BIGINT
	, fileID BIGINT
	, conttypecode uCodeLookup
	, assoctype uCodeLookup
);

WITH Res AS
(
SELECT     
	A.*
	, C.contName AS ContName
	, C.conttypecode as conttypecode
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.assocType, '''') + ''~'') AS assoctypedesc
	, CASE WHEN CA.AddressTypes IS NULL THEN @AddressTypes ELSE CA.AddressTypes END AS addtypedesc
	, COALESCE(A.AssocAddressee, C.ContName) AS AssociateName
	, PolicyCodes.cdDesc AS SecurityPolicy
FROM dbo.dbAssociates A 
	INNER JOIN dbo.dbContact C ON C.contID = A.contID 
	LEFT JOIN
	(Select ContID, ContAddID, dbo.GetAssocAddressTypes(ContAddID, ContID, @UI) as AddressTypes 
		from dbContactAddresses GROUP BY contID, contaddID) as CA ON A.assocdefaultaddID = CA.contaddID and CA.contid = A.contid
	LEFT OUTER JOIN dbUser U on C.UserID = U.usrID 
	LEFT OUTER JOIN relationship.UserGroup_File R ON U.SecurityID = R.UserGroupID AND (R.fileID = @fileID OR R.fileID IS NULL)
	LEFT OUTER JOIN config.ObjectPolicy P ON P.ID = R.PolicyID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''POLICY'', null) PolicyCodes on PolicyCodes.cdCode = P.Type
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''SUBASSOC'', @UI) CL1 ON CL1.cdCode = A.assocType
WHERE A.fileID = @FILEID
	'

IF @ACTIVE IS NOT NULL
	SET @SELECT = @SELECT + N'AND A.assocActive = @ACTIVE
	'

IF @ADDRESSEE IS NOT NULL
	SET @SELECT = @SELECT + N'AND C.conttypecode = @ADDRESSEE
	'

IF @ASSOCTYPE IS NOT NULL
	SET @SELECT = @SELECT + N'AND A.assoctype = @ASSOCTYPE
	'

SET @SELECT = @SELECT + N'
)
INSERT INTO @Res (assocID, fileID, conttypecode, assoctype, Id)
SELECT assocID
	, fileID
	, conttypecode
	, assoctype 
	'


IF @ORDERBY IS NULL
	SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY assocOrder)'
ELSE 
	IF @ORDERBY NOT LIKE '%assocOrder%'
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N', assocOrder)'
	ELSE 
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N')'

SET @SELECT = @SELECT + N'
FROM Res
SET @Total = @@ROWCOUNT

SELECT     
	A.*, C.contName AS ContName, res.conttypecode as conttypecode, COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.assocType, '''') + ''~'') AS assoctypedesc, 
	(case when AddressTypes is null then @AddressTypes else AddressTypes  end) AS addtypedesc,
	Coalesce(AssocAddressee, ContName) as AssociateName	, PolicyCodes.cdDesc AS SecurityPolicy, @Total as Total
FROM         
	@RES res
	INNER JOIN dbo.dbAssociates A ON res.assocID = A.assocID
	INNER JOIN dbo.dbContact C ON C.contID = A.contID 
	LEFT JOIN
	(Select ContID, ContAddID, dbo.GetAssocAddressTypes(ContAddID, ContID, @UI) as AddressTypes 
		from dbContactAddresses GROUP BY contID, contaddID) as CA ON A.assocdefaultaddID = CA.contaddID and CA.contid = A.contid
	LEFT JOIN dbUser U on C.UserID = U.usrID 
	LEFT JOIN [relationship].[UserGroup_File] R on U.SecurityID = R.UserGroupID and (R.fileID = @fileID or R.fileID is null)
	LEFT JOIN [config].[ObjectPolicy] P on P.ID = R.PolicyID
	LEFT JOIN dbo.GetCodeLookupDescription(''POLICY'', null) PolicyCodes on PolicyCodes.cdCode = P.[Type]   
	LEFT JOIN dbo.GetCodeLookupDescription ( ''SUBASSOC'', @UI ) CL1 ON CL1.[cdCode] = A.assocType                  
WHERE res.Id > @OFFSET 
ORDER BY res.Id
'

PRINT @SELECT

EXEC sp_executesql @SELECT,  N'	@FILEID BIGINT, @ADDRESSEE uCodeLookup, @ASSOCTYPE uCodeLookup, @UI uUICultureInfo, @MAX_RECORDS INT, @PageNo INT, @ACTIVE BIT',
	@FILEID, @ADDRESSEE, @ASSOCTYPE, @UI, @MAX_RECORDS, @PageNo, @ACTIVE

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAssocList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAssocList] TO [OMSAdminRole]
    AS [dbo];


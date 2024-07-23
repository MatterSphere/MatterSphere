



CREATE PROCEDURE [dbo].[sprAssocListForDoc] (@FILEID bigint = null, @DOCID bigint = null,  @ADDRESSEE uCodeLookup = null, @ASSOCTYPE uCodeLookup = null, @UI uUICultureInfo = '{default}')  AS
-- Get Associate List including Address Type
if @FILEID  is NULL
	set @FILEID = (SELECT top(1) fileID FROM dbDocument where docID = @DOCID)
	
if @ADDRESSEE = '' or @ADDRESSEE = '%' or @ADDRESSEE = '*'
 set @ADDRESSEE = null
 
if @ASSOCTYPE = '' or @ASSOCTYPE = '%' or @ASSOCTYPE = '*'
 set @ASSOCTYPE = null

DECLARE @AddressTypes nvarchar(1000) = dbo.GetCodeLookupDesc('RESOURCE', 'DEFAULT', @UI);
 
SELECT     
			A.*, C.contName AS ContName, C.ContTypeCode as ContTypeCode, COALESCE(CL1.cdDesc, '~' + NULLIF(A.assocType, '') + '~') AS assoctypedesc, 
			(case when AddressTypes is null then @AddressTypes else AddressTypes  end) AS addtypedesc,
			Coalesce(AssocAddressee, ContName) as AssociateName	, PolicyCodes.cdDesc AS SecurityPolicy
FROM         
			dbo.dbAssociates A INNER JOIN
			dbo.dbContact C ON C.contID = A.contID LEFT JOIN
			(Select ContID, ContAddID, dbo.GetAssocAddressTypes(ContAddID, ContID, @UI) as AddressTypes from dbContactAddresses GROUP BY contID, contaddID) as CA ON A.assocdefaultaddID = CA.contaddID and CA.contid = A.contid
			LEFT JOIN dbUser U on C.UserID = U.usrID 
			LEFT JOIN [relationship].[UserGroup_Document] R on U.SecurityID = R.UserGroupID and (R.DocumentID =@DocID or R.DocumentID is null)
			LEFT JOIN [config].[ObjectPolicy] P on P.ID = R.PolicyID
			LEFT JOIN dbo.GetCodeLookupDescription('POLICY',null) PolicyCodes on PolicyCodes.cdCode = P.[Type]                      
			LEFT JOIN dbo.GetCodeLookupDescription ( 'SUBASSOC', @UI ) CL1 ON CL1.[cdCode] =  A.assocType

WHERE		A.fileID = @FILEID and conttypecode = coalesce(@ADDRESSEE, conttypecode) and assoctype = coalesce(@ASSOCTYPE, assoctype) 
			
			
ORDER BY 
			A.assocOrder
			



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAssocListForDoc] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAssocListForDoc] TO [OMSAdminRole]
    AS [dbo];


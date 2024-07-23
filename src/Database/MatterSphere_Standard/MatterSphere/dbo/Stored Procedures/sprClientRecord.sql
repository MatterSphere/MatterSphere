

CREATE PROCEDURE [dbo].[sprClientRecord]
	@CLID [bigint],
	@UI [uUICultureInfo] = '{default}'
AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @defcontid bigint , @defaddid bigint , @cltypecode uCodeLookup , @defContAddID bigint
SELECT @defcontid = clDefaultContact , @cltypecode = cltypecode , @defaddid = clDefaultAddress FROM [dbo].[dbclient] WHERE CLID = @CLID
SET @defContAddID = ( SELECT contDefaultAddress FROM [dbo].[dbContact] WHERE contid = @defContID )

-- FILES
--Get Quick File List
SELECT  
	  fileID
	  , fileNo
	  , filedesc
	  , fileStatus
	  , F.fileno + ' : ' + F.filedesc as [fileJointDesc]
	  , F.Created
	  , F.filetype
FROM         
	[dbo].[dbFile] F 
WHERE     F.clID = @CLID
ORDER BY F.Created Desc

-- DEFAULTCONTACT
--Contact Header Information
SELECT * FROM [dbo].[dbContact] WHERE contid = @defContID
	

-- DEFAULTCONTACTADDRESS
--Get Contact Addresses
SELECT * FROM [dbo].[dbAddress] WHERE addid = Coalesce(@defaddid,@defContAddID)
	

-- DEFAULTCONTACTNUMBERS
--Get Number Records
SELECT *,
	Y.cdDesc as NumberTypeDesc,
	Z.cdDesc as NumberLocDesc 
FROM  
	[dbo].[dbContactNumbers]  CN
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'NUMBERTYPE' , @UI ) Y ON Y.[cdCode] = CN.[contCode] 
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'INFOTYPE' , @UI ) Z ON Z.[cdCode] = CN.[contExtraCode] 
WHERE 
	contid = @defContID
ORDER BY 
	ContOrder


-- DEFAULTCONTACTEMAILS
--Get Email Addresses
SELECT *,
	Z.cdDesc as EmailTypeDesc 
FROM  
	[dbo].[dbContactEmails]  CE 
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'INFOTYPE' , @UI ) Z ON Z.[cdCode] = CE.[contCode] 
WHERE
	contid = @defContID
ORDER BY 
	ContOrder

-- CONTACTS
--Get Contacts
SELECT *, 
	Y.cdDesc as ContactTypeDesc ,
	Z.cdDesc as ContactRelationDesc
FROM  
	[dbo].[dbClientContacts]
JOIN
	[dbo].[dbContact] ON dbClientContacts.contID = dbContact.contID
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'CONTTYPE' , @UI ) Y ON Y.[cdCode] = dbContact.[contTypeCode] 
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'CONTRELATION' , @UI ) Z ON Z.[cdCode] = dbClientContacts.[clRelation] 
	WHERE CLID = @CLID


--FILEPHASES
SELECT * FROM [dbo].[dbFilePhase] FP JOIN [dbo].[dbFile] F ON F.fileID = FP.fileID 
WHERE F.clID = @clID AND  FP.phactive = 1 
ORDER BY FP.fileid, FP.created

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientRecord] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientRecord] TO [OMSAdminRole]
    AS [dbo];


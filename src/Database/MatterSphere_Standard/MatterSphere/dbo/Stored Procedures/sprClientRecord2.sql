﻿CREATE PROCEDURE [dbo].[sprClientRecord2]
		@CLID [bigint],
	@UI [uUICultureInfo] = '{default}'
AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @defcontid bigint ,
		@defaddid bigint , @cltypecode uCodeLookup , @defContAddID bigint
SELECT @defcontid = clDefaultContact , @cltypecode = cltypecode , @defaddid = clDefaultAddress FROM [dbo].[dbclient] WHERE CLID = @CLID
SET @defContAddID = ( SELECT contDefaultAddress FROM [dbo].[dbContact] WHERE contid = @defContID )

-- FILES - Separate call in 2
--Get Quick File List
--exec dbo.sprClientRecord_Files @CLID, @UI

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
	-- Performance May 2017 - Change to identify default schema
if exists (select top 1 1 from sys.synonyms where [schema_id] =schema_id('dbo') and base_object_name like '_config_.%')
begin
	SELECT fp.phNo,fp.phActive,fp.phDesc,f.* into #T1 FROM [dbo].[dbFilePhase] FP 
	JOIN [config].[dbFile] F ON F.fileID = FP.fileID 
	WHERE F.clID = @CLID
	AND  FP.phactive = 1 

	if exists (select top 1 1 from #T1)
		select temp.* from #T1 temp join [dbo].[dbfile] f on temp.fileID = F.fileID
		order by temp.fileid,temp.Created
	else 
		select * from #T1
	end
else
begin
	SELECT fp.phNo,fp.phActive,fp.phDesc,f.* FROM [dbo].[dbFilePhase] FP 
	JOIN [dbo].[dbFile] F ON F.fileID = FP.fileID 
	WHERE F.clID = @CLID
	AND  FP.phactive = 1 
end

SET ANSI_NULLS ON

GO
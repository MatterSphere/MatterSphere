/*
	Login details, default associate types are stored here in associate data. 
*/
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTACRGLBUSRDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTACRGLBUSRDETS' , 1, '', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTACRPSSWRDDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTACRPSSWRDDETS' , 1, '', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTACRUSRNMEDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTACRUSRNMEDETS' , 1, '', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTADEFENDDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTADEFENDDETS' , 1, 'DEFSOL', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTADRIVERDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTADRIVERDETS' , 1, '3RDDRIV', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTAINSURERDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTAINSURERDETS' , 1, '3RDINSCO', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTAPOLICEDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTAPOLICEDETS' , 1, 'POL', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTAMIBCONTDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTAMIBCONTDETS' , 1, '', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTAPORTALDETS') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTAPORTALDETS' , 1, '', (newid()) )
IF NOT EXISTS ( SELECT * FROM [dbo].[dbSpecificData] WHERE spLookup = 'RTAXMLADDCLAIM') INSERT dbo.dbSpecificData ([spLookup], [brID], [spData], [rowguid]) VALUES ('RTAXMLADDCLAIM' , 1, '', (newid()) )

GO
-- Insert an 'Unknown' address in order to allow a Defendant to be created 
-- (as an address is required to create a contact in MatterCentre.)
IF NOT EXISTS (SELECT 1 FROM DBADDRESS WHERE addLine1 = 'Unknown'  AND addLine2 = 'Unknown' AND addLine3 = 'Unknown'  AND addLine4 = 'Unknown' AND addLine5 = 'Unknown' AND addPostcode = 'Unknown' AND addCountry = 223 AND addExtTxtID = 'RTAUNKNOWNADDRESS')
INSERT INTO DBADDRESS 
(
	addLine1
	, addLine2
	, addLine3
	,addLine4
	, addLine5
	, addPostcode
	, addCountry
	, addDXCode
	, Created
	, CreatedBy
	, Updated
	, UpdatedBy
	, addExtTxtID
)
VALUES
(
	'Unknown'
	, 'Unknown'
	, 'Unknown'
	, 'Unknown'
	, 'Unknown'
	, 'Unknown'
	, 223
	, NULL
	, GETDATE()
	, -1
	, GETDATE()
	, -1
	, 'RTAUNKNOWNADDRESS'
)	

-- Location where xml files are to be saved as they're submitted to the portal
-- e.g. C:\Users\Daniel.i\Desktop\RTA Xml Files\
if not exists (select * from dbSpecificData where spLookup = 'RTAXMLFILELOC')
begin
	insert into dbSpecificData 
	(
		spLookup
		, brID
		, spData
		, rowguid
	)
	values
	(
		'RTAXMLFILELOC'
		, 1
		, NULL
		, (newid())
	)
end


-- Location where xml file is saved, containing data of the claim to date
-- e.g. C:\Users\Daniel.i\Desktop\RTA_GetClaim_XmlFile.xml (note this code needs the file name also)
if not exists (select * from dbSpecificData where spLookup = 'RTAXMLGETCLAIM')
begin
	insert into dbSpecificData 
	(
		spLookup
		, brID
		, spData
		, rowguid
	)
	values
	(
		'RTAXMLGETCLAIM'
		, 1
		, NULL
		, (newid())
	)
end
 ----------------- 2

 if exists (select * from dbCodeLookup where cdType = 'RTACLMTLOSSES' and cdCode = '11')
begin
	update dbCodeLookup
	set cdDesc = 'General Damages (PSLA)' 
	where cdType = 'RTACLMTLOSSES' and cdCode = '11'
end	


if not exists (select * from dbCodeLookup where cdType = 'RTACLMTLOSSES' and cdCode = '13')
begin
	 insert into dbCodeLookup
	 (
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	 )
	 values
	 (
		'RTACLMTLOSSES'	 
		, '13'
		, '{default}'
		, 'Disavantage on the labour market'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0 
	 )
end 
 
 if not exists (select * from dbCodeLookup where cdType = 'RTACLMTLOSSES' and cdCode = '14')
begin
	 insert into dbCodeLookup
	 (
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	 )
	 values
	 (
		'RTACLMTLOSSES'	 
		, '14'
		, '{default}'
		, 'Loss of congenial employment'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0 
	 )
end 


if not exists (select * from dbCodeLookup where cdType = 'RTACLMTLOSSES' and cdCode = '15')
begin
	 insert into dbCodeLookup
	 (
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	 )
	 values
	 (
		'RTACLMTLOSSES'	 
		, '15'
		, '{default}'
		, 'Future losses'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0 
	 )
end 

------4

if not exists (select * from dbCodeLookup where cdtype = 'USRROLES' and cdCode = 'fdRPDAUTHORISED')
begin
	insert into dbCodeLookup
	(
		cdType
		,cdCode
		,cdUICultureInfo
		,cdDesc
		,cdSystem
		,cdDeletable
		,cdAddLink
		,cdHelp
		,cdNotes
		,cdGroup
	)
	values
	(
		'USRROLES'
		, 'fdRPDAUTHORISED'
		, '{default}'
		, 'fd RaPId - Authorised Role'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists(select * from dbCodeLookup where cdtype = 'USRROLES' and cdCode = 'fdRTACLONE')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'USRROLES'
		, 'fdRTACLONE'
		, '{default}'
		, 'fd RaPId - Cloning Visibility Role'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end
---- 5


IF NOT EXISTS(SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT'	AND CDCODE = 'RTAINTERIM')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'RTAINTERIM'
		, '{default}'
		, 'RaPId - Interim Pack Submitted to RTA Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END
ELSE
BEGIN
	PRINT '"RTAINTERIM" ALREADY EXISTS AS A FILEEVENT CODE. THIS ITEM HAS NOT BEEN RE-ADDED.'
END


IF NOT EXISTS (SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT' AND CDCODE = 'RTASETTLEMENT')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'RTASETTLEMENT'
		, '{default}'
		, 'RaPId - Settlement Pack Submitted to RTA Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END
ELSE
BEGIN
	PRINT '"RTASETTLEMENT" ALREADY EXISTS AS A FILEEVENT CODE. THIS ITEM HAS NOT BEEN RE-ADDED.'
END


IF NOT EXISTS (SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT' AND CDCODE = 'RTACOUNTEROFFER')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'RTACOUNTEROFFER'
		, '{default}'
		, 'RaPId - Settlement Pack Counter Offer Submitted to RTA Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END
ELSE
BEGIN 
	PRINT '"RTACOUNTEROFFER" ALREADY EXISTS AS A FILEEVENT CODE. THIS ITEM HAS NOT BEEN RE-ADDED.'
END




IF NOT EXISTS (SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT' AND CDCODE = 'RTADEFRESPONSE')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'RTADEFRESPONSE'
		, '{default}'
		, 'RaPId - Defendant Insurer''s Response Data Added by User'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END
ELSE
BEGIN 
	PRINT '"RTADEFRESPONSE" ALREADY EXISTS AS A FILEEVENT CODE. THIS ITEM HAS NOT BEEN RE-ADDED.'
END


IF NOT EXISTS(SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT'	AND CDCODE = 'RTAADDDAMAGES')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'RTAADDDAMAGES'
		, '{default}'
		, 'RaPId - Settlement Pack Additional Damages submitted to RTA portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END


if not exists (select * from dbCodeLookup where cdType = 'FILEEVENT' and cdCode = 'RTAERROR')
begin
	insert into dbCodeLookup
	(	
		cdType
		,cdCode
		,cdUICultureInfo
		,cdDesc
		,cdSystem
		,cdDeletable
		,cdAddLink
		,cdHelp
		,cdNotes
		,cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAERROR'
		, '{default}'
		, 'RaPId - Error Generated Upon Submission'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end





if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTASUBMITCLAIM')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTASUBMITCLAIM'
		, '{default}'
		, 'RaPId - Claim Submitted to RTA Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTARESUBMIT')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTARESUBMIT'
		, '{default}'
		, 'RaPId - Claim Resubmitted to RTA Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKDENLIAB')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKDENLIAB'
		, '{default}'
		, 'RaPId - Acknowledged Denied Liability'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKLIABCHILD')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKLIABCHILD'
		, '{default}'
		, 'RaPId - Acknowledged Liability Admitted for Child'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKLIABNEG')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKLIABNEG'
		, '{default}'
		, 'RaPId - Acknowledged Liability Admitted with Negligence'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKLIABTO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKLIABTO'
		, '{default}'
		, 'RaPId - Acknowledged Liability Decision Timeout'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTASTG1PAYMENT')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTASTG1PAYMENT'
		, '{default}'
		, 'RaPId - Confirmation of Initial Stage 1 Payment'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKLIABADMT')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKLIABADMT'
		, '{default}'
		, 'RaPId - Acknowledged Liability Admitted'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAEXITPROCESS')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAEXITPROCESS'
		, '{default}'
		, 'RaPId - Requested Early Exit from Claim'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKEXITPRO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKEXITPRO'
		, '{default}'
		, 'RaPId - Acknowledged Early Exit Request'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAINTERIMREQ')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAINTERIMREQ'
		, '{default}'
		, 'RaPId - Interim Settlement Pack Required'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAINTPAYREC')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAINTPAYREC'
		, '{default}'
		, 'RaPId - Interim Settlement Pack Payment Received'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAINTPARTIAL')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAINTPARTIAL'
		, '{default}'
		, 'RaPId - Acceptance of Partial Interim Settlement Pack Payment'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKINTPAYTO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKINTPAYTO'
		, '{default}'
		, 'RaPId - Acknowledged Interim Payment Decision Timeout'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKINTREJ')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKINTREJ'
		, '{default}'
		, 'RaPId - Acknowledged Rejected Interim Settlement Pack'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTARETURNSTG21')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTARETURNSTG21'
		, '{default}'
		, 'RaPId - Return to Start of Stage 2.1'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKSPCONFIRM')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKSPCONFIRM'
		, '{default}'
		, 'RaPId - Acknowledged Settlement Pack Confirmation'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKSPREPUD')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKSPREPUD'
		, '{default}'
		, 'RaPId - Acknowledged Settlement Pack Repudiation'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKSPNOTAGRD')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKSPNOTAGRD'
		, '{default}'
		, 'RaPId - Acknowledged Settlement Pack Not Agreed'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKSPAGRD')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKSPAGRD'
		, '{default}'
		, 'RaPId - Acknowledged Settlement Pack Agreed'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTACONEEDED')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTACONEEDED'
		, '{default}'
		, 'RaPId - Counter Offer Needed'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTASPDECISION')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTASPDECISION'
		, '{default}'
		, 'RaPId - Settlement Pack Agreement Decision'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAADEXISTENCE')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAADEXISTENCE'
		, '{default}'
		, 'RaPId - Existence of Additional Damages'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAADDECISION')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAADDECISION'
		, '{default}'
		, 'RaPId - Decision on Additional Damages'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAADACKALLDMGS')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAADACKALLDMGS'
		, '{default}'
		, 'RaPId - Acknowledged All Additional Damages Agreed'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAACKADDMGS')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAACKADDMGS'
		, '{default}'
		, 'RaPId - Acknowledged Additional Damages Agreed'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'RTAADACKDECTO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'RTAADACKDECTO'
		, '{default}'
		, 'RaPId - Acknowledged Additional Damages Decision Timeout'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end

---- 8
update dbCodeLookup
set cdDesc = 'Claims Portal - %FILES%'
where cdtype = 'DLGTABCAPTION'	and cdcode = 'FDSCHCOMRPDMAIN'


UPDATE dbcodelookup SET CDDESC='Claims Portal - Notification(s)'
where cdtype='DLGTABCAPTION'
and cdcode='FDSCHCOMRPDNOTF'


UPDATE dbcodelookup SET CDDESC='Claims Portal - Activity Required'
where cdtype='DLGTABCAPTION'
and cdcode='FDSCHCOMRPDCLM'


UPDATE dbcodelookup SET CDDESC='Claims Portal'
where cdtype='DLGTABCAPTION'
and cdcode='FDSCRUSRRPDPSWD'

------ 9
-- CLAIMS PORTAL DEFAULT MATTER TYPE
insert into dbSpecificData
 (
  spLookup
  , brID
  , spData
 )
 
select 
  'CPDEFAULTTYPE' 
  , dbbranch.BRID
  , NULL
from dbbranch 
left join dbspecificdata on dbspecificdata.splookup = 'CPDEFAULTTYPE' and dbbranch.brid = dbspecificdata.brid 
where
dbspecificdata.rowguid is null




-- CLAIMS PORTAL DEFAULT PROCESS ('RTA' OR 'ELPL')
insert into dbSpecificData
 (
  spLookup
  , brID
  , spData
 )
 
select 
  'CPDEFPROCESS' 
  , dbbranch.BRID
  , NULL
from dbbranch 
left join dbspecificdata on dbspecificdata.splookup = 'CPDEFPROCESS' and dbbranch.brid = dbspecificdata.brid 
where
dbspecificdata.rowguid is null



-- CLAIMS PORTAL AUTOMATIC UPDATES
insert into dbSpecificData
 (
  spLookup
  , brID
  , spData
 )
 select 
  'CPAUTOUPDATES' 
  , dbbranch.BRID
  , 'N'
from dbbranch 
left join dbspecificdata on dbspecificdata.splookup = 'CPAUTOUPDATES' and dbbranch.brid = dbspecificdata.brid 
where
dbspecificdata.rowguid is null




-- CLAIMS PORTAL AUTOMATIC TASK CREATION/COMPLETION
insert into dbSpecificData
 (
  spLookup
  , brID
  , spData
 )
 
select 
  'CPAUTOTASKS' 
  , dbbranch.BRID
  , 'N'
from 
	dbbranch 
left join 
	dbspecificdata on dbspecificdata.splookup = 'CPAUTOTASKS' and dbbranch.brid = dbspecificdata.brid 
where
	dbspecificdata.rowguid is null



-- CLAIMS PORTAL AUTOMATIC TASK CREATION/COMPLETION
insert into dbSpecificData
(
  spLookup
  , brID
  , spData
)

select 
  'CPAUTOTASKS' 
  , dbbranch.BRID
  , 'N'
from 
       dbbranch 
left join 
       dbspecificdata on dbspecificdata.splookup = 'CPAUTOTASKS' and dbbranch.brid = dbspecificdata.brid 
where
       dbspecificdata.rowguid is null
------- 13

if not exists (select * from dbSpecificData where spLookup = 'ELPLXMLFILELOC')
begin
	insert into dbSpecificData 
	(
		spLookup
		, brID
		, spData
		, rowguid
	)
	values
	(
		'ELPLXMLFILELOC'
		, 1
		, NULL
		, (newid())
	)
end


-- ELPL URL
insert into dbSpecificData
 (
  spLookup
  , brID
  , spData
 )
 
select 
  'ELPLPORTALDETS' 
  , dbbranch.BRID
  , NULL
from dbbranch 
left join dbspecificdata on dbspecificdata.splookup = 'ELPLPORTALDETS' and dbbranch.brid = dbspecificdata.brid 
where
dbspecificdata.rowguid is null

----- 14


IF NOT EXISTS(SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT'	AND CDCODE = 'ELINTERIM')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'ELINTERIM'
		, '{default}'
		, 'ELPL - Interim Pack Submitted to ELPL Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END
ELSE
BEGIN
	PRINT '"ELINTERIM" ALREADY EXISTS AS A FILEEVENT CODE. THIS ITEM HAS NOT BEEN RE-ADDED.'
END


IF NOT EXISTS (SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT' AND CDCODE = 'ELSETTLEMENT')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'ELSETTLEMENT'
		, '{default}'
		, 'ELPL - Settlement Pack Submitted to ELPL Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END
ELSE
BEGIN
	PRINT '"ELSETTLEMENT" ALREADY EXISTS AS A FILEEVENT CODE. THIS ITEM HAS NOT BEEN RE-ADDED.'
END


IF NOT EXISTS (SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT' AND CDCODE = 'ELCOUNTEROFFER')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'ELCOUNTEROFFER'
		, '{default}'
		, 'ELPL - Settlement Pack Counter Offer Submitted to ELPL Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END
ELSE
BEGIN 
	PRINT '"ELCOUNTEROFFER" ALREADY EXISTS AS A FILEEVENT CODE. THIS ITEM HAS NOT BEEN RE-ADDED.'
END




IF NOT EXISTS (SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT' AND CDCODE = 'ELDEFRESPONSE')
BEGIN 
	INSERT INTO DBCODELOOKUP
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	VALUES
	(
		'FILEEVENT'
		, 'ELDEFRESPONSE'
		, '{default}'
		, 'ELPL - Defendant Insurer''s Response Data Added by User'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
END
ELSE
BEGIN 
	PRINT '"ELDEFRESPONSE" ALREADY EXISTS AS A FILEEVENT CODE. THIS ITEM HAS NOT BEEN RE-ADDED.'
END


--IF NOT EXISTS(SELECT * FROM DBCODELOOKUP WHERE CDTYPE = 'FILEEVENT'	AND CDCODE = 'ELADDDAMAGES')
--BEGIN 
--	INSERT INTO DBCODELOOKUP
--	(
--		cdType
--		, cdCode
--		, cdUICultureInfo
--		, cdDesc
--		, cdSystem
--		, cdDeletable
--		, cdAddLink
--		, cdHelp
--		, cdNotes
--		, cdGroup
--	)
--	VALUES
--	(
--		'FILEEVENT'
--		, 'ELADDDAMAGES'
--		, '{default}'
--		, 'ELPL - Settlement Pack Additional Damages submitted to ELPL portal'
--		, 0
--		, 1
--		, NULL
--		, NULL
--		, NULL
--		, 0
--	)
--END


if not exists (select * from dbCodeLookup where cdType = 'FILEEVENT' and cdCode = 'ELERROR')
begin
	insert into dbCodeLookup
	(	
		cdType
		,cdCode
		,cdUICultureInfo
		,cdDesc
		,cdSystem
		,cdDeletable
		,cdAddLink
		,cdHelp
		,cdNotes
		,cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELERROR'
		, '{default}'
		, 'ELPL - Error Generated Upon ELPL Data Submission'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end





if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELSUBMITCLAIM')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELSUBMITCLAIM'
		, '{default}'
		, 'ELPL - Claim Submitted to ELPL Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELRESUBMIT')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELRESUBMIT'
		, '{default}'
		, 'ELPL - Claim Resubmitted to ELPL Portal'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKDENLIAB')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKDENLIAB'
		, '{default}'
		, 'ELPL - Acknowledged Denied Liability'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKLIABCHILD')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKLIABCHILD'
		, '{default}'
		, 'ELPL - Acknowledged Liability Admitted for Child'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKLIABNEG')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKLIABNEG'
		, '{default}'
		, 'ELPL - Acknowledged Liability Admitted with Negligence'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKLIABTO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKLIABTO'
		, '{default}'
		, 'ELPL - Acknowledged Liability Decision Timeout'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


--if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELSTG1PAYMENT')
--begin
--	insert into dbCodeLookup
--	(
--		cdType
--		, cdCode
--		, cdUICultureInfo
--		, cdDesc
--		, cdSystem
--		, cdDeletable
--		, cdAddLink
--		, cdHelp
--		, cdNotes
--		, cdGroup
--	)
--	values
--	(
--		'FILEEVENT'
--		, 'ELSTG1PAYMENT'
--		, '{default}'
--		, 'ELPL - Confirmation of Initial Stage 1 Payment'
--		, 0
--		, 1
--		, NULL
--		, NULL
--		, NULL
--		, 0
--	)
--end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKLIABADMT')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKLIABADMT'
		, '{default}'
		, 'ELPL - Acknowledged Liability Admitted'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELEXITPROCESS')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELEXITPROCESS'
		, '{default}'
		, 'ELPL - Requested Early Exit from Claim'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKEXITPRO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKEXITPRO'
		, '{default}'
		, 'ELPL - Acknowledged Early Exit Request'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELINTERIMREQ')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELINTERIMREQ'
		, '{default}'
		, 'ELPL - Interim Settlement Pack Required'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELINTPAYREC')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELINTPAYREC'
		, '{default}'
		, 'ELPL - Interim Settlement Pack Payment Received'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELINTPARTIAL')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELINTPARTIAL'
		, '{default}'
		, 'ELPL - Acceptance of Partial Interim Settlement Pack Payment'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKINTPAYTO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKINTPAYTO'
		, '{default}'
		, 'ELPL - Acknowledged Interim Payment Decision Timeout'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKINTREJ')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKINTREJ'
		, '{default}'
		, 'ELPL - Acknowledged Rejected Interim Settlement Pack'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELRETURNSTG21')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELRETURNSTG21'
		, '{default}'
		, 'ELPL - Return to Start of Stage 2.1'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKSPCONFIRM')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKSPCONFIRM'
		, '{default}'
		, 'ELPL - Acknowledged Settlement Pack Confirmation'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKSPREPUD')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKSPREPUD'
		, '{default}'
		, 'ELPL - Acknowledged Settlement Pack Repudiation'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKSPNOTAGRD')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKSPNOTAGRD'
		, '{default}'
		, 'ELPL - Acknowledged Settlement Pack Not Agreed'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKSPAGRD')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKSPAGRD'
		, '{default}'
		, 'ELPL - Acknowledged Settlement Pack Agreed'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELCONEEDED')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELCONEEDED'
		, '{default}'
		, 'ELPL - Counter Offer Needed'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end


if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELSPDECISION')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELSPDECISION'
		, '{default}'
		, 'ELPL - Settlement Pack Agreement Decision'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end



--if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELADEXISTENCE')
--begin
--	insert into dbCodeLookup
--	(
--		cdType
--		, cdCode
--		, cdUICultureInfo
--		, cdDesc
--		, cdSystem
--		, cdDeletable
--		, cdAddLink
--		, cdHelp
--		, cdNotes
--		, cdGroup
--	)
--	values
--	(
--		'FILEEVENT'
--		, 'ELADEXISTENCE'
--		, '{default}'
--		, 'ELPL - Existence of Additional Damages'
--		, 0
--		, 1
--		, NULL
--		, NULL
--		, NULL
--		, 0
--	)
--end



--if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELADDECISION')
--begin
--	insert into dbCodeLookup
--	(
--		cdType
--		, cdCode
--		, cdUICultureInfo
--		, cdDesc
--		, cdSystem
--		, cdDeletable
--		, cdAddLink
--		, cdHelp
--		, cdNotes
--		, cdGroup
--	)
--	values
--	(
--		'FILEEVENT'
--		, 'ELADDECISION'
--		, '{default}'
--		, 'ELPL - Decision on Additional Damages'
--		, 0
--		, 1
--		, NULL
--		, NULL
--		, NULL
--		, 0
--	)
--end



--if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKADALLDMGS')
--begin
--	insert into dbCodeLookup
--	(
--		cdType
--		, cdCode
--		, cdUICultureInfo
--		, cdDesc
--		, cdSystem
--		, cdDeletable
--		, cdAddLink
--		, cdHelp
--		, cdNotes
--		, cdGroup
--	)
--	values
--	(
--		'FILEEVENT'
--		, 'ELADACKALLDMGS'
--		, '{default}'
--		, 'ELPL - Acknowledged All Additional Damages Agreed'
--		, 0
--		, 1
--		, NULL
--		, NULL
--		, NULL
--		, 0
--	)
--end


--if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKADDMGS')
--begin
--	insert into dbCodeLookup
--	(
--		cdType
--		, cdCode
--		, cdUICultureInfo
--		, cdDesc
--		, cdSystem
--		, cdDeletable
--		, cdAddLink
--		, cdHelp
--		, cdNotes
--		, cdGroup
--	)
--	values
--	(
--		'FILEEVENT'
--		, 'ELACKADDMGS'
--		, '{default}'
--		, 'ELPL - Acknowledged Additional Damages Agreed'
--		, 0
--		, 1
--		, NULL
--		, NULL
--		, NULL
--		, 0
--	)
--end



if not exists (select * from dbCodeLookup where cdtype = 'FILEEVENT' and cdCode like 'ELACKADDECTO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	(
		'FILEEVENT'
		, 'ELACKADDECTO'
		, '{default}'
		, 'ELPL - Acknowledged Additional Damages Decision Timeout'
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	)
end

---16
-- New events added for new sections in rejected claims part of stage 1. RTA versions:
	if not exists (select * from dbCodeLookup where cdType = 'FILEEVENT' and cdCode = 'RTAACKREJECT')
	begin
		insert into dbCodeLookup
		(
			cdType	
			, cdCode	
			, cdUICultureInfo	
			, cdDesc	
			, cdSystem	
			, cdDeletable	
			, cdAddLink	
			, cdHelp	
			, cdNotes	
			, cdGroup	
		)
		values
		(
			'FILEEVENT'
			, 'RTAACKREJECT'
			, '{default}'
			, 'RaPId - Acknowledgement of claim rejection submitted'
			, 0	
			, 1	
			, NULL	
			, NULL	
			, NULL	
			, 0	
		)
	end
	
	
	if not exists (select * from dbCodeLookup where cdType = 'FILEEVENT' and cdCode = 'RTAEXITREJCLM')
	begin
		insert into dbCodeLookup
		(
			cdType	
			, cdCode	
			, cdUICultureInfo	
			, cdDesc	
			, cdSystem	
			, cdDeletable	
			, cdAddLink	
			, cdHelp	
			, cdNotes	
			, cdGroup	
		)
		values
		(
			'FILEEVENT'
			, 'RTAEXITREJCLM'
			, '{default}'
			, 'RaPId - Exit of rejected claim submitted'
			, 0	
			, 1	
			, NULL	
			, NULL	
			, NULL	
			, 0	
		)
	end

--...and ELPL versions:
	if not exists (select * from dbCodeLookup where cdType = 'FILEEVENT' and cdCode = 'ELPLACKREJECT')
		begin
			insert into dbCodeLookup
			(
				cdType	
				, cdCode	
				, cdUICultureInfo	
				, cdDesc	
				, cdSystem	
				, cdDeletable	
				, cdAddLink	
				, cdHelp	
				, cdNotes	
				, cdGroup	
			)
			values
			(
				'FILEEVENT'
				, 'ELPLACKREJECT'
				, '{default}'
				, 'ELPL - Acknowledgement of claim rejection submitted'
				, 0	
				, 1	
				, NULL	
				, NULL	
				, NULL	
				, 0	
			)
		end
		
		
		if not exists (select * from dbCodeLookup where cdType = 'FILEEVENT' and cdCode = 'ELPLEXITREJCLM')
		begin
			insert into dbCodeLookup
			(
				cdType	
				, cdCode	
				, cdUICultureInfo	
				, cdDesc	
				, cdSystem	
				, cdDeletable	
				, cdAddLink	
				, cdHelp	
				, cdNotes	
				, cdGroup	
			)
			values
			(
				'FILEEVENT'
				, 'ELPLEXITREJCLM'
				, '{default}'
				, 'ELPL - Exit of rejected claim submitted'
				, 0	
				, 1	
				, NULL	
				, NULL	
				, NULL	
				, 0	
			)
		end

----------- 17

	if not exists(select * from dbState where stateCode = 'ACTIVITYREQUIRED_DOWNLOADING_ELPL')
	begin
		insert into dbState
		(
			stateCode
			, brID
			, usrID
			, stateData
		)
		values
		(
			'ACTIVITYREQUIRED_DOWNLOADING_ELPL'
			, NULL
			, NULL
			, NULL
		)
	
		print 'ACTIVITYREQUIRED_DOWNLOADING_ELPL added to dbState.'
	end
	else
	begin
		print 'ACTIVITYREQUIRED_DOWNLOADING_ELPL already exists in dbState. Skipping...'
	end
	
	
	if not exists(select * from dbState where stateCode = 'ACTIVITYREQUIRED_LASTDOWNLOADED_ELPL')
	begin
		insert into dbState
		(
			stateCode
			, brID
			, usrID
			, stateData
		)
		values
		(
			'ACTIVITYREQUIRED_LASTDOWNLOADED_ELPL'
			, NULL
			, NULL
			, NULL
		)
	
		print 'ACTIVITYREQUIRED_LASTDOWNLOADED_ELPL added to dbState.'
	end
	else
	begin
		print 'ACTIVITYREQUIRED_LASTDOWNLOADED_ELPL already exists in dbState. Skipping...'
	end
	
	
	if not exists(select * from dbState where stateCode = 'ACTIVITYREQUIRED_DOWNLOADING_RTA')
	begin
		insert into dbState
		(
			stateCode
			, brID
			, usrID
			, stateData
		)
		values
		(
			'ACTIVITYREQUIRED_DOWNLOADING_RTA'
			, NULL
			, NULL
			, NULL
		)
	
		print 'ACTIVITYREQUIRED_DOWNLOADING_RTA added to dbState.'
	end
	else
	begin
		print 'ACTIVITYREQUIRED_DOWNLOADING_RTA already exists in dbState. Skipping...'
	end
	
	
	if not exists(select * from dbState where stateCode = 'ACTIVITYREQUIRED_LASTDOWNLOADED_RTA')
	begin
		insert into dbState
		(
			stateCode
			, brID
			, usrID
			, stateData
		)
		values
		(
			'ACTIVITYREQUIRED_LASTDOWNLOADED_RTA'
			, NULL
			, NULL
			, NULL
		)
	
		print 'ACTIVITYREQUIRED_LASTDOWNLOADED_RTA added to dbState.'
	end
	else
	begin
		print 'ACTIVITYREQUIRED_LASTDOWNLOADED_RTA already exists in dbState. Skipping...'
	end
---------------



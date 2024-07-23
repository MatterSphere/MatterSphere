
CREATE PROCEDURE [dbo].[sprContactLinksAutomatedSetup]
AS

--***************************************************
--*********** Add Enterprise Client Type ************
--***************************************************

--Add the Code Lookup for the Client Type

IF NOT EXISTS ( SELECT cdCode FROM dbo.dbCodeLookup WHERE cdType = 'CLTYPE' AND cdCode = 'ENT')
BEGIN
	INSERT INTO dbo.dbCodeLookup (cdType, cdCode, cdUICultureInfo, cdDesc, cdSystem, cdDeletable, cdAddLink, cdHelp, cdNotes, cdGroup)
	VALUES ('CLTYPE','ENT','{Default}','Enterprise',1,0,NULL,NULL,NULL,0)
END


--Create the Client Type

IF NOT EXISTS ( SELECT typeCode FROM dbo.dbClientType WHERE typeCode = 'ENT' )
BEGIN
	INSERT dbo.dbClientType ( typeCode, typeVersion, typeXML, typeGlyph, typeSeed, typeActive )
	VALUES ('ENT', '60', '<?xml version="1.0"?><Config><ClientDescription>{\rtf1\ansi\ansicpg1252\deff0\deflang1033\deflangfe1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Tahoma;}}  {\*\generator Msftedit 5.41.15.1503;}\viewkind4\uc1\pard\b\f0\fs16 Premise Name :-&lt;BS&gt;\par  \b0 &lt;CLIENT:CLName&gt;\b &lt;BS&gt;&lt;BS&gt;\b0\par  \b Default Contact :-&lt;BS&gt;\fs18\par  \b0\fs16 &lt;DefaultContact:ContName&gt;&lt;BS&gt;\par  &lt;DefaultContactAddress:AddLine1&gt;&lt;BS&gt;\par  &lt;DefaultContactAddress:AddLine2&gt;&lt;BS&gt;\par  &lt;DefaultContactAddress:AddLine3&gt;&lt;BS&gt;\par  &lt;DefaultContactAddress:AddLine4&gt;&lt;BS&gt;\par  &lt;DefaultContactAddress:AddLine5&gt;&lt;BS&gt;\par  &lt;DefaultContactAddress:AddPostCode&gt;&lt;BS&gt;\par  \par  \b Salutation :  \b0 &lt;DefaultContact:ContSalut&gt;\par  &lt;loop:DefaultContactNumbers&gt;\b &lt;DefaultContactNumbers:NumberTypeDesc&gt; \b0 : &lt;DefaultContactNumbers:ContNumber&gt;\par  &lt;/loop:DefaultContactNumbers&gt;\par  &lt;loop:DEFAULTCONTACTEMAILS&gt;\b &lt;DEFAULTCONTACTEMAILS:EmailTypeDesc&gt;\b0  : mailto:&lt;DEFAULTCONTACTEMAILS:ContEmail&gt;\par  &lt;/loop:DEFAULTCONTACTEMAILS&gt;\par  }   </ClientDescription><Dialog><Form lookup="CLCAPTION" width="715" height="513" /><Buttons><Button id="0" name="cmdBack" lookup="BTNBACK" visible="True" /><Button id="1" name="cmdRefresh" lookup="BTNREFRESH" visible="True" /><Button id="2" name="cmdSave" lookup="BTNSAVE" visible="True" /><Button id="3" name="cmdOK" lookup="BTNOK" visible="True" /><Button id="4" name="cmdCancel" lookup="BTNCANCEL" visible="True" /></Buttons><Panels width="200" backcolor="Salmon" forecolor="" brightness="50"><Panel lookup="CLDETAILS" order="0" expanded="True" height="225" property="ClientDescription" glyph="Cyan" panelType="Property" /></Panels><Tabs><Tab lookup="CLDETAILS" glyph="23" order="0" type="ENQ" source="SCRCLIMAIN" tabtype="Enquiry" /><Tab lookup="CLFILES" glyph="58" order="1" type="ADDIN" source="SCHCLIFILELSTAD" /><Tab lookup="CLCONTACTS" glyph="24" order="2" type="ADDIN" source="SCHCLICONTACTS" conditional="IsPackageInstalled(&quot;CLMCONTLEGAL&quot;)" /><Tab lookup="-1203789504" glyph="24" source="SCRCLINOTES" conditional="IsPackageInstalled(&quot;CLMCONTLEGAL&quot;)" /><Tab lookup="-77326977" glyph="59" source="SCHCLIARCHLIST" conditional="Ispackageinstalled(&quot;ArchivedDoc&quot;)" /><Tab lookup="CLDOCUMENTS" glyph="0" source="SCHCLIDOCALL" tabtype="Addin" conditional="IsPackageInstalled(&quot;DMS&quot;)" /><Tab lookup="SCHCLICOMPLAINT" glyph="57" source="SCHCLICOMPLAINT" conditional="Ispackageinstalled(&quot;Complaints&quot;)" /><Tab lookup="-761812480" glyph="29" source="SCRCLIMARKETING" conditional="IsPackageInstalled(&quot;CLMCONTLEGAL&quot;)" /></Tabs></Dialog><ExtendedDataList><ExtendedData code="ADDCLIENTINFO" /></ExtendedDataList><Settings minContactCount="1" maxContactCount="3" contactCount="2" searchOnCreate="True" quickContactType="ORGANISATION" wizard="SCRCLINEWCORP" /><defaultTemplates /></Config>', 1, 'CL', 1)
END




--***************************************************
--********* Add Contact Links Specific Data *********
--***************************************************


-- Insert Contact Links Specific Data entries for each branch
	
INSERT INTO dbSpecificData(spLookup, brID, spData)
(
	select distinct 'CONTLINKINCCLTS' as spLookup,
			b.brID,
			'ENT;1;2' as spData
	from
			dbBranch b left join dbSpecificData s on b.brID = s.brID
	where
			((select 'CONTLINKINCCLTS' + ',' + cast(b.brID as nvarchar(15))) not in (select s.spLookup + ',' + cast(s.brID as nvarchar(15)) from dbSpecificData s))
)



--***************************************************
--***** Add Contact Links tab to Contacts types *****
--***************************************************


--Insert the Code Lookup for the tab caption

IF NOT EXISTS ( SELECT cdCode FROM dbo.dbCodeLookup WHERE cdType = 'DLGTABCAPTION' AND cdCode = 'CONTTYPESCLTAB')
BEGIN
	INSERT INTO dbo.dbCodeLookup (cdType, cdCode, cdUICultureInfo, cdDesc, cdSystem, cdDeletable, cdAddLink, cdHelp, cdNotes, cdGroup)
	VALUES ('DLGTABCAPTION','CONTTYPESCLTAB','{Default}','Contact Links',1,0,NULL,NULL,NULL,0)
END


--Insert the new tab into the tabs collection for the Contact types

update a
set a.typeXML = substring(convert(nvarchar(max),a.[typeXML]),0,charindex('</Tabs>',convert(nvarchar(max),a.[typeXML])))
+ '<Tab lookup="CONTTYPESCLTAB" source="FDSCRCONLINKLST" tabtype="Enquiry" glyph="49" />'
+ substring(convert(nvarchar(max),a.[typeXML]),charindex('</Tabs>',convert(nvarchar(max),a.[typeXML])),len(convert(nvarchar(max),a.[typeXML])))
FROM [dbo].[dbContactType] a
WHERE not exists
	(SELECT 1
		FROM [dbo].[dbContactType] b
		WHERE b.typeXML like '% source="FDSCRCONLINKLST" %'
		and b.typeCode = a.typeCode)



SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactLinksAutomatedSetup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactLinksAutomatedSetup] TO [OMSAdminRole]
    AS [dbo];


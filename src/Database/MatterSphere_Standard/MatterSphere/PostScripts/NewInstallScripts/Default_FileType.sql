Print 'NewInstallScripts\Default_FileType.sql'


-- Add default File type
IF NOT EXISTS ( SELECT typeCode FROM dbo.dbFileType WHERE typeCode = 'TEMPLATE' )
BEGIN
	INSERT dbo.dbFileType ( typeCode, typeversion, typeXML, typeGlyph, typeSeed, typeActive, fileDeptCode, fileDefFundCode, fileDestroyPeriod, filePrecCategory, filePrecSubCategory, filePrecMinorCategory, fileMilestoneActive, fileReview, fileReviewdays, fileRemoteAccSet )
	VALUES ( 'TEMPLATE', 1282, '<?xml version="1.0"?><Config><Dialog>
<Form lookup="FILECAPTION" width="715" height="513" />
<Buttons><Button id="0" name="cmdBack" lookup="BTNBACK" visible="True" />
<Button id="1" name="cmdRefresh" lookup="BTNREFRESH" visible="True" />
<Button id="2" name="cmdSave" lookup="BTNSAVE" visible="True" />
<Button id="3" name="cmdOK" lookup="BTNOK" visible="True" />
<Button id="4" name="cmdCancel" lookup="BTNCANCEL" visible="True" />
</Buttons>
<Panels width="185" backcolor="CadetBlue" forecolor="" brightness="100">
<Panel lookup="-1613643696" height="100" expanded="False" glyph="Grey" property="File" panelType="TimeStatistics" conditional="Ispackageinstalled(&quot;TimeRecording&quot;)" />
<Panel lookup="CLDETAILS" property="FileClientDescription" />
</Panels>
<Tabs>
<Tab lookup="FILEDETAILS" glyph="23" order="0" source="SCRFILMAIN" group="NAVGRPFILINFO" />
<Tab lookup="CLIDETAILSTAB" order="1" source="SCRCLIDETAILS" tabtype="Enquiry" group="NAVGRPFILINFO" />
<Tab lookup="CLDOCUMENTS" glyph="58" order="5" source="SCHDOCALLTRAN" conditional="Ispackageinstalled(&quot;DMS&quot;)" group="NAVGRPDOCUMENTS" hidebuttons="True" />
<Tab lookup="CLAPPOINTMENTS" glyph="29" order="3" source="SCHFILAPPOINTS" conditional="Ispackageinstalled(&quot;Appointments&quot;)" group="NAVGRPFILMANAGE" hidebuttons="True" />
<Tab lookup="CLASSOCIATES" glyph="4" order="2" source="SCHFILASSLIST" conditional="IsPackageInstalled(&quot;CLMCONTLEGAL&quot;)" group="NAVGRPFILINFO" hidebuttons="True" />
<Tab lookup="-410578691" glyph="29" source="SCHFILTIMELIST" conditional="Ispackageinstalled(&quot;TimeRecording&quot;)&#xD;&#xA;IsLicensedFor(&quot;TIMEREC&quot;)" group="NAVGRPFINANCE" />
<Tab lookup="227574651" glyph="7" source="SCHFILTASKS" conditional="Ispackageinstalled(&quot;tasks&quot;)" group="NAVGRPFILMANAGE" hidebuttons="True" />
<Tab lookup="1788437295" glyph="7" source="ADDFMTASKSFIL" tabtype="Addin" conditional="IsPackageInstalled(&quot;FILEMANAGEMENT&quot;)" userRoles="" group="NAVGRPFILMANAGE" hidebuttons="True" />
<Tab lookup="-1404627464" glyph="27" source="SCRFILMSTAD" conditional="Ispackageinstalled(&quot;Milestones&quot;)" group="NAVGRPFILMANAGE" />
<Tab lookup="671482604" glyph="7" source="ADDFMMSPLAN" tabtype="Addin" conditional="IsPackageInstalled(&quot;FILEMANAGEMENT&quot;)" group="NAVGRPFILMANAGE" />
<Tab lookup="SCHFILKEYDATES" glyph="29" source="SCHFILKEYDATES" conditional="Ispackageinstalled(&quot;KeyDates&quot;)" group="NAVGRPFILMANAGE" hidebuttons="True" />
<Tab lookup="CLUNDERTAKINGS" glyph="30" order="4" source="SCHFILUTAKING" conditional="Ispackageinstalled(&quot;Undertakings&quot;)" group="NAVGRPFILMANAGE" hidebuttons="True" />
<Tab lookup="1209491420" glyph="27" source="SCHFILEVENTS" conditional="Ispackageinstalled(&quot;CLMCONTLEGAL&quot;)" />
<Tab lookup="-395998192" glyph="43" source="SCRFILFUNDINGAD" conditional="Ispackageinstalled(&quot;CLMCONTLEGAL&quot;)" group="NAVGRPFINANCE" />
<Tab lookup="795700281" source="SCHSYSFINHIST" tabtype="ListGroup" glyph="25" conditional="Ispackageinstalled(&quot;ACCSLIP&quot;)" group="NAVGRPFINANCE" />
<Tab lookup="-1390301088" glyph="31" source="SCHFILREMOTEACC" tabtype="List" conditional="Ispackageinstalled(&quot;RemoteAccount&quot;)" />
<Tab lookup="1084535360" glyph="24" source="SCRFILNOTES" tabtype="Enquiry" group="NAVGRPFILINFO" />
<Tab lookup="-905451847" glyph="65" source="SCHFILPHASES" tabtype="List" conditional="IsPackageInstalled(&quot;PHASES&quot;)" />
</Tabs></Dialog><Settings /><ExtendedDataList><ExtendedData code="ADDFILEINFO" />
</ExtendedDataList><Jobs /><defaultTemplates /></Config>'
	, 1
	, 'FI'
	, 1
	, 'TEMPLATE'
	, 'NOCHG'
	, 2190
	, 'TEMPLATE'
	, 'TEMPLATE'
	, 'TEMPLATE'
	, 1
	, 1
	, 0
	, '00000' )
END
GO	
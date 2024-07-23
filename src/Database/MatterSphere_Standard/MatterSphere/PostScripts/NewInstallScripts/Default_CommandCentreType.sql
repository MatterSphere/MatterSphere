Print 'Starting NewInstallScripts\Default_CommandCentreType.sql'


-- Add default Command Centre types
IF NOT EXISTS ( SELECT typeCode FROM dbo.dbCommandCentreType WHERE typeCode = 'STANDARD' )
BEGIN
	INSERT dbo.dbCommandCentreType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ('STANDARD', 48, '
   <Config>
   <Dialog>
      <Form lookup="FEECAPTION" />
      <Tabs>
         <Tab lookup="DASHBOARD" source="DSHSYSTEM" tabtype="Addin" group="CMNDCTRCAPTION" hidebuttons="True" glyph="38" conditional="Ispackageinstalled(&quot;CommandCentre&quot;)" />
         <Tab lookup="CONTACTLIST" source="SCHCONTLIST" tabtype="List" group="NAVGRPCLIENTS" hidebuttons="True" glyph="39" conditional="Ispackageinstalled(&quot;CommandCentre&quot;)" />
         <Tab lookup="593393211" source="SCHFEEFILELIST" tabtype="List" glyph="18" conditional="Ispackageinstalled(&quot;CommandCentre&quot;)" group="NAVGRPFILINFO" />
         <Tab lookup="-1274905461" glyph="26" source="SCHFEEREVMGR" tabtype="ListGroup" conditional="Ispackageinstalled(&quot;CommandCentre&quot;)" group="NAVGRPFILINFO" />
         <Tab lookup="1788437295" glyph="4" source="ADDFMTASKSUSR" tabtype="Addin" conditional="IsPackageInstalled(&quot;FILEMANAGEMENT&quot;)" group="NAVGRPFILMANAGE" hidebuttons ="True" />
         <Tab lookup="177651448" source="SCHFEEMSMANAGER" tabtype="List" glyph="13" conditional="Ispackageinstalled(&quot;Milestones&quot;)" group="NAVGRPFILMANAGE" />
         <Tab lookup="CLAPPOINTMENTS" glyph="3" source="SCHFEEAPPLST" tabtype="List" conditional="Ispackageinstalled(&quot;Appointments&quot;)" group="NAVGRPFILMANAGE" hidebuttons="True" />
         <Tab lookup="-1349601477" source="SCHCOMARCHALL" tabtype="List" glyph="8" userRoles="" conditional="Ispackageinstalled(&quot;ArchivedDoc&quot;)" group="NAVGRPDOCUMENTS" />
         <Tab lookup="334876670" source="SCHCOMFINAUTH" tabtype="List" glyph="15" conditional="IsPackageInstalled(&quot;ACCSLIP&quot;)" group="NAVGRPFINANCE" />
         <Tab lookup="-129345043" source="SCHCOMAWAITACK" tabtype="List" glyph="25" conditional="IsPackageInstalled(&quot;DTS&quot;)" group="NAVGRPFILINFO" />
         <Tab lookup="654467957" glyph="20" source="SCHFEERSKMANAGE" tabtype="List" conditional="IsPackageInstalled(&quot;RISKASS&quot;)" userRoles="RISKMANAGER,ADMIN" group="NAVGRPFILMANAGE" />
         <Tab lookup="-444739803" glyph="19" source="UDSCHPROOFOFID" tabtype="List" conditional="IsPackageInstalled(&quot;SECURITY&quot;)" userRoles="MONEYLAUND" group="NAVGRPFILMANAGE" />
         <Tab lookup="-143822477" glyph="1" source="SCHCOMSCANPOST" tabtype="List" conditional="IsPackageInstalled(&quot;SCANNING&quot;)" userRoles="VIEWSCANNEDITMS" group="-961749066" />
         <Tab lookup="-1120902713" glyph="7" source="SCRCOMWEBFAVS" tabtype="Enquiry" conditional="IsPackageInstalled(&quot;WEBFAVS&quot;)" />
         <Tab lookup="SCHCLICOMPLAINT" source="SCHCOMCOMPMNG" tabtype="List" glyph="17" conditional="IsPackageInstalled(&quot;COMPLAINTS&quot;)" />
         <Tab lookup="CLUNDERTAKINGS" source="SCHCOMUNDTAKING" tabtype="List" conditional="IsPackageInstalled(&quot;UNDERTAKINGS&quot;)" glyph="24" group="NAVGRPFILMANAGE" hidebuttons ="True" />
      </Tabs>
      <Panels />
   </Dialog>
   <ExtendedDataList />
   <Settings />
   <defaultTemplates />
</Config>
	', 0, 1)
END
GO

IF NOT EXISTS ( SELECT typeCode FROM dbo.dbCommandCentreType WHERE typeCode = 'SUPPORT' )
BEGIN
	INSERT dbo.dbCommandCentreType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ('SUPPORT', 10, '<Config><Dialog><Form lookup="FEECAPTION" /><Tabs><Tab lookup="5862329" source="GRPSUPUI" tabtype="ListGroup" glyph="0" /><Tab lookup="-1565288618" source="GRPSUPEXTDATA" tabtype="ListGroup" glyph="28" /><Tab lookup="33203404" glyph="8" source="GRPSUPPKG" tabtype="ListGroup" /><Tab lookup="1100358628" source="SCRSQLCONFIG" tabtype="Enquiry" glyph="28" /><Tab lookup="642710536" source="SCRSQLFILES" tabtype="Enquiry" glyph="28" /><Tab lookup="1597838259" source="SCHSERVERPRO" tabtype="List" glyph="28" /><Tab lookup="-1070598953" source="SCHSERVERCHKSUM" tabtype="List" glyph="28" /></Tabs><Panels backcolor="Sienna" /></Dialog><ExtendedDataList /><Settings /><defaultTemplates /></Config>', 4, 1)
END
GO	




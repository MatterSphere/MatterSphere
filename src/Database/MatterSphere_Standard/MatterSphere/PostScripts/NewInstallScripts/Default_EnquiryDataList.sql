Print 'Starting NewInstallScripts\Default_EnquiryDataList.sql'



-- Add default Enquiry data list
IF NOT EXISTS ( SELECT enqTable FROM dbo.dbEnquiryDataList WHERE enqTable = 'DSADMENU' )
BEGIN
	INSERT dbo.dbEnquiryDataList ( enqTable , enqSourceType , enqSource , enqCall , enqParameters , enqSystem , Created , CreatedBy , Updated , UpdatedBy )
	VALUES ( 'DSADMENU' , 'OMS' , '' , 'SELECT *, dbo.GetCodeLookupDesc(''ADMINMENU'', admnucode, @UI) as [menudesc] from dbAdminMenu WHERE admnuName = @MENUNAME ORDER BY admnuorder,admnuSearchListCode,menudesc' , 
			'<params><param name="@UI" type="NVarChar" test="fr-ca">%#UI%</param><param name="@MENUNAME" type="NVarChar" test="ADMIN">%MENUNAME%</param></params>' , 1 , Getdate() , -1 , Getdate() , -1 )
END
GO					




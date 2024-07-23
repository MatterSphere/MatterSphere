

CREATE PROCEDURE [dbo].[sprConfigurableType] (@xml ntext, @UI uUICultureInfo, @UIType tinyint = 0, @handle int = 0) AS
	
	if @handle = 0
		exec sp_xml_preparedocument @handle output, @xml
	
	--FORM
	select top 1 lookup, dbo.GetCodeLookupDesc('DLGFRMCAPTION', lookup, @UI) as [frmdesc]
		from openxml(@handle, 'Config/Dialog/Form', 1) 
		with (
			lookup 		nvarchar(15)
			)
	--BUTTON
	select [id], dbo.GetCodeLookupDesc('DLGBTNCAPTION', lookup, @UI) as [btndesc], dbo.GetCodeLookupHelp('DLGBTNCAPTION', lookup, @UI) as [btnhelp]
		from openxml(@handle, 'Config/Dialog/Buttons/Button', 1) 
		with (
			[id]		int,
			lookup		nvarchar(15)
		)
	--PANEL
	select lookup, dbo.GetCodeLookupDesc('DLGPNLCAPTION', lookup, @UI) as [pnldesc], dbo.GetCodeLookupHelp('DLGPNLCAPTION', lookup, @UI) as [pnlhelp]
	from openxml(@handle, 'Config/Dialog/Panels/Panel', 1) 
		with (
			lookup 		nvarchar(15)
		)
	--TAB
	select lookup, (case when @UIType = 0 then ObjWinNamespace when @UIType = 1 then objWebNameSpace else objPDANamespace end) as [tabsource], dbo.GetCodeLookupDesc('DLGTABCAPTION', lookup, @UI) as [tabdesc], dbo.GetCodeLookupHelp('DLGTABCAPTION', lookup, @UI) as [tabhelp], O.objtype as [tabtype]
	from openxml(@handle, 'Config/Dialog/Tabs/Tab', 1) 
		with (
			lookup 		nvarchar(15),
			source 		varchar(100)
			)
	left join dbOMSObjects O on source = O.objcode
	
	if @handle <> 0
		exec sp_xml_removedocument @handle

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprConfigurableType] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprConfigurableType] TO [OMSAdminRole]
    AS [dbo];


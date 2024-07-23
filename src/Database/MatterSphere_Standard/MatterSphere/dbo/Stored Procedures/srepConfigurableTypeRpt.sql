

CREATE PROCEDURE [dbo].[srepConfigurableTypeRpt] (@OType nvarchar(30), @xml nvarchar(4000), @Code nvarchar(50), @Version nvarchar(20), @Active bit, @Desc nvarchar(100)) AS
	declare @handle int
	exec sp_xml_preparedocument @handle output, @xml
	--TAB
	insert into dbo.#OMSTypes
	(
		Otype, typeCode, typeVersion, typeActive, lookup, [tabsource], [tabtype], typeDesc
	)
	select @Otype, @Code AS typeCode, @Version AS typeVersion, @Active AS typeActive, lookup, ObjWinNamespace as [tabsource], O.objtype as [tabtype], @Desc
	from openxml(@handle, 'Config/Dialog/Tabs/Tab', 1) 
		with (
			lookup 		nvarchar(15),
			source 		varchar(100)
			)
	left join dbOMSObjects O on source = O.objcode
	
	exec sp_xml_removedocument @handle

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConfigurableTypeRpt] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConfigurableTypeRpt] TO [OMSAdminRole]
    AS [dbo];


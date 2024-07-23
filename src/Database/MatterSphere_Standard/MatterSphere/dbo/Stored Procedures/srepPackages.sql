

CREATE PROCEDURE [dbo].[srepPackages] 
(
	---@OType nvarchar(30), 
	@XML nvarchar(4000), 
	@CODE nvarchar(50), 
	@VERSION nvarchar(20), 
	@EXTERNAL bit---, 
	---@DESC nvarchar(100)
) 

AS

DECLARE @HANDLE int
exec sp_xml_preparedocument @handle output, @XML
--TAB
insert into dbo.#OMSTypes
(
	/*Otype, */pkgCode, pkgVersion, pkgExternal---, lookup, [tabsource], [tabtype], typeDesc
)
select 
	---@Otype, 
	@CODE AS pkgCode, 
	@VERSION AS pkgVersion, 
	@EXTERNAL AS pkgExternal
	---lookup, 
	---ObjWinNamespace as [tabsource], 
	---O.objtype as [tabtype], 
	---@DESC
from 
	openxml(@HANDLE, 'Config/Dialog/Tabs/Tab', 1) 
		with (
			lookup 		nvarchar(15),
			source 		varchar(100)
		     )
	left join dbOMSObjects O on source = O.objcode
	
	exec sp_xml_removedocument @HANDLE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPackages] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPackages] TO [OMSAdminRole]
    AS [dbo];


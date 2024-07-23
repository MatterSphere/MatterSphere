

CREATE PROCEDURE [dbo].[srepOMSPackages]
(
	@UI uUICultureInfo='{default}'---,
	---@findTypeCode nvarchar(30),
	---@findtabsource nvarchar(100)
)

AS

--Create the temporary table for the information to be input
CREATE TABLE [dbo].[#OMSTypes] 
	(
		---[OType] [nvarchar] (30),
		[pkgCode] [nvarchar] (50),
		[pkgVersion] [nvarchar] (50),
		[pkgExternal] [bit] NULL,
		[lookup] [nvarchar] (50)---,
		---[tabsource] [nvarchar] (50),
		---[tabtype] [nvarchar] (50),
		---[typeDesc] [nvarchar] (50) 
	) 
ON [PRIMARY]

---Declare variables
DECLARE @CODE nvarchar(50)
DECLARE @XML nvarchar(4000)
DECLARE @VERSION nvarchar(20)
DECLARE @EXTERNAL bit
---DECLARE @Desc nvarchar(100)
---DECLARE @OType nvarchar(30)

---Open Cursor to retrieve the fields
DECLARE OMS_Type cursor forward_only
	for SELECT /*'File' as OType, */P.pkgCode, P.pkgXML, P.pkgVersion, P.pkgExternal/*, dbo.GetCodeLookupDesc('FILETYPE', FT.typeCode, @UI) AS typeDesc */FROM dbPackages P
   	for read only

	open OMS_Type

		fetch from OMS_Type
		into /*@OType, */@CODE, @XML, @VERSION, @EXTERNAL---, @Desc

		while @@FETCH_STATUS = 0
		begin
			exec srepConfigurableTypeRpt /*@OType, */@XML, @CODE, @VERSION, @EXTERNAL---, @Desc
  			fetch next from OMS_Type
			into /*@OType, */@CODE, @XML, @VERSION, @EXTERNAL---, @Desc
		end

	close  OMS_Type
deallocate OMS_Type
/*
declare OMS_Type cursor forward_only
	for select 'Contact' as OType, CT.typeCode, CT.typeXML, CT.typeVersion, CT.typeActive, dbo.GetCodeLookupDesc('CONTTYPE', CT.typeCode, @UI) AS typeDesc from dbContactType CT
   	for read only

	open OMS_Type

		fetch from OMS_Type
		into @OType, @Code, @XML, @Version, @Active, @Desc

		while @@FETCH_STATUS = 0
		begin
			exec srepConfigurableTypeRpt @OType, @XML, @Code, @Version, @Active, @Desc
  			fetch next from OMS_Type
			into @OType, @Code, @XML, @Version, @Active, @Desc
		end

	close  OMS_Type
deallocate OMS_Type

declare OMS_Type cursor forward_only
	for select 'Client' as OType, CLT.typeCode, CLT.typeXML, CLT.typeVersion, CLT.typeActive, dbo.GetCodeLookupDesc('CLTYPE', CLT.typeCode, @UI) AS typeDesc from dbClientType CLT
   	for read only

	open OMS_Type

		fetch from OMS_Type
		into @OType, @Code, @XML, @Version, @Active, @Desc

		while @@FETCH_STATUS = 0
		begin
			exec srepConfigurableTypeRpt @OType, @XML, @Code, @Version, @Active, @Desc
  			fetch next from OMS_Type
			into @OType, @Code, @XML, @Version, @Active, @Desc
		end

	close  OMS_Type
deallocate OMS_Type
*/

--Get OMSType information from the temporary table
select 
	* 
from 
	dbo.#OMSTypes
---where
	---typeCode like coalesce(@findTypeCode, typeCode) + '%'
---and
	---tabsource like coalesce(@findtabsource, [tabsource]) + '%'
---order by
	---OType, typeCode
--Drop the temporary table before running the query
DROP TABLE
	[dbo].[#OMSTypes]

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepOMSPackages] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepOMSPackages] TO [OMSAdminRole]
    AS [dbo];


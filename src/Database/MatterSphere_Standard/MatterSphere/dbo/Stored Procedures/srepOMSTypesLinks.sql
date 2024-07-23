

CREATE PROCEDURE [dbo].[srepOMSTypesLinks]
(@UI uUICultureInfo='{default}',
@findTypeCode nvarchar(30),
@findtabsource nvarchar(100))

AS

--Create the temporary table for the information to be input
CREATE TABLE [dbo].[#OMSTypes] 
	(
		[OType] [nvarchar] (30),
		[typeCode] [nvarchar] (50),
		[typeVersion] [nvarchar] (50),
		[typeActive] [bit] NULL,
		[lookup] [nvarchar] (50),
		[tabsource] [nvarchar] (50),
		[tabtype] [nvarchar] (50),
		[typeDesc] [nvarchar] (50) 
	) 
ON [PRIMARY]

---Declare variables
DECLARE @Code nvarchar(50)
DECLARE @XML nvarchar(4000)
DECLARE @Version nvarchar(20)
DECLARE @Active bit
DECLARE @Desc nvarchar(100)
DECLARE @OType nvarchar(30)

---Open Cursor to retrieve the fields
declare OMS_Type cursor forward_only
	for select 'File' as OType, FT.typeCode, FT.typeXML, FT.typeVersion, FT.typeActive, dbo.GetCodeLookupDesc('FILETYPE', FT.typeCode, @UI) AS typeDesc from dbFileType FT
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

--Get OMSType information from the temporary table
select 
	* 
from 
	dbo.#OMSTypes
where
	typeCode like coalesce(@findTypeCode, typeCode) + '%'
and
	tabsource like coalesce(@findtabsource, [tabsource]) + '%'
order by
	OType, typeCode
--Drop the temporary table before running the query
DROP TABLE
	[dbo].[#OMSTypes]

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepOMSTypesLinks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepOMSTypesLinks] TO [OMSAdminRole]
    AS [dbo];


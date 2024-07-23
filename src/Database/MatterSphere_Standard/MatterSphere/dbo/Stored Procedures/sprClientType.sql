

CREATE PROCEDURE [dbo].[sprClientType]  (@Code uCodeLookup, @Version bigint = 0, @Force bit = 0, @UI uUICultureInfo = '{default}', @UIType tinyint = 0, @text ntext = '') AS

declare @curversion bigint

select @curversion = typeversion from dbclienttype where typecode = @Code

-- Check the version numbers.
if (@version < @curversion) or @force = 1
begin
	--HEADER
	select  *, dbo.GetCodeLookupDesc('CLTYPE', typecode, @UI) as [typedesc] from dbclienttype where typecode = @Code

	set @UI = replace(@UI, '''', '''''') 

	--Convert the list view column into a table from its base source of xml.
	--declare @handle int
	declare @xml_he int
	
	DECLARE @xml NVARCHAR(MAX) = (select typexml from dbclienttype where typecode = @code)

	
	DECLARE @handle int exec sp_xml_preparedocument @handle output,@xml declare he_cur cursor for select @handle
 	open he_cur 
	fetch he_cur into @xml_he
	deallocate he_cur
	
	-- Get the xml defintion.
	execute sprConfigurableType null, @UI,  @UIType, @xml_he
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientType] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientType] TO [OMSAdminRole]
    AS [dbo];


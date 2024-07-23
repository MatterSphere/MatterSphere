SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprFeeEarnerType]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sprFeeEarnerType]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sprFeeEarnerType]  (@Code uCodeLookup, @Version bigint = 0, @Force bit = 0, @UI uUICultureInfo = '{default}',  @UIType tinyint = 0) 
AS

declare @curversion bigint

select @curversion = typeversion from dbfeeearnertype where typecode = @Code

-- Check the version numbers.
if (@version < @curversion) or @force = 1
begin
	--HEADER
	select *, dbo.GetCodeLookupDesc('FEEEARNERTYPE', typecode, @UI) as [typedesc] from dbfeeearnertype where typecode = @Code
	
	--Convert the list view column into a table from its base source of xml.
	--declare @handle int
	declare @xml_he int
	
	DECLARE @xml NVARCHAR(MAX) = (select typexml from dbfeeearnertype where typecode = @code)
	
	DECLARE @handle int exec sp_xml_preparedocument @handle output,@xml declare he_cur cursor for select @handle
 	open he_cur 
	fetch he_cur into @xml_he
	deallocate he_cur
	
	-- Get the xml defintion.
	execute sprConfigurableType null, @UI,  @UIType, @xml_he
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFeeEarnerType] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFeeEarnerType] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[sprFieldListEx] (@code uCodeLookup, @UI uUICultureInfo = '{default}')
as
declare @xml nvarchar(4000)
declare @handle int
declare @FieldType nvarchar(2)

set @FieldType = '$$'

select @xml = schreturnfield from dbsearchlistconfig where schcode = @code

exec sp_xml_preparedocument @handle output, @xml


	select 
		@FieldType as [fldType], 
		upper(S.fldname) as [fldName], 
		null as [fldAlias], 
		null as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(coalesce(F.fldcode, S.fldname), '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', coalesce(F.fldcode, S.fldname), @UI) as [fldHelp]
	from openxml(@handle, 'fields/field', 1 ) 
		with (
			fldname	 nvarchar(15) '.'
		) S
	left outer join dbFields F on S.fldname =  coalesce(F.fldalias, F.fldname) and F.fldtype = @fieldtype and F.fldgroup = @code
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  coalesce(F.fldcode, S.fldname)
	where coalesce(F.fldexclude, 0) = 0 
	
	exec sp_xml_removedocument @handle

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFieldListEx] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFieldListEx] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[sprAssociateTypes] (@ContactType uCodeLookup, @UI uUICultureInfo = '{default}',  @IncludeNull bit = 1) AS

if @IncludeNull = 1
begin

      select null as typecode, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as typedesc , null as contType, null as assoctype
      union 
      select 'CLIENT' as typecode, dbo.GetCodeLookupDesc('SUBASSOC', 'CLIENT', @UI)  as typedesc, 'CLIENT', 'CLIENT'
      union
      select 'SOURCE' as typecode, dbo.GetCodeLookupDesc('SUBASSOC', 'SOURCE', @UI)  as typedesc, @ContactType, 'SOURCE'
      union
      select typecode, COALESCE(CL1.cdDesc, '~' + NULLIF(dbassociatetype.typecode, '') + '~') as typedesc, contType, assoctype
      from dbassociatetype
    inner join dbassociatedtypes on assoctype = typecode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'SUBASSOC', @UI ) CL1 ON CL1.[cdCode] =  dbassociatetype.typecode
      where conttype = @ContactType and not typecode in ('CLIENT', 'SOURCE')
      order by typedesc

end
else
begin

      select 'CLIENT' as typecode, dbo.GetCodeLookupDesc('SUBASSOC', 'CLIENT', @UI)  as typedesc, 'CLIENT' as contType, 'CLIENT' as assoctype
      union
      select 'SOURCE' as typecode, dbo.GetCodeLookupDesc('SUBASSOC', 'SOURCE', @UI)  as typedesc, @ContactType, 'SOURCE'
      union
      select typecode, COALESCE(CL1.cdDesc, '~' + NULLIF(dbassociatetype.typecode, '') + '~') as typedesc, contType, assoctype
      from dbassociatetype
    inner join dbassociatedtypes on assoctype = typecode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'SUBASSOC', @UI ) CL1 ON CL1.[cdCode] =  dbassociatetype.typecode
      where conttype = @ContactType and not typecode in ('CLIENT', 'SOURCE')
      order by typedesc

end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAssociateTypes] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAssociateTypes] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[sprCodeLookupListAddLink] (@Type uCodeLookup = null, @Code uCodeLookup = null, @UI uUICultureInfo = '{default}',  @IncludeNull bit = 0)
as
	if @IncludeNull = 0
	begin
		if @Type is null
			select cdaddlink, dbo.GetCodeLookupAddlinkDesc(cdType, cdCode, @UI, cdaddlink) as [cddesc], cdcode, cdgroup from dbcodelookup group by cdtype, cdcode, cdaddlink, cdgroup order by cdcode, cddesc
		else if @Code is null
			select cdaddlink, dbo.GetCodeLookupAddlinkDesc(@Type, cdCode, @UI, cdaddlink) as [cddesc], cdcode, cdgroup from dbcodelookup where cdtype = @Type  group by cdcode, cdaddlink, cdgroup order by cdcode, cddesc
		else
			select cdaddlink, dbo.GetCodeLookupAddlinkDesc(@Type, cdCode, @UI, cdaddlink) as [cddesc], cdcode, cdgroup from dbcodelookup where cdtype = @Type and cdcode = @Code group by cdcode, cdaddlink, cdgroup order by cdcode, cddesc
	end
	else
	begin
		if @Type is null
			select null as cdaddlink, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as cddesc, null as cdcode, null as cdgroup union select cdaddlink, dbo.GetCodeLookupAddlinkDesc(cdType, cdCode, @UI, cdaddlink) as [cddesc], cdcode, cdgroup from dbcodelookup group by cdtype, cdcode, cdaddlink, cdgroup order by cdcode, cddesc
		else if @Code is null
			select null as cdaddlink, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as cddesc, null as cdcode, null as cdgroup union select cdaddlink, dbo.GetCodeLookupAddlinkDesc(@Type, cdCode, @UI, cdaddlink) as [cddesc], cdcode, cdgroup from dbcodelookup where cdtype = @Type  group by cdcode, cdaddlink, cdgroup order by cdcode, cddesc
		else
			select null as cdaddlink, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as cddesc, null as cdcode, null as cdgroup union select cdaddlink, dbo.GetCodeLookupAddlinkDesc(@Type, cdCode, @UI, cdaddlink) as [cddesc], cdcode, cdgroup from dbcodelookup where cdtype = @Type and cdcode = @Code  group by cdcode, cdaddlink, cdgroup order by cdcode, cddesc
	end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupListAddLink] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupListAddLink] TO [OMSAdminRole]
    AS [dbo];


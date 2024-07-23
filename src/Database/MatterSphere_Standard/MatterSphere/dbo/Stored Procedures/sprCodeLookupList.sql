

CREATE PROCEDURE [dbo].[sprCodeLookupList] (@Type uCodeLookup = null, @Code uCodeLookup = null, @UI uUICultureInfo = '{default}', @Brief bit = 1, @IncludeNull bit = 0)
as
if @Brief = 1
begin
	if @UI is null
	begin
		if @Type is null
			select cduicultureinfo, cdcode, cddesc, cdhelp, cdaddlink, cduicultureinfo from dbcodelookup order by cddesc
		else if @Code is null
			select cduicultureinfo, cdcode, cddesc, cdhelp, cdaddlink, cduicultureinfo from dbcodelookup where cdtype = @Type order by cddesc
		else
			select cduicultureinfo, cdcode, cddesc, cdhelp, cdaddlink, cduicultureinfo from dbcodelookup where cdtype = @Type and cdcode = @Code
	end
	else
	begin
		if @IncludeNull = 0
		begin
			if @Type is null
				select cdcode, dbo.GetCodeLookupDesc(cdType, cdCode, @UI) as [cddesc], dbo.GetCodeLookupHelp(@Type, cdCode, @UI) as [cdhelp], cdaddlink, cdgroup from dbcodelookup group by cdtype, cdcode, cdaddlink, cdgroup order by cddesc
			else if @Code is null
				select cdcode, dbo.GetCodeLookupDesc(@Type, cdCode, @UI) as [cddesc], dbo.GetCodeLookupHelp(@Type, cdCode, @UI) as [cdhelp], cdaddlink, cdgroup from dbcodelookup where cdtype = @Type  group by cdcode, cdaddlink, cdgroup order by cddesc
			else
				select cdcode, dbo.GetCodeLookupDesc(@Type, cdCode, @UI) as [cddesc], dbo.GetCodeLookupHelp(@Type, cdCode, @UI) as [cdhelp], cdaddlink, cdgroup from dbcodelookup where cdtype = @Type and cdcode = @Code  group by cdcode, cdaddlink, cdgroup
		end
		else
		begin
			if @Type is null
				select null as cdcode, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as cddesc, null as cdhelp, null as cdaddlink, 0 as [cdgroup] union select cdcode, dbo.GetCodeLookupDesc(cdType, cdCode, @UI) as [cddesc], dbo.GetCodeLookupHelp(@Type, cdCode, @UI) as [cdhelp], cdaddlink, cdgroup from dbcodelookup group by cdtype, cdcode, cdaddlink, cdgroup order by cddesc
			else if @Code is null
				select null as cdcode, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as cddesc, null as cdhelp, null as cdaddlink, 0 as [cdgroup] union select cdcode, dbo.GetCodeLookupDesc(@Type, cdCode, @UI) as [cddesc], dbo.GetCodeLookupHelp(@Type, cdCode, @UI) as [cdhelp], cdaddlink, cdgroup from dbcodelookup where cdtype = @Type  group by cdcode, cdaddlink, cdgroup order by cddesc
			else
				select null as cdcode, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as cddesc, null as cdhelp, null as cdaddlink, 0 as [cdgroup] union select cdcode, dbo.GetCodeLookupDesc(@Type, cdCode, @UI) as [cddesc], dbo.GetCodeLookupHelp(@Type, cdCode, @UI) as [cdhelp], cdaddlink, cdgroup from dbcodelookup where cdtype = @Type and cdcode = @Code  group by cdcode, cdaddlink, cdgroup
		end
	end
end
else
begin
	if @Type is null
		select * from dbcodelookup order by cduicultureinfo
	else if @Code is null
		select * from dbcodelookup where cdtype = @Type order by cduicultureinfo
	else
		select * from dbcodelookup where cdtype = @Type and cdcode = @Code order by cduicultureinfo
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupList] TO [OMSAdminRole]
    AS [dbo];


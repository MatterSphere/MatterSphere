

CREATE PROCEDURE [dbo].[sprPrecCatList]
(@UI uUICultureInfo = '{default}', @IncludeNull bit = 0, @All bit = 0)
as

if (select object_id(N'[tempdb]..[##omsPrecedentCategory]')) is null
begin
	select distinct PrecCategory, cast(null as nvarchar(15)) as PrecType, cast(null as nvarchar(15)) as PrecLibrary into ##omsPrecedentCategory from dbo.dbPrecedents where PrecCategory <> '' 
	insert into ##omsPrecedentCategory select distinct PrecCategory, PrecType, PrecLibrary from dbo.dbPrecedents where PrecCategory <> ''
end


if @All = 1
begin 
	if @IncludeNull = 1 
	Begin
		select null as [precCategory], dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecCatDesc, null as PrecLibrary
		union
		select distinct  precCategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecCategory, '') + '~'), PrecLibrary from ##omsPrecedentCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECCAT', '' ) CL1 ON CL1.[cdCode] =  PrecCategory
	end
	else
	begin
		select distinct  PrecCategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecCategory, '') + '~'), PrecLibrary from ##omsPrecedentCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECCAT', '' ) CL1 ON CL1.[cdCode] =  PrecCategory
	end
end
else
begin
	
	if @IncludeNull = 1 
	Begin
		select null as PrecCategory, dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecCatDesc, null as PrecType, null as PrecLibrary
		union 
		select  PrecCategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecCategory, '') + '~'), PrecType, PrecLibrary from ##omsPrecedentCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECCAT', '' ) CL1 ON CL1.[cdCode] =  PrecCategory
	end
	else
	Begin
		select PrecCategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecCategory, '') + '~') AS PrecCatDesc, PrecType, PrecLibrary from ##omsPrecedentCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECCAT', '' ) CL1 ON CL1.[cdCode] =  PrecCategory
	End
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPrecCatList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPrecCatList] TO [OMSAdminRole]
    AS [dbo];


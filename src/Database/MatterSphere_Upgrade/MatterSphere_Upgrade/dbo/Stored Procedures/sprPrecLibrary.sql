CREATE PROCEDURE [dbo].[sprPrecLibrary] (@UI uUICultureInfo = '{default}', @IncludeNull bit = 0)
as

if (select object_id(N'[tempdb]..[##omsPrecedentLibrary]')) is null
begin
	select distinct PrecLibrary into ##omsPrecedentLibrary from dbo.dbPrecedents where PrecLibrary <> '' 
end


if @IncludeNull = 1 
Begin
	select null as PrecCategory, dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecLibDesc
	union 
	select PrecLibrary, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecLibrary, '') + '~')  from ##omsPrecedentLibrary
	LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECLIBRARY', '' ) CL1 ON CL1.[cdCode] =  PrecLibrary
end
else
Begin
	select PrecLibrary, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecLibrary, '') + '~') as PrecLibDesc from ##omsPrecedentLibrary
	LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECLIBRARY', '' ) CL1 ON CL1.[cdCode] =  PrecLibrary
End

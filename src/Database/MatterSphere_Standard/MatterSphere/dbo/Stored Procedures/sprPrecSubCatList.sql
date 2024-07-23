

CREATE PROCEDURE [dbo].[sprPrecSubCatList] (@UI uUICultureInfo = '{default}', @IncludeNull bit = 0, @All bit = 0)
as

if (select object_id(N'[tempdb]..[##omsPrecedentSubCategory]')) is null
begin
	select distinct 
    PrecSubCategory
	, PrecCategory
	, CASE d.O WHEN 1 THEN NULL ELSE PrecType END AS PrecType
	, CASE d.O WHEN 1 THEN NULL ELSE PrecLibrary END AS PrecLibrary
	into ##omsPrecedentSubCategory 
	from dbo.dbPrecedents 
	,( SELECT 1 AS O UNION ALL SELECT 2 ) AS d
	where PrecSubCategory <> '' 
	
end

if @All = 1
begin 
	if @IncludeNull = 1 
	Begin
		select null as [precSubCategory], dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecCatDesc, null as precCategory, null as PrecLibrary
		union
		select distinct  precsubcategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecSubCategory, '') + '~'), PrecCategory, PrecLibrary from ##omsPrecedentSubCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECSUBCAT', '' ) CL1 ON CL1.[cdCode] =  PrecSubCategory
	end
	else
	begin
		select distinct  PrecSubCategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecSubCategory, '') + '~'), PrecCategory, PrecLibrary from ##omsPrecedentSubCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECSUBCAT', '' ) CL1 ON CL1.[cdCode] =  PrecSubCategory
	end
end
else
begin
		
	if @IncludeNull = 1 
	Begin
		select null as PrecSubCategory, dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecCatDesc , null as precCategory, null as PrecType, null as PrecLibrary
		union 
		select PrecSubCategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecSubCategory, '') + '~'), PrecCategory, PrecType, PrecLibrary FROM ##omsPrecedentSubCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECSUBCAT', '' ) CL1 ON CL1.[cdCode] =  PrecSubCategory
	end
	else
	Begin
		select PrecSubCategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecSubCategory, '') + '~') as PrecCatDesc, PrecCategory, PrecType, PrecLibrary FROM ##omsPrecedentSubCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECSUBCAT', '' ) CL1 ON CL1.[cdCode] =  PrecSubCategory
	End
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPrecSubCatList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPrecSubCatList] TO [OMSAdminRole]
    AS [dbo];


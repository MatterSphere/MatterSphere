CREATE PROCEDURE [dbo].[sprPrecMinorCatList] (@UI uUICultureInfo = '{default}', @IncludeNull bit = 0, @All bit = 0)
as

if (select object_id(N'[tempdb]..[##omsPrecedentMinorCategory]')) is null
begin
	select distinct 
	PrecMinorCategory
	, PrecSubCategory
	, PrecCategory
	, CASE d.O WHEN 1 THEN NULL ELSE PrecType END AS PrecType
	, CASE d.O WHEN 1 THEN NULL ELSE PrecLibrary END AS PrecLibrary
	into ##omsPrecedentMinorCategory 
	from dbo.dbPrecedents 
	,( SELECT 1 AS O UNION ALL SELECT 2 ) AS d
	where PrecSubCategory <> ''  and PrecMinorCategory <> '' 
	
end

if @All = 1
begin 
	if @IncludeNull = 1 
	Begin
		select null as [precMinorCategory],
		dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecMinorCatDesc, 
		null as [precSubCategory],
		dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecSubCatDesc,
		null as precCategory, 
		null as PrecLibrary
		union
		select distinct  
			precminorcategory, 
			COALESCE(CL1.cdDesc, '~' + NULLIF(PrecMinorCategory, '') + '~'),
			precsubcategory,
			COALESCE(CL2.cdDesc, '~' + NULLIF(PrecSubCategory, '') + '~'), 
			PrecCategory, 
			PrecLibrary from ##omsPrecedentMinorCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECSUBCAT', '' ) CL2 ON CL2.[cdCode] =  PrecSubCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECMINORCAT', '' ) CL1 ON CL1.[cdCode] =  PrecMinorCategory
	end
	else
	begin
		select distinct 
			PrecMinorCategory, 
			COALESCE(CL1.cdDesc, '~' + NULLIF(PrecMinorCategory, '') + '~'),
			PrecSubCategory, 
			COALESCE(CL2.cdDesc, '~' + NULLIF(PrecSubCategory, '') + '~'), 
			PrecCategory, 
			PrecLibrary from ##omsPrecedentMinorCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECSUBCAT', '' ) CL2 ON CL2.[cdCode] =  PrecSubCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECMINORCAT', '' ) CL1 ON CL1.[cdCode] =  PrecMinorCategory
	end
end
else
begin
		
	if @IncludeNull = 1 
	Begin
		select 
		null as PrecMinorCategory, 
		dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecMinorCatDesc,
		null as PrecSubCategory, 
		dbo.GetCodeLookupDesc('RESOURCE', 'RESALL', @UI) as PrecSubCatDesc , 
		null as precCategory, 
		null as PrecType, 
		null as PrecLibrary
		union 
		select 
			PrecMinorCategory, 
			COALESCE(CL1.cdDesc, '~' + NULLIF(PrecMinorCategory, '') + '~'),
			PrecSubCategory,
			COALESCE(CL2.cdDesc, '~' + NULLIF(PrecSubCategory, '') + '~'), 
			PrecCategory, 
			PrecType, 
			PrecLibrary FROM ##omsPrecedentMinorCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECSUBCAT', '' ) CL2 ON CL2.[cdCode] =  PrecSubCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECMINORCAT', '' ) CL1 ON CL1.[cdCode] =  PrecMinorCategory
	end
	else
	Begin
		select 
			PrecMinorCategory, COALESCE(CL1.cdDesc, '~' + NULLIF(PrecMinorCategory, '') + '~') as PrecMinorCatDesc,
			PrecSubCategory, COALESCE(CL2.cdDesc, '~' + NULLIF(PrecSubCategory, '') + '~') as PrecSubCatDesc, PrecCategory, PrecType, PrecLibrary FROM ##omsPrecedentMinorCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECSUBCAT', '' ) CL2 ON CL2.[cdCode] =  PrecSubCategory
		LEFT JOIN dbo.GetCodeLookupDescription ( 'PRECMINORCAT', '' ) CL1 ON CL1.[cdCode] =  PrecMinorCategory
	End
end
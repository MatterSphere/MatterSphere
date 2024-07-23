CREATE PROCEDURE [dbo].[sprFieldList]  (@FieldType nvarchar(10), @UI uUICultureInfo = '{default}') 
AS

declare @table_name nvarchar(50)

if @FieldType = 'REG'
	set @table_name = 'dbRegInfo'
if @FieldType = 'BR'
	set @table_name = 'dbBranch'
else if @FieldType = 'USR'
	set @table_name = 'dbUser'
else if @FieldType = 'FEE'
	set @table_name = 'dbFeeEarner'
else if @FieldType = 'CONT'
	set @table_name = 'dbcontact'
else if @FieldType = 'COMP'
	set @table_name = 'dbcontactcompany'
else if @FieldType = 'IND'
	set @table_name = 'dbcontactindividual'
else if @FieldType = 'CL'
	set @table_name = 'dbclient'
else if @FieldType = 'CAD'
	set @table_name = 'dbaddclientinfo'
else if @FieldType = 'FILE'
	set @table_name = 'dbfile'
else if @FieldType = 'FAD'
	set @table_name = 'dbaddfileinfo'
else if @FieldType = 'ASSOC'
	set @table_name = 'dbassociates'
else if @FieldType = 'APP'
	set @table_name = 'dbappointments'
else if @FieldType = 'PH'
	set @table_name = 'dbfilephase'

if @fieldtype = '@@' -- SPECIFIC DATA
begin
	select @fieldtype as fldType, 
		(case when left(splookup, len(@FieldType)) = @FieldType then Upper(splookup) else upper(@FieldType + splookup) end) as [fldName], 
		upper(splookup) as [fldAlias], 
		CAST(null as nvarchar(50)) as [fldOld],
		MAX(COALESCE(CL1.cdDesc, '~' + NULLIF(splookup, '') + '~')) as [fldDesc],
		dbo.GetCodeLookupHelp('SPECIFICDATA', splookup, @UI) as [fldHelp],
		CAST(CASE WHEN LEFT(splookup, 2) = '__' THEN 1 ELSE 0 END as bit) as [fldHide],
		CAST(null as nvarchar(50)) as [fldFormat],
		CAST(null as nvarchar(15)) as [fldLookup]
	from dbspecificdata 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'SPECIFICDATA', @UI ) CL1 ON CL1.[cdCode] =  splookup
	group by splookup
end
else if @fieldtype = 'LOOKUP'
begin
	select 
		F.fldType as [fldType], 
		upper('#LOOKUP;' + fldname + ';' + fldLookup) as [fldName],
		upper('#LOOKUP;' + fldalias + ';' + fldLookup) as [fldAlias] ,
		upper('#LOOKUP;' + fldold + ';' + fldLookup)  as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(F.fldcode, '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', fldcode, @UI) as [fldHelp],
		fldhide as [fldHide],
		fldformat as [fldFormat],
		fldlookup as [fldLookup]
	from dbFields F 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  F.fldcode
	where fldlookup is not null and coalesce(F.fldexclude, 0) = 0 
end
else if @fieldtype = '$$' --EXTENDED DATA
begin
	select 
		F.fldType as [fldType], 
		upper(@fieldtype + fldgroup + '.' + fldextended + '.' + fldname) as [fldName], 
		upper(fldgroup + '.' + fldextended + '.' + fldname) as [fldAlias], 
		upper(F.fldOld) as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(F.fldcode, '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', fldcode, @UI) as [fldHelp],
		fldhide as [fldHide],
		fldformat as [fldFormat],
		fldlookup as [fldLookup]
		from dbFields F 
		LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  F.fldcode
	where fldtype = @fieldtype and coalesce(F.fldexclude, 0) = 0
end
else if @fieldtype = 'DATETIME' -- DATE TIME DATA
begin
	select 
		F.fldType as [fldType], 
		upper(fldname) as [fldName],
		upper(fldalias) as [fldalias],
		upper(fldold) as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(F.fldcode, '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', fldcode, @UI) as [fldHelp],
		fldhide as [fldHide],
		fldformat as [fldFormat],
		fldlookup as [fldLookup]
	from dbFields F 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  F.fldcode
	where (fldtype = @fieldtype or charindex(@fieldtype, fldgroup) > 0) and coalesce(F.fldexclude, 0) = 0
end
else if @FieldType = '.'	-- COMMON FIELDS
begin

	select 
		F.fldType,
		(case when left(F.fldName, len(F.fldType)) = F.fldType then Upper(F.fldName) else upper(F.fldType + F.fldName) end) as [fldName], 
		upper(F.fldalias) as [fldAlias], 
		upper(F.fldOld) as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(F.fldcode, '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', F.fldcode, @UI) as [fldHelp],
		F.fldhide as [fldHide],
		F.fldformat as [fldFormat],
		F.fldlookup as [fldLookup]
	from dbFields F
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  F.fldcode
	where F.fldexclude  = 0  and F.fldCommon = 1 and F.fldType <> '$$'
	union

	select 
		F.fldType as [fldType], 
		upper(F.fldType + fldgroup + '.' + fldextended + '.' + fldname) as [fldName], 
		upper(fldgroup + '.' + fldextended + '.' + fldname) as [fldAlias], 
		upper(F.fldOld) as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(F.fldcode, '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', fldcode, @UI) as [fldHelp],
		F.fldhide as [fldHide],
		F.fldformat as [fldFormat],
		F.fldlookup as [fldLookup]
	from dbFields F 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  F.fldcode
	where coalesce(F.fldexclude, 0) = 0 and F.fldCommon = 1 and F.fldType = '$$'
	union
	select 
		F.fldType as [fldType], 
		upper('#LOOKUP;' + fldname + ';' + fldLookup) as [fldName],
		upper('#LOOKUP;' + fldalias + ';' + fldLookup) as [fldAlias] ,
		upper('#LOOKUP;' + fldold + ';' + fldLookup)  as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(F.fldcode, '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', fldcode, @UI) as [fldHelp],
		fldhide as [fldHide],
		fldformat as [fldFormat],
		fldlookup as [fldLookup]
	from dbFields F 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  F.fldcode
	where fldlookup is not null and coalesce(F.fldexclude, 0) = 0  and F.fldCommon = 1
	order by fldname
	
end
else
begin

	select 
		@FieldType as [fldType], 
		(case when left(S.column_name, len(@FieldType)) = @FieldType then Upper(S.column_name) else upper(@FieldType + S.column_name) end) as [fldName], 
		upper(S.column_name) as [fldAlias], 
		upper(F.fldOld) as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(CAST(coalesce(F.fldCode, S.column_name) AS NVARCHAR(15)), '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', coalesce(fldCode, S.column_name), @UI) as [fldHelp],
		cast (coalesce(F.fldhide, 0) as bit) as [fldHide],
		F.fldformat as [fldFormat],
		F.fldlookup as [fldLookup]
	from information_schema.columns S
	left outer join dbFields F on S.column_name =  coalesce(F.fldalias, F.fldname) and F.fldType = @FieldType 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  CAST(coalesce(F.fldCode, S.column_name) AS NVARCHAR(15))
	where table_name = @table_name and coalesce(F.fldexclude, 0) = 0
	union
	select 
		F.fldType as [fldType], 
		(case when F.fldtype = '$$'then 
				upper(F.fldType + fldgroup + '.' + fldextended + '.' + fldname)
			when left(F.fldName, len(F.fldType)) = F.fldType then 
				Upper(F.fldName) else upper(F.fldType + F.fldName) 
			end) as [fldName], 
		(case when F.fldtype = '$$' then
				upper(fldgroup + '.' + fldextended + '.' + fldname) 
			else 
				upper(F.fldalias) 
			end) as [fldAlias], 
		upper(F.fldOld) as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(F.fldcode, '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', F.fldcode, @UI) as [fldHelp],
		F.fldhide as [fldHide],
		F.fldformat as [fldFormat],
		F.fldlookup as [fldLookup]
	from dbFields F
	left join information_schema.columns S on coalesce(F.fldalias, F.fldname) = S.column_name and S.table_name = @table_name
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  F.fldcode
	where (fldtype = @FieldType or charindex(@fieldtype, fldgroup) > 0) and S.column_name is null and coalesce(F.fldexclude, 0) = 0 
	union
	select 
		F.fldType as [fldType], 
		upper('#LOOKUP;' + fldname + ';' + fldLookup) as [fldName],
		upper('#LOOKUP;' + fldalias + ';' + fldLookup) as [fldAlias] ,
		upper('#LOOKUP;' + fldold + ';' + fldLookup)  as [fldOld],
		COALESCE(CL1.cdDesc, '~' + NULLIF(F.fldcode, '') + '~') as [fldDesc],
		dbo.GetCodeLookupHelp('FIELDS', fldcode, @UI) as [fldHelp],
		fldhide as [fldHide],
		fldformat as [fldFormat],
		fldlookup as [fldLookup]
	from dbFields F 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FIELDS', @UI ) CL1 ON CL1.[cdCode] =  F.fldcode
	where fldlookup is not null and coalesce(F.fldexclude, 0) = 0 and  (fldtype = @FieldType or charindex(@fieldtype, fldgroup) > 0) 
	order by fldname
	

end

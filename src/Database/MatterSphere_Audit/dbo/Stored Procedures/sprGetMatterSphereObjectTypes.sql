/*
	Return a list of MatterSphere Object types based on the table name passed in (e.g. dbFileType or dbClientType) and the
	associated code lookup group name to get the description (e.g. FILETYPE or CLTYPE)
*/

CREATE PROC [dbo].[sprGetMatterSphereObjectTypes] 
( 
	@UI nvarchar(15) = '{default}'
	, @TABLENAME nvarchar(200) 
	, @TYPECODE nvarchar(20) 
) AS

declare @SQL nvarchar(max) = ''

DECLARE @Types TABLE	(
							[typeCode] nvarchar(200),
							[typeCodeDesc] nvarchar(200)
						)

set @SQL = 'select 
				typeCode
				,  dbo.GetCodeLookupDesc(''' + @TYPECODE + ''', TypeCode, ''' + @UI + ''') as typeCodeDesc 
			from 
				' + @TABLENAME + ' 
			order by 
				typeCodeDesc asc'

insert into @Types execute (@sql)

select [typeCode], [typeCodeDesc] from @Types


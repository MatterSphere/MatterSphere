CREATE PROCEDURE [audit].[CreateAudit]
       @DB_NAME sysname = null,
       @SCHEMA_NAME sysname ,
       @TABLE_NAME sysname

AS
SET NOCOUNT ON
DECLARE @pkcount int = 0,
@sql nvarchar(max),
@createsql nvarchar(max),
@PKCol nvarchar(max),
@PKType nvarchar(max),
@PKvalueselect nvarchar(max),
@PKfieldselect nvarchar(max), 
@PKConstraintsql nvarchar(max),
@IUD_Type nvarchar(max),
@INSERT_AUDIT bit,
@UPDATE_AUDIT bit,
@DELETE_AUDIT bit,
@AUDID bigint

SET NOCOUNT ON

if not exists ( SELECT 1 FROM SYS.SYNONYMS where SCHEMA_NAME([schema_id]) = 'Audit' and name = @SCHEMA_NAME+'_'+@TABLE_NAME)
       set @createsql = 'CREATE SYNONYM [audit].'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)+' FOR '+case when @DB_NAME is null then '' else QUOTENAME(@DB_NAME)+'.' end+'Audit.'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)
else
       set @createsql = 'Drop SYNONYM [audit].'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)+'; CREATE SYNONYM [audit].'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)+' FOR '+case when @DB_NAME is null then '' else QUOTENAME(@DB_NAME)+'.' end+'Audit.'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)

exec (@createsql)

declare  @isql nvarchar(250) = '''insert '+case when @DB_NAME is null then '' else QUOTENAME(@DB_NAME)+'.' end+'[Audit].'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)+' (Type, '


select @createsql = 'IF NOT EXISTS(SELECT 1 FROM '+case when @DB_NAME is null then '' else QUOTENAME(@DB_NAME)+'.' end+'INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA= ''Audit'' AND TABLE_NAME= '''+@SCHEMA_NAME+'_'+@TABLE_NAME+''')
CREATE TABLE '+case when @DB_NAME is null then '' else QUOTENAME(@DB_NAME)+'.' end+'[Audit].'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)+
'(
AuditID [uniqueidentifier] DEFAULT (newid()),
Type char(1), 
'

declare @maxfield int = (select max(ORDINAL_POSITION) 
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @Table_Name
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME
and c.TABLE_SCHEMA = @SCHEMA_NAME)
;

if @maxfield is null
begin
raiserror('no PK on table %s.%s or table doesn''t exist', 16, -1, @Schema_Name,@Table_Name)
return
end

select @AUDID=acID,@INSERT_AUDIT = acInsert,@UPDATE_AUDIT = acUpdate,@DELETE_AUDIT = acDelete from  [audit].[Configuration] where acAuditTableName = case when @DB_NAME is null then '' else QUOTENAME(@DB_NAME)+'.' end+'[Audit].'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)
if @UPDATE_AUDIT is null
begin
INSERT into [audit].[Configuration] (acTableName,acAuditTableName,acInsert,acUpdate,acDelete,acRetentionPeriod,Created,CreatedBy,Updated,UpdatedBy)
VALUES (QUOTENAME(DB_NAME())+'.'+QUOTENAME(@SCHEMA_NAME)+'.'+QUOTENAME(@TABLE_NAME),QUOTENAME(@DB_NAME)+'.[Audit].'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME),1,1,1,30,GETDATE(),-1,GETDATE(),-1)
select @AUDID=acID,@INSERT_AUDIT = acInsert,@UPDATE_AUDIT = acUpdate,@DELETE_AUDIT = acDelete from  [audit].[Configuration] where acAuditTableName = case when @DB_NAME is null then '' else QUOTENAME(@DB_NAME)+'.' end+'[Audit].'+QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME)
--raiserror('no record in Audit.Configuration', 16, -1, @Schema_Name,@Table_Name)
--return
end


if @INSERT_AUDIT = 1 
       set @IUD_Type = 'INSERT'
if @UPDATE_AUDIT = 1
       set @IUD_Type = case when @IUD_Type is null then 'UPDATE' else @IUD_Type+' , UPDATE' end
if @DELETE_AUDIT = 1
       set @IUD_Type = case when @IUD_Type is null then 'DELETE' else @IUD_Type+' , DELETE' end

WHILE @maxfield > @pkcount 
BEGIN
       set @pkcount = @pkcount + 1

-- Get primary key columns for full outer join
       select @PKCOL=c.COLUMN_NAME ,@PKTYPE=case when sc.DATA_TYPE like '%char' then sc.DATA_TYPE+'('+cast(sc.CHARACTER_MAXIMUM_LENGTH as varchar(200))+')' else sc.DATA_TYPE end
       from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk join
       INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
       ON  c.TABLE_SCHEMA = pk.CONSTRAINT_SCHEMA
       and c.TABLE_NAME = pk.TABLE_NAME
       and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME 
       JOIN INFORMATION_SCHEMA.COLUMNS sc 
       ON  c.TABLE_SCHEMA = sc.TABLE_SCHEMA
       and c.TABLE_NAME = sc.TABLE_NAME
       and c.COLUMN_NAME = sc.COLUMN_NAME
       where pk.TABLE_NAME = @Table_Name
       and c.TABLE_SCHEMA = @SCHEMA_NAME
       and CONSTRAINT_TYPE = 'PRIMARY KEY'
       and c.ORDINAL_POSITION = @PKCOUNT

select @createsql = @createsql+@PKCOL+' '+@PKType+' ,'
select @isql = @isql + @PKCOL+' ,'
select @PKFIELDSELECT = coalesce(@PKFIELDSELECT + ' and', ' on') + ' i.' + @PKCOL + ' = d.' + @PKCOL
select @PKVALUESELECT = coalesce(@PKVALUESELECT + ' ,', '') +' CASE WHEN '''''' + @Type + '''''' = ''''D'''' THEN d.' + @PKCOL + ' else i.'+ @PKCOL +' end'
select @PKConstraintsql = coalesce(@PKConstraintsql + ' ,', 'CONSTRAINT [PK_AUDIT_'+@SCHEMA_NAME+'_'+@TABLE_NAME+'] PRIMARY KEY CLUSTERED 
(
') + '['+@PKCOL+'] ASC'


END

select @createsql = @createsql+
'
FieldName nvarchar(max), 
FriendlyName nvarchar(max), 
OldValue nvarchar(max), 
LinkedValue nvarchar(max),
NewValue nvarchar(max), 
UpdateDate datetime DEFAULT (GetDate()), 
AppName nvarchar(200),
UserID int,
UserName nvarchar(200)
'+@PKConstraintsql+', [AuditID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)'

select @isql = @isql + 'FieldName, FriendlyName, OldValue, LinkedValue, NewValue, UpdateDate, AppName, UserId, UserName)'

BEGIN
set @sql = 'IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'''+ @SCHEMA_NAME+'.'+ @SCHEMA_NAME+'_'+@TABLE_NAME+ '_ChangeTracking''))
                           DROP TRIGGER '+ QUOTENAME(@SCHEMA_NAME)+'.'+ QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME+ '_ChangeTracking')

Exec (@SQL)
print (@SQL)

SELECT @sql = 
'
create trigger ' + @SCHEMA_NAME+'_'+@TABLE_NAME+ '_ChangeTracking on '+@SCHEMA_NAME+'.'+ @TABLE_NAME+ ' WITH EXECUTE AS ''MSAUDITOR''  for '+ @IUD_TYPE + ' NOT FOR REPLICATION
as

SET NOCOUNT ON;

declare @bit int ,
@maxfield int ,
@field int ,
@char int ,
@fieldname nvarchar(128) ,
@sql nvarchar(max), 
@UpdateDate nvarchar(21) ,
@UserName nvarchar(128) ,
@Type char(1),
@rsql nvarchar(max),
@newvalue nvarchar(max),
@friendlyname nvarchar(max),
@linkvalue nvarchar(max),
@UserID int,
@AppName nvarchar(236)

-- date and user
SELECT @AppName=APP_NAME()
select @UserID = isnull( isnull((select [usrID] from [dbo].[dbuser] where [usrADID] = cast(CONTEXT_INFO() as  nvarchar(200))), (select [usrID] from [dbo].[dbuser] where [usrADID] = ORIGINAL_LOGIN())), -999)
select @UserName = isnull((select replace(name,'''''''','''''''''''') from [item].[user] where [NTLOGIN] = cast(CONTEXT_INFO() as  nvarchar(200))),ORIGINAL_LOGIN())

select @UpdateDate = convert(varchar(8), getdate(), 112) + '' '' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = ''U''
else
select @Type = ''I''
else
select @Type = ''D''

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '''+@Table_Name+''' AND TABLE_SCHEMA = '''+@SCHEMA_NAME + '''
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '''+@Table_Name + ''' AND TABLE_SCHEMA = '''+@SCHEMA_NAME + ''' and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in (''I'',''D'')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '''+@Table_Name + ''' AND TABLE_SCHEMA = '''+@SCHEMA_NAME + ''' and ORDINAL_POSITION = @field
set @newvalue = ''''
set @friendlyname = ''''

set @rsql = ''select @friendlyname= x.friendlyName,@linkvalue=REPLACE (x.LinkField Collate database_default,''''NEWVALUE'''', convert(nvarchar(max),i.'' + @fieldname + '')) from #ins i''
set @rsql = @rsql+'' join [audit].[ColumnNameMapping] x on x.TableName = ''''config_dbFile'''' and x.ColumnName = '''''' + @fieldname+''''''''

exec sp_executeSQl @rsql, N''@friendlyname nvarchar(max) output,@linkvalue nvarchar(max) output'', @friendlyname output ,@linkvalue output

if @Linkvalue is not null
       exec sp_executeSQl @linkvalue, N''@value nvarchar(max) output'', @newvalue output

       
select @sql = '+@isql+'''
select @sql = @sql + '' select '''''' + @Type + ''''''''
select @sql = @sql + '',' + isnull(@PKVALUESELECT,'')+'''
select @sql = @sql + '','''''' + @fieldname + ''''''''
select @sql = @sql + '',''''''+@friendlyname+''''''''
select @sql = @sql + '',convert(nvarchar(max),d.'' + @fieldname + '')''
select @sql = @sql + '',''''''+@newvalue+''''''''
select @sql = @sql + '',convert(nvarchar(max),i.'' + @fieldname + '')''
select @sql = @sql + '','''''' + @UpdateDate + ''''''''
select @sql = @sql + '','''''' + @AppName + ''''''''
select @sql = @sql + '','' + isnull(cast(@UserID as nvarchar(10)),''NULL'') + ''''
select @sql = @sql + '','''''' + @UserName + ''''''''
select @sql = @sql + '' from #ins i full outer join #del d''
select @sql = @sql + '' on i.rowguid = d.rowguid''
select @sql = @sql + '' where i.'' + @fieldname + '' <> d.'' + @fieldname 
select @sql = @sql + '' or (i.'' + @fieldname + '' is null and  d.'' + @fieldname + '' is not null)'' 
select @sql = @sql + '' or (i.'' + @fieldname + '' is not null and  d.'' + @fieldname + '' is null)'' 
EXECUTE AS LOGIN = ''MSAUDITOR''
exec (@sql)
REVERT;
end
end

'
end
EXECUTE AS LOGIN = 'MSAUDITOR'
Exec (@createsql)
REVERT;
EXEC(@sql)

Update [audit].[Configuration] 
set acSourceTableTriggerId = OBJECT_ID(QUOTENAME(@SCHEMA_NAME)+'.'+ QUOTENAME(@SCHEMA_NAME+'_'+@TABLE_NAME+ '_ChangeTracking'))
where @AUDID=acID


-- If Auditing is disabled, the trigger will need disabling too
declare @isAuditingEnabled bit = (select isnull(regAuditingEnabled, 0) from dbRegInfo)

if (@isAuditingEnabled = 0)
begin
       declare @sourceTableTriggerID bigint = (select acSourceTableTriggerID from [audit].[Configuration] where @AUDID = acID)

       if (@sourceTableTriggerID is not null and @sourceTableTriggerID != 0)
       begin
              declare @triggerName nvarchar(150) = (select name from sys.triggers where object_id = @sourceTableTriggerID)

              set @sql = 'DISABLE TRIGGER ' + @triggerName + ' ON ' + @SCHEMA_NAME + '.' + @TABLE_NAME 
              exec(@sql)
       end
end


select 0,0

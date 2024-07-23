/**************** STOP AND DISABLE *************/
/************ Stop and Disable Snapshot ********/
DECLARE @JOBNAME sysname;
Select @JOBNAME = name
from msdb..sysjobs a 
where category_id = 
(select category_id from msdb..syscategories where name = 'REPL-Snapshot') 
and a.name like '%'+DB_NAME()+'%'
IF @JOBNAME is null
	Select @JOBNAME = name
	from msdb..sysjobs a 
	where category_id = 
	(select category_id from msdb..syscategories where name = 'REPL-Merge') 
	and a.name like '%'+DB_NAME()+'%'
	select @jobname

IF EXISTS
(SELECT 1
FROM msdb.dbo.sysjobactivity ja 
LEFT JOIN msdb.dbo.sysjobhistory jh 
    ON ja.job_history_id = jh.instance_id
JOIN msdb.dbo.sysjobs j 
ON ja.job_id = j.job_id
JOIN msdb.dbo.sysjobsteps js
    ON ja.job_id = js.job_id
    AND ISNULL(ja.last_executed_step_id,0)+1 = js.step_id
WHERE ja.session_id = (SELECT TOP 1 session_id FROM msdb.dbo.syssessions ORDER BY agent_start_date DESC)
AND start_execution_date is not null
AND stop_execution_date is null
AND j.name = @JOBNAME)
begin
	Exec msdb..sp_stop_job @job_name = @JOBNAME;
end

IF @JOBNAME is not null
begin
Exec msdb..sp_update_job @job_name = @JOBNAME , @enabled = 0;
DISABLE TRIGGER [MSmerge_tr_alterschemaonly] ON DATABASE;
DISABLE TRIGGER [MSmerge_tr_altertable] ON DATABASE;
DISABLE TRIGGER [MSmerge_tr_altertrigger] ON DATABASE;
DISABLE TRIGGER [MSmerge_tr_alterview] ON DATABASE;
end;

IF  EXISTS (SELECT * FROM sys.triggers WHERE parent_class_desc = 'DATABASE' AND name = N'tgrSchemaChangeNotification')
	DISABLE TRIGGER [tgrSchemaChangeNotification] ON DATABASE;

--RUN THIS ON THE MATTERSPHERE DATABASE TO INSTALL AUDITING ON
--DO NOT RUN ON REPLICATED DATABASES
DECLARE @CHANGESQL nvarchar(max)
DECLARE @DROPCREATESQL nvarchar(max)
DECLARE @DROPSQL nvarchar(max)
DECLARE @TypeID int
DECLARE @NewTypeName sysname
DECLARE @ExistingTypeName sysname
DECLARE @NewTypeID int
DECLARE @rc int 

select @CHANGESQL = 'CREATE TYPE [dbo].'+QUOTENAME(t.name+'_2')+' FROM [nvarchar](max) NULL ; ' ,
@DROPCREATESQL = 'DROP TYPE [dbo].'+QUOTENAME(t.name)+'; CREATE TYPE [dbo].'+QUOTENAME(t.name)+' FROM [nvarchar](max) NULL ; ' ,
@DROPSQL = 'DROP TYPE [dbo].'+QUOTENAME(t.name+'_2')+' ; ' ,
 @TYPEID =t.user_type_id ,
 @NewTypeName = 'dbo.'+t.name+'_2',
 @ExistingTypeName = 'dbo.'+t.name
from sys.types t JOIN sys.types pt on t.system_type_id = pt.user_type_id
where t.is_user_defined = 1
and pt.name in ('ntext', 'text');

WHILE LEN(@CHANGESQL) > 0
BEGIN
	/****** CREATE DUMMY Type ************/
	EXEC sp_executesql @CHANGESQL

	set @CHANGESQL = ''
	SELECT @CHANGESQL = @CHANGESQL+CASE WHEN d.name is not null then  ' ALTER TABLE '+QUOTENAME(SCHEMA_NAME(t.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' DROP CONSTRAINT ' +d.name +' ;' else '' end+
		' ALTER TABLE '+QUOTENAME(SCHEMA_NAME(t.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' ALTER COLUMN '+ QUOTENAME(c.name)+ @NewTypeName + case when c.is_nullable = 1 then ' NULL' else ' NOT NULL ' end	+ ';' +
		CASE WHEN d.name is not null then  ' ALTER TABLE '+QUOTENAME(SCHEMA_NAME(t.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' ADD CONSTRAINT ' +d.name +' DEFAULT '+d.definition+ ' For '+c.name+';' else '' end
	FROM sys.columns AS c
	 INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
	 JOIN sys.tables ta on c.object_id = ta.object_id
	left JOIN sys.default_constraints AS D on d.object_id = c.default_object_id
	WHERE c.user_type_id = @TYPEID
	AND ta.type = 'U'; 

	/****** ALTER ALL Columns using wrong type to dummy type pause after to allow lazy writer to catch up ************/
	EXEC sp_executesql @CHANGESQL
	WAITFOR DELAY '00:00:02'

	set @CHANGESQL = ''
	SELECT 	@CHANGESQL=@CHANGESQL+'exec sp_refreshview N'''+QUOTENAME(SCHEMA_NAME(t.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+''';'
	FROM sys.columns AS c
	 INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
	 JOIN sys.views ta on c.object_id = ta.object_id
	left JOIN sys.default_constraints AS D on d.object_id = c.default_object_id
	WHERE c.user_type_id = @TYPEID
	AND ta.type = 'V'; 

	/******* Refresh all views which use tables changed to clear dependencies **********/
	EXEC sp_executesql @CHANGESQL


	/************* Change the current Type to nvarchar ************/
	EXEC sp_executesql @DROPCREATESQL


	select @TYPEID =t.user_type_id 
	from sys.types t 
	where 'dbo.'+t.name = @NewTypeName;

	set @CHANGESQL = ''
	SELECT @CHANGESQL = @CHANGESQL+
	CASE WHEN d.name is not null then  'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(t.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' DROP CONSTRAINT ' +d.name +' ;' else '' end+
		'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(t.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' ALTER COLUMN '+ QUOTENAME(c.name)+ @ExistingTypeName + case when c.is_nullable = 1 then ' NULL' else ' NOT NULL ' end	+ ';' +
		CASE WHEN d.name is not null then  'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(t.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' ADD CONSTRAINT ' +d.name +' DEFAULT '+d.definition+ ' For '+c.name+';' else '' end

	FROM sys.columns AS c
	 INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
	 JOIN sys.tables ta on c.object_id = ta.object_id
	left JOIN sys.default_constraints AS D on d.object_id = c.default_object_id
	WHERE c.user_type_id = @TYPEID
	AND ta.type = 'U'; 

	/******** Now Revert all tables back to correct datatype *****/
	EXEC sp_executesql @CHANGESQL
	WAITFOR DELAY '00:00:02'

	set @CHANGESQL = ''
	SELECT 	@CHANGESQL=@CHANGESQL+'exec sp_refreshview N'''+QUOTENAME(SCHEMA_NAME(t.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+''';'
	FROM sys.columns AS c
	 INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
	 JOIN sys.views ta on c.object_id = ta.object_id
	left JOIN sys.default_constraints AS D on d.object_id = c.default_object_id
	WHERE c.user_type_id = @TYPEID
	AND ta.type = 'V'; 


	/******** Again clean up views *****/
	EXEC sp_executesql @CHANGESQL


	/****** Remove Temp type ***********/
	EXEC sp_executesql @DROPSQL

	SET @CHANGESQL = NULL

	select @CHANGESQL = 'CREATE TYPE [dbo].'+QUOTENAME(t.name+'_2')+' FROM [nvarchar](max) NULL ; ' ,
	@DROPCREATESQL = 'DROP TYPE [dbo].'+QUOTENAME(t.name)+'; CREATE TYPE [dbo].'+QUOTENAME(t.name)+' FROM [nvarchar](max) NULL ; ' ,
	@DROPSQL = 'DROP TYPE [dbo].'+QUOTENAME(t.name+'_2')+' ; ' ,
	 @TYPEID =t.user_type_id ,
	 @NewTypeName = 'dbo.'+t.name+'_2',
	 @ExistingTypeName = 'dbo.'+t.name
	from sys.types t JOIN sys.types pt on t.system_type_id = pt.user_type_id
	where t.is_user_defined = 1
	and pt.name in ('ntext', 'text');
END
GO

/******************* Now the remaining tables text/ntext  *******************/
DECLARE @CHANGESQL nvarchar(max)
DECLARE @NewTypeName nvarchar(max)

set @NewTypeName = 'nvarchar(max)'

set @CHANGESQL = ''
SELECT @CHANGESQL = @CHANGESQL+CASE WHEN d.name is not null then  'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(ta.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' DROP CONSTRAINT ' +d.name +' ;' else '' end+
	'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(ta.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' ALTER COLUMN '+ QUOTENAME(c.name)+ @NewTypeName + case when c.is_nullable = 1 then ' NULL' else ' NOT NULL ' end	+ ';' +
	CASE WHEN d.name is not null then  'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(ta.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' ADD CONSTRAINT ' +d.name +' DEFAULT '+d.definition+ ' For '+c.name+';' else '' end

FROM sys.columns AS c
 INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
 JOIN sys.tables ta on c.object_id = ta.object_id
left JOIN sys.default_constraints AS D on d.object_id = c.default_object_id
WHERE t.name in ('text','ntext')
AND ta.type = 'U'; 


/********** change type on all tables using text and ntext directly **********/
EXEC sp_executesql @CHANGESQL

WAITFOR DELAY '00:00:05'

set @CHANGESQL = ''
SELECT 	@CHANGESQL='exec sp_refreshview N'''+QUOTENAME(SCHEMA_NAME(ta.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+''';'
FROM sys.columns AS c
 INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
 JOIN sys.views ta on c.object_id = ta.object_id
left JOIN sys.default_constraints AS D on d.object_id = c.default_object_id
WHERE t.name in ('text','ntext')
AND ta.type = 'V'; 


/********** change type on all views using tables changed **********/
EXEC sp_executesql @CHANGESQL

/******************* Now the remaining tables image  *******************/
set @NewTypeName = 'varbinary(max)'

set @CHANGESQL = ''
SELECT @CHANGESQL = @CHANGESQL+CASE WHEN d.name is not null then  'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(ta.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' DROP CONSTRAINT ' +d.name +' ;' else '' end+
	'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(ta.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' ALTER COLUMN '+ QUOTENAME(c.name)+ @NewTypeName + case when c.is_nullable = 1 then ' NULL' else ' NOT NULL ' end	+ ';' +
	CASE WHEN d.name is not null then  'ALTER TABLE '+QUOTENAME(SCHEMA_NAME(ta.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+' ADD CONSTRAINT ' +d.name +' DEFAULT '+d.definition+ ' For '+c.name+';' else '' end

FROM sys.columns AS c
 INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
 JOIN sys.tables ta on c.object_id = ta.object_id
left JOIN sys.default_constraints AS D on d.object_id = c.default_object_id
WHERE t.name in ('image')
AND ta.type = 'U'; 


/********** change type on all tables using text and ntext directly **********/
EXEC sp_executesql @CHANGESQL

WAITFOR DELAY '00:00:05'

set @CHANGESQL = ''
SELECT 	@CHANGESQL='exec sp_refreshview N'''+QUOTENAME(SCHEMA_NAME(ta.schema_id))+'.'+QUOTENAME(OBJECT_NAME(c.object_id))+''';'
FROM sys.columns AS c
 INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
 JOIN sys.views ta on c.object_id = ta.object_id
left JOIN sys.default_constraints AS D on d.object_id = c.default_object_id
WHERE t.name in ('image')
AND ta.type = 'V'; 


/********** change type on all views using tables changed **********/
EXEC sp_executesql @CHANGESQL;


/************ Get Job Details Again ******************/

DECLARE @JOBNAME sysname;
Select @JOBNAME = name
from msdb..sysjobs a 
where category_id = 
(select category_id from msdb..syscategories where name = 'REPL-Snapshot') 
and a.name like '%'+DB_NAME()+'%'
IF @JOBNAME is null
	Select @JOBNAME = name
	from msdb..sysjobs a 
	where category_id = 
	(select category_id from msdb..syscategories where name = 'REPL-Merge') 
	and a.name like '%'+DB_NAME()+'%'
	select @jobname


	
IF @JOBNAME is not null
begin
/************** ENABLE Triggers ************************/
ENABLE TRIGGER [MSmerge_tr_alterschemaonly] ON DATABASE;
ENABLE TRIGGER [MSmerge_tr_altertable] ON DATABASE;
ENABLE TRIGGER [MSmerge_tr_altertrigger] ON DATABASE;
ENABLE TRIGGER [MSmerge_tr_alterview] ON DATABASE;

/*************** Enable Job **************************/
Exec msdb..sp_update_job @job_name = @JOBNAME , @enabled = 1;

/*************** Start Job ***************************/

Exec msdb..sp_start_job @job_name = @JOBNAME;
end;

IF  EXISTS (SELECT * FROM sys.triggers WHERE parent_class_desc = 'DATABASE' AND name = N'tgrSchemaChangeNotification')
	ENABLE TRIGGER [tgrSchemaChangeNotification] ON DATABASE;
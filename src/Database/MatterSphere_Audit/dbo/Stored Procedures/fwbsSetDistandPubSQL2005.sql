CREATE PROCEDURE [dbo].[fwbsSetDistandPubSQL2005]
	 @mDistributor nvarchar (100) = NULL  -- defaults to Current Server name																														
	,@mDistDatabase nvarchar (50)	 = 'Distribution'																													
	,@mDataFolder nvarchar (150) = NULL -- defaults to the data directory for the SQL Server instance										
	,@mLogFolder nvarchar (150) = NULL -- defaults to the data directory for the SQL Server instance									
	,@mAlternateSnapshotFolder nvarchar (max) = NULL -- defaults to the snapshot directory for the SQL Server instance									
	,@mPublisher nvarchar (50) = NULL -- Defaults to Current Server name																											
	,@mWorkingDir nvarchar (150) = NULL -- Defaults to the ReplData folder for the SQL Server instance
	,@mPubDatabase nvarchar (50) = NULL -- defaults to Current Database
	,@udFlag bit = 1
	,@disableReplication bit = 0																	
																						

AS

SET NOCOUNT ON
SET XACT_ABORT ON

DECLARE
	 @LoginName nvarchar(150)
	,@snapshot_job_name nvarchar(300)		
	,@data_file nvarchar(100)
	,@log_file nvarchar(100)
	,@description nvarchar(300)
	,@JobNameScript nvarchar (200)
	,@idRangeManOption nvarchar(20)
	,@version varchar(300)
	,@pubCompatLevel varchar(10)
	,@schemaOptions varchar(20)
	,@SQL nvarchar(max)		
	,@JOBID uniqueidentifier
	,@CMD nvarchar(256)

-- Version Check
SET @version = ( SELECT @@version )
IF LEFT ( @version , 26 ) =  'Microsoft SQL Server  2000'
BEGIN
	RAISERROR ('This version of the procedure is designed to run against SQL Server 2005 or higher' , 10 , 1 )
	RETURN
END 

IF LEFT (@version , 25 ) = 'Microsoft SQL Server 2008'
BEGIN
	SET @pubCompatLevel = '100RTM'
	SET @schemaOptions =  '0x000000b208044fd1'
END

IF LEFT (@version , 25) = 'Microsoft SQL Server 2005'
BEGIN
	SET @pubCompatLevel = '90RTM'
	SET @schemaOptions = '0x08044FD1'
END

-- Set Variables

SET @snapshot_job_name = @mPublisher + '-' + @mDistDatabase + '-' + @mPubDatabase
IF @mDistributor IS NULL
	SET @mDistributor = ( SELECT @@ServerName )
IF @mPublisher IS NULL
	SET @mPublisher = ( SELECT @@ServerName )
IF @mPubDatabase IS NULL
	SET @mPubDatabase = ( SELECT db_name() )
SET @description =  N'Merge publication of ' + @mPubDatabase + ' database from Publisher ' + @mPublisher + ''

-- If @disablereplication is set to 1 this will disable replication for the specified database
IF @disableReplication = 1
BEGIN
	RAISERROR ( 'Disabling replication for database %s ..............' , 10 , 1 , @mPubDatabase ) WITH NOWAIT
	EXECUTE sp_replicationdboption @dbname = @mPubdatabase, @optname = N'merge publish', @value = N'fALSE'
	RAISERROR ( '' , 10 , 1 ) WITH NOWAIT -- line space only
	PRINT CHAR(13) +  'Replication disabled. You may need to manually remove any subscriptions at the Subscriber'
	RETURN
END

-- Check that the distribution database is not already configured
IF NOT EXISTS (SELECT  [Name] FROM Master.sys.Databases WHERE [is_distributor] = 1 AND [Name] = @mDistDatabase)
BEGIN
	RAISERROR ( 'Distribution database is %s' , 10 , 1 , @mDistDatabase ) WITH NOWAIT
	-- Add the Distribution database
	EXEC Master..sp_adddistributor  @distributor = @mDistributor 
	RAISERROR ( '' , 10 , 1 ) WITH NOWAIT -- line space only
	-- Add the default Agent profiles
	EXEC master..sp_MSupdate_agenttype_default @profile_id = 1
	EXEC master..sp_MSupdate_agenttype_default @profile_id = 2
	EXEC master..sp_MSupdate_agenttype_default @profile_id = 4
	EXEC master..sp_MSupdate_agenttype_default @profile_id = 6
	EXEC master..sp_MSupdate_agenttype_default @profile_id = 11
	-- Add the Distribution Database
	EXEC sp_adddistributiondb  @database = @mDistDatabase, @data_folder = @mDataFolder, @data_file_size = 50, @log_folder = @mLogFolder, @log_file_size = 10
	-- Configure the Publisher to use the Distribution database
	EXEC sp_adddistpublisher  @publisher = @mPublisher, @distribution_db = @mDistDatabase, @security_mode = 1, @working_directory = @mWorkingDir , @thirdparty_flag = 0
	RAISERROR ( '' , 10 , 1 ) WITH NOWAIT -- line space only	
END

-- Enable Merge replication if not already enabled
IF EXISTS ( SELECT [Name] FROM Master.sys.databases WHERE [is_merge_published] = 1 AND [Name] = @mPubDatabase)
BEGIN
	RAISERROR ( 'A publication for database %s already exists, terminating proceure' , 16 ,1 , @mPubDatabase )
	RETURN
END
ELSE
BEGIN
	EXEC sp_replicationdboption @dbname = @mPubDatabase, @optname = N'merge publish', @value = N'true'
	RAISERROR ( '' , 10 , 1 ) WITH NOWAIT -- line space only
	-- Create the publication
	set @SQL = QUOTENAME(@mPubDatabase)+'..sp_addmergepublication @publication = '''+@mPubDatabase+''', @description = '''+isnull(@description,'')+''', @retention = 30, @sync_mode = N''native'','+ case when @mAlternateSnapshotFolder is null then '' else '@snapshot_in_defaultfolder = False, @alt_snapshot_folder ='''+@mAlternateSnapshotFolder+''',' end  +' @conflict_logging = N''publisher'', @ftp_login = N''anonymous'', @keep_partition_changes = N''false'', @publication_compatibility_level = '''+@pubCompatLevel+''''

	exec (@SQL)
	--  Creates the Snapshot Agent for the publication
	set @SQL = QUOTENAME(@mPubDatabase)+'..sp_addpublication_snapshot 	@publication = '''+@mPubDatabase+''', @frequency_type = 4, @frequency_subday = 1, @frequency_subday_interval = 0, @active_start_time_of_day = 233000'

	exec (@SQL)
END

-- Grant logins for Publication access
DECLARE Login_Cursor CURSOR FOR
SELECT [Name] from Master..syslogins where sysadmin = 1 and isntuser = 0 or securityadmin = 1
OPEN Login_Cursor
FETCH NEXT FROM Login_Cursor INTO @LoginName
WHILE @@fetch_status = 0
BEGIN
	set @SQL = QUOTENAME(@mPubDatabase)+'..sp_grant_publication_access @publication = '''+@mPubDatabase+''', @login = '''+@LoginName+''''
	exec (@SQL)
	RAISERROR ( 'Adding login %s for Publication access' , 10 ,1 , @LoginName ) WITH NOWAIT
	FETCH NEXT FROM Login_Cursor INTO @LoginName
END
CLOSE Login_Cursor
DEALLOCATE Login_Cursor

-- Create temporary table to hold a list of all tables required for the publication with respective column id value and datatype
DECLARE @articles table								
	(
	[Name] nvarchar (50) NOT NULL,
	[system_type_id] tinyint,
	[is_identity] bit,
	[Schema] nvarchar(100)
	);

-- Populate temporary table with one row per article for the publication. 
	set @SQL='SELECT [Name] , system_type_id , is_identity , SchemaName FROM 
	(SELECT Row_Number()  OVER ( PARTITION BY TAB.[Object_id] ORDER BY COL.is_identity DESC ) as RowNumber , 
	Replace ( Replace ( TAB.[Name] , ''/'' , '''' ) , ''\'' , '''' ) as [Name] , 
	COL.system_type_id ,
	 COL.is_identity , 
	 SY.Name as SchemaName
	FROM ['+@mPubDatabase+'].sys.Columns COL 
	JOIN ['+@mPubDatabase+'].sys.Tables TAB ON TAB.[Object_id] = COL.[Object_id] 
	JOIN ['+@mPubDatabase+'].sys.Schemas SY ON SY.[Schema_id] = TAB.[schema_id] 
	WHERE TAB.type = ''U'' 
	AND TAB.is_ms_shipped = 0 
	AND TAB.[Name] NOT IN (''dbregInfo'' , ''dbState'' , ''sysUpgrade'',''Configuration'',''ColumnNameMapping'')) Articles WHERE RowNumber = 1'

INSERT @articles exec (@SQL)


IF @udFlag = 0 -- Excludes extended data tables
	DELETE @articles WHERE [Name] LIKE 'fd%' OR [Name] LIKE 'ud%'

-- This cursor scans the temporary table @articles and returns the data to the stored proc sp_addmergearticle, this in turn adds the articles to the publication.
DECLARE	@curPubName nvarchar (100) , @curName nvarchar(100) , @curIDrange int , @curSystype int , @curIsIdentity tinyint , @curThreshold nvarchar (10) , @curSchema nvarchar(100) , @article nvarchar(250)
DECLARE AddArticle_Cursor CURSOR FOR			
SELECT [Name] , [system_type_id] , [is_identity] , [Schema] FROM @articles  ORDER BY [Name]
OPEN AddArticle_Cursor
FETCH FROM AddArticle_Cursor INTO @curName, @curSysType , @curIsIdentity , @curSchema
WHILE @@fetch_status = 0 
BEGIN
	IF @curIsIdentity = 1
	BEGIN
	-- Case statement applies the appropriate identity range values depending on datatype
	SELECT @curIDrange = 																									
		CASE 
			WHEN @curSysType = 48 THEN 5 -- tinyint	
			WHEN @curSysType = 52 THEN 100 -- smallint
			WHEN @curSysType = 56 THEN 10000 -- int
			WHEN @curSysType = 127 THEN 50000 -- bigint
		END ,
		@curThreshold = 95 , @idRangeManOption = 'auto'
	END
	ELSE
	BEGIN
		SELECT @curIDrange =  NULL, @curThreshold = NULL , @idRangeManOption = 'none'
	END
	-- Create a unique article name
	SET @article = @curSchema + '-'+@curName
	-- Add the article to the publication
	set @SQL = QUOTENAME(@mPubDatabase)+'..sp_addmergearticle @publication = '''+@mPubDatabase+''' , @article = '''+@article+''' , @source_owner = '''+@curSchema+''', @source_object = '''+@curName+''' , @schema_option = '+@schemaOptions+', @column_tracking = ''true'' ,
			@destination_owner = '''+@curSchema+''' , @pub_identity_range = '+isnull(cast(@curIDrange as nvarchar(10)),'NULL')+' , @identity_range = '+isnull(cast(@curIDrange as nvarchar(10)),'NULL')+' , @threshold = '+Case when @curThreshold is null then 'NULL' else ''+@curThreshold+'' end +' , @stream_blob_columns = ''true'' ,  @identityRangeManagementOption ='''+ @idRangeManOption+''''
	Exec(@SQL)
	RAISERROR ( '' , 10 , 1 ) WITH NOWAIT
	RAISERROR ( 'Adding table [%s].[%s] to Publication %s' , 10 ,1 , @curSchema , @curName , @mPubDatabase ) WITH NOWAIT

	FETCH FROM AddArticle_Cursor INTO @curName, @curSysType , @curIsIdentity , @curSchema
END
CLOSE AddArticle_Cursor
DEALLOCATE AddArticle_Cursor

-- This script gets the name of the Snapshot job from sysjobs and passes it to the stored proc. sp_start_job to start the snapshot Agent
SET @JobNameScript = ( SELECT TOP 1 [Name] FROM [distribution].[dbo].[MSsnapshot_agents] WHERE publisher_db = @mPubDatabase ORDER BY [id] Desc )

set @JOBID = (select top 1 j.job_id from msdb..sysjobs j  where j.name = @JobNameScript)
select @CMD = '-Publisher '+QUOTENAME(@mPublisher)+' -PublisherDB '+QUOTENAME(@mPubDatabase)+' -Distributor '+QUOTENAME(@mDistributor)+' -Publication '+QUOTENAME(@mPubDatabase)+' -ReplicationType 2 -DistributorSecurityMode 1 '
-- This script adds a disable and enable of trigger around the snapshot
 
EXEC msdb.dbo.sp_delete_jobstep @job_id=@JOBID, @step_id=3
EXEC msdb.dbo.sp_delete_jobstep @job_id=@JOBID, @step_id=2
EXEC msdb.dbo.sp_delete_jobstep @job_id=@JOBID, @step_id=1
EXEC msdb.dbo.sp_add_jobstep @job_id=@JOBID, @step_name=N'Disable', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_fail_action=3, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'Disable Trigger [tgrSchemaChangeNotification] ON DATABASE', 
		@database_name=@mPubDatabase, 
		@flags=0
EXEC msdb.dbo.sp_add_jobstep @job_id=@JOBID, @step_name=N'Snapshot Agent startup message.', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_fail_action=3, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'sp_MSadd_snapshot_history @perfmon_increment = 0,  @agent_id = 96, @runstatus = 1, @comments = N''Starting agent.''', 
		@server=@mPublisher, 
		@database_name=N'distribution', 
		@flags=0
EXEC msdb.dbo.sp_add_jobstep @job_id=@JOBID, @step_name=N'Run agent.', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=4, 
		@on_success_step_id=5, 
		@on_fail_action=3, 
		@retry_attempts=10, 
		@retry_interval=1, 
		@os_run_priority=0, @subsystem=N'Snapshot', 
		@command=@CMD, 
		@server=@mPublisher, 
		@database_name=N'distribution', 
		@flags=0
EXEC msdb.dbo.sp_add_jobstep @job_id=@JOBID, @step_name=N'Detect nonlogged agent shutdown.', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=2, 
		@on_fail_action=2, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'sp_MSdetect_nonlogged_shutdown @subsystem = ''Snapshot'', @agent_id = 96', 
		@server=@mPublisher, 
		@database_name=N'distribution', 
		@flags=0
EXEC msdb.dbo.sp_add_jobstep @job_id=@JOBID, @step_name=N'Enable', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_fail_action=1, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'Enable trigger [tgrSchemaChangeNotification] ON DATABASE', 
		@database_name=@mPubDatabase, 
		@flags=0

EXEC [msdb].[dbo].[sp_start_job] @job_name = @JobNameScript
PRINT CHAR(13) + 'Job Name is ' + @JobNameScript 

/*

Execution example
===========

EXEC fwbsSetDistandPubSQL2005
		@mDistributor = 'FWBSWS33'
	, @mPubDatabase = 'OMSPub' 
	, @mDistDatabase = 'Distribution' 
	, @mDataFolder = 'C:\MSSQL\Data'
	, @mLogFolder = 'C:\MSSQL\Data'
	, @mAlternateSnapshotFolder = 'C:\MSSQL\Snapshot'
	, @mPublisher = 'FWBSWS33'
	, @mWorkingDir = 'C:\MSSQL\Repldata'
	, @udFlag = 1
*/

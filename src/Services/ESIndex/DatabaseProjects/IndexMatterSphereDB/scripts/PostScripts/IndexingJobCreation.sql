Print 'Starting SQLAgentJobs\IndexerJob.sql'

DECLARE @user_name NVARCHAR(250) = suser_sname()

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
	EXEC msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
END

DECLARE @db_name SYSNAME = DB_NAME()
	, @JOBNAME NVARCHAR(257)

SET @JOBNAME = @db_name + N' MatterCentre Indexer'

IF EXISTS (SELECT 1 FROM msdb.dbo.sysjobs_view WHERE name = @JOBNAME)
EXEC msdb.dbo.sp_delete_job @job_name = @JOBNAME

DECLARE @jobId BINARY(16)
EXEC msdb.dbo.sp_add_job @job_name = @JOBNAME, 
	@enabled = 0,
	@notify_level_eventlog = 0,
	@notify_level_email = 0,
	@notify_level_netsend = 0,
	@notify_level_page = 0,
	@delete_level = 0,
	@description = N'MS indexing', 
	@category_name = N'[Uncategorized (Local)]', 
	@owner_login_name = @user_name, 
	@job_id = @jobId OUTPUT

EXEC msdb.dbo.sp_add_jobstep @job_id = @jobId,
	@step_name = N'Indexing',
	@step_id = 1,
	@cmdexec_success_code = 0,
	@on_success_action = 1,
	@on_success_step_id = 0,
	@on_fail_action = 2,
	@on_fail_step_id = 0,
	@retry_attempts = 0,
	@retry_interval = 0,
	@os_run_priority = 0,
	@subsystem = N'TSQL',
	@command = N'EXEC search.RunIndexing',
	@database_name = @db_name,
	@flags = 0 

EXEC msdb.dbo.sp_add_jobschedule
	    @job_id = @jobId,
		@name=N'Indexing Daily', 
		@freq_type = 4, 
		@freq_interval = 1, 
		@freq_subday_type = 4, 
		@freq_subday_interval = 2, 
		@active_start_date=20170803, 
		@active_start_time=1000, 
		@active_end_time=235959

EXEC msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = @@SERVERNAME

GO

Print 'Starting NewInstallScripts\OMSPermissions.sql'

DECLARE @user_name nvarchar(250)
SET @user_name = suser_sname()

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
	EXEC msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
END


DECLARE @db_name sysname
SET @db_name = DB_NAME()

DECLARE @JOBNAME nvarchar(257)
SET @JOBNAME = @db_name+N' Matter Centre Permissions'


IF NOT EXISTS (SELECT 1 FROM msdb.dbo.sysjobs WHERE (name = @JOBNAME))
BEGIN
	DECLARE @jobId BINARY(16)
	EXEC msdb.dbo.sp_add_job @job_name=@JOBNAME, 
			@enabled=1, 
			@notify_level_eventlog=0, 
			@notify_level_email=0, 
			@notify_level_netsend=0, 
			@notify_level_page=0, 
			@delete_level=0, 
			@description=N'No description available.', 
			@category_name=N'[Uncategorized (Local)]', 
			@owner_login_name=@user_name, @job_id = @jobId OUTPUT

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=@db_name, 
			@step_id=1, 
			@cmdexec_success_code=0, 
			@on_success_action=1, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'
	EXECUTE dbo.fwbsGrantToOMSApplicationRole
	GO
	EXECUTE dbo.fwbsGrantToOMSRole
	GO
	EXECUTE dbo.fwbsGrantToOMSAdminRole
	GO', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1

	EXEC msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily', 
			@enabled=1, 
			@freq_type=4, 
			@freq_interval=1, 
			@freq_subday_type=1, 
			@freq_subday_interval=0, 
			@freq_relative_interval=0, 
			@freq_recurrence_factor=0, 
			@active_start_date=20090422, 
			@active_end_date=99991231, 
			@active_start_time=50000, 
			@active_end_time=235959

	EXEC msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = @@SERVERNAME
END
GO



Print 'SQLAgentJobs\ClientDeleteJob.sql'


DECLARE @user_name nvarchar(250)
SET @user_name = suser_sname()

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
	EXEC msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
END

DECLARE @db_name sysname
SET @db_name = DB_NAME()

DECLARE @JOBNAME nvarchar(257)
SET @JOBNAME = @db_name+N' MatterCentre Client Delete Routine'


IF NOT EXISTS (SELECT 1 FROM msdb.dbo.sysjobs_view WHERE name = @JOBNAME)
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

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Flag Record to Process', 
			@step_id=1, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_FlagRowToProcess', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactEmails', 
			@step_id=2, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbContactEmails', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactNumbers', 
			@step_id=3, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbContactNumbers', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactAddresses', 
			@step_id=4, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbContactAddresses', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbAppointments', 
			@step_id=5, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbAppointments', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbAddClientInfo', 
			@step_id=6, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbAddClientInfo', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbClientLinks', 
			@step_id=7, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbClientLinks', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbComplaints', 
			@step_id=8, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbComplaints', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbClientContacts', 
			@step_id=9, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbClientContacts', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbClient', 
			@step_id=10, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_dbClient', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Flag Record as Processed', 
			@step_id=11, 
			@cmdexec_success_code=0, 
			@on_success_action=1, 
			@on_success_step_id=0, 
			@on_fail_action=1, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ClientDelete_FlagRowAsProcessed', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily', 
			@enabled=1, 
			@freq_type=4, 
			@freq_interval=1, 
			@freq_subday_type=4, 
			@freq_subday_interval=5, 
			@freq_relative_interval=0, 
			@freq_recurrence_factor=0, 
			@active_start_date=20100511, 
			@active_end_date=99991231, 
			@active_start_time=190000, 
			@active_end_time=215959

	EXEC msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = @@SERVERNAME
END


GO

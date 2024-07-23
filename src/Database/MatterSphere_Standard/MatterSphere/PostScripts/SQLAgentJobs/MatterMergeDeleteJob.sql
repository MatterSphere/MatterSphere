Print 'Starting SQLAgentJobs\MatterMergeDeleteJob.sql'


DECLARE @user_name nvarchar(250)
SET @user_name = suser_sname()

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
	EXEC msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
END

DECLARE @db_name sysname
SET @db_name = DB_NAME()

DECLARE @JOBNAME nvarchar(257)
SET @JOBNAME = @db_name+N' MatterCentre Matter MergeDelete Routine'


IF  NOT EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = @JOBNAME)
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
			@command=N'EXECUTE dbo.MatterMergeDelete_FlagRowToProcess', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbAddFileInfo', 
			@step_id=2, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbAddFileInfo', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbFileLegal', 
			@step_id=3, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbFileLegal', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbDocumentLog', 
			@step_id=4, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbDocumentLog', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbDocumentStorage', 
			@step_id=5, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE MatterMergeDelete_dbDocumentStorage', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbDocumentVersionPreview', 
			@step_id=6, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbDocumentVersionPreview', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbDocumentVersion', 
			@step_id=7, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbDocumentVersion', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbDocumentPreview', 
			@step_id=8, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbDocumentPreview', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbTimeLedger', 
			@step_id=9, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbTimeLedger', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbTasks', 
			@step_id=10, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbTasks', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbDocument', 
			@step_id=11, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbDocument', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbAppointments', 
			@step_id=12, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbAppointments', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbComplaints', 
			@step_id=13, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbComplaints', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbFileEvents', 
			@step_id=14, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbFileEvents', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbFinancialLedger', 
			@step_id=15, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbFinancialLedger', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbKeyDtes', 
			@step_id=16, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbKeyDates', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbMilestone', 
			@step_id=17, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbFinancialLedger', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbAssociates', 
			@step_id=18, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbAssociates', 
			@database_name=@db_name, 
			@flags=0
			
	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbFileManagementAppData', 
			@step_id=19, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbFileManagementAppData', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbFile', 
			@step_id=20, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_dbFile', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Flag Record as Processed', 
			@step_id=21, 
			@cmdexec_success_code=0, 
			@on_success_action=1, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.MatterMergeDelete_FlagRowAsProcessed', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1

	EXEC msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily', 
			@enabled=1, 
			@freq_type=4, 
			@freq_interval=1, 
			@freq_subday_type=4, 
			@freq_subday_interval=10, 
			@freq_relative_interval=0, 
			@freq_recurrence_factor=0, 
			@active_start_date=20100511, 
			@active_end_date=99991231, 
			@active_start_time=190000, 
			@active_end_time=215959

	EXEC msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = @@SERVERNAME
END
GO
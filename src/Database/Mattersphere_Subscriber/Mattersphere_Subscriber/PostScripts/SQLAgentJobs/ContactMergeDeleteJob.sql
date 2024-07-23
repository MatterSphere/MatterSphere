Print 'Starting SQLAgentJobs\ContactMergeDeleteJob.sql'


DECLARE @user_name nvarchar(250)
SET @user_name = suser_sname()


IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
	EXEC msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
END

DECLARE @db_name sysname
SET @db_name = DB_NAME()

DECLARE @JOBNAME nvarchar(257)
SET @JOBNAME = @db_name+N' MatterCentre Contact MergeDelete Routine'

IF NOT EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = @JOBNAME)
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
			@command=N'EXECUTE dbo.ContactMergeDelete_FlagRowToProcess', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbAssociates', 
			@step_id=2, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbAssociates', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbAssociatesMulti', 
			@step_id=3, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbAssociatesMulti', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContact_Solicitors', 
			@step_id=4, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContact_Solicitors', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactCompany', 
			@step_id=5, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContactCompany', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactIndividual', 
			@step_id=6, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContactIndividual', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactLinks', 
			@step_id=7, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContactIndividual', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactReferral', 
			@step_id=8, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContactReferral', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactSecurity', 
			@step_id=9,
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContactSecurity', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactEmails', 
			@step_id=10, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContactEmails', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactNumbers', 
			@step_id=11, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContactNumbers', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContactAddresses', 
			@step_id=12, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContactAddresses', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbPerformance', 
			@step_id=13, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbPerformance', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbClientContacts', 
			@step_id=14, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbClientContacts', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbClient', 
			@step_id=15, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbClient', 
			@database_name=@db_name, 
			@flags=0
			
	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbFile', 
			@step_id=16, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbFile', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'dbContact', 
			@step_id=17, 
			@cmdexec_success_code=0, 
			@on_success_action=3, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_dbContact', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Flag Record as Processed', 
			@step_id=18, 
			@cmdexec_success_code=0, 
			@on_success_action=1, 
			@on_success_step_id=0, 
			@on_fail_action=2, 
			@on_fail_step_id=0, 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem=N'TSQL', 
			@command=N'EXECUTE dbo.ContactMergeDelete_FlagRowAsProcessed', 
			@database_name=@db_name, 
			@flags=0

	EXEC msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1

	EXEC msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily', 
			@enabled=1, 
			@freq_type=4, 
			@freq_interval=1, 
			@freq_subday_type=4, 
			@freq_subday_interval=5, 
			@freq_relative_interval=0, 
			@freq_recurrence_factor=0, 
			@active_start_date=20100512, 
			@active_end_date=20100512, 
			@active_start_time=190000, 
			@active_end_time=215959

	EXEC msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = @@SERVERNAME

END
ELSE
BEGIN
	DECLARE @jobId_Exist BINARY(16)
	SELECT @jobId_Exist = job_id FROM msdb.dbo.sysjobs_view WHERE name = @JOBNAME
	
	DECLARE @StepID Int
	IF EXISTS (SELECT step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocument')
	BEGIN
		Select @StepID =  step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocument'
		EXEC msdb.dbo.sp_delete_jobstep @job_id = @jobId_Exist, @step_id = @StepID
	END
	IF EXISTS (SELECT step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentPreview')
	BEGIN
		Select @StepID =  step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentPreview'
		EXEC msdb.dbo.sp_delete_jobstep @job_id = @jobId_Exist, @step_id = @StepID
	END
	IF EXISTS (SELECT step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentVersion')
	BEGIN
		Select @StepID =  step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentVersion'
		EXEC msdb.dbo.sp_delete_jobstep @job_id = @jobId_Exist, @step_id = @StepID	
	END
	IF EXISTS (SELECT step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentVersionPreview')
	BEGIN
		Select @StepID =  step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentVersionPreview'
		EXEC msdb.dbo.sp_delete_jobstep @job_id = @jobId_Exist, @step_id = @StepID	
	END
	IF EXISTS (SELECT step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentStorage')
	BEGIN
		Select @StepID =  step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentStorage'
		EXEC msdb.dbo.sp_delete_jobstep @job_id = @jobId_Exist, @step_id = @StepID	
	END
	IF EXISTS (SELECT step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentLog')
	BEGIN
		Select @StepID =  step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbDocumentLog'
		EXEC msdb.dbo.sp_delete_jobstep @job_id = @jobId_Exist, @step_id = @StepID	
	END
	IF EXISTS (SELECT step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbTimeLedger')
	BEGIN
		Select @StepID =  step_id FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId_Exist and Step_name = 'dbTimeLedger'
		EXEC msdb.dbo.sp_delete_jobstep @job_id = @jobId_Exist, @step_id = @StepID	
	END
END
GO









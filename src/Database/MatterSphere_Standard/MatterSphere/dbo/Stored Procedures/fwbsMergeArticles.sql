

CREATE PROCEDURE [dbo].[fwbsMergeArticles]
	@omsSwitch char(1) = NULL ,
	@omsPublication nvarchar(150) = NULL ,
	@omsArticle nvarchar(150) = NULL ,
	@omsNewField nvarchar(50) = NULL ,
	@omsFieldDesc nvarchar(500) = NULL 

AS

DECLARE 
	@omsThreshold tinyint ,
	@omsIdentityRange int ,
	@omsIdentityEnabled nchar(5) ,
	@omsSnapShotName nvarchar(250)

--IF @omsSwitch is NULL
BEGIN
	SELECT
		SO.[name] as Article ,
		CASE SO.[name] WHEN 'dbReginfo' THEN 'Replication not allowed' WHEN 'dbState' THEN 'Replication not allowed' ELSE coalesce ( MSP.[publication] , 'NONE' ) END as Publication ,
		SO.[replinfo]
	FROM
		dbo.[sysobjects] SO 
	LEFT JOIN 
		[Distribution].dbo.[MSarticles] MSA ON MSA.[article] = SO.[name] collate Latin1_General_CI_AS
	LEFT JOIN
		[Distribution].dbo.[MSPublications] MSP ON MSP.[publication_id] = MSA.[publication_id]
	WHERE 
		(SO.[name] LIKE 'db%' OR SO.[name] LIKE 'ud%' ) AND SO.[type] = 'U' 
	ORDER BY SO.[replinfo] desc , SO.[name]
END

IF @omsSwitch = '1'
-- add article to Publication
BEGIN
	-- Test for identity range and set Identity flag
	IF ( SELECT TOP 1 SC.[autoval] FROM syscolumns SC JOIN sysobjects SO on SC.[id] = SO.[id] WHERE SO.[name] = @omsArticle ORDER BY SC.[Autoval] DESC) IS NOT NULL
		SET @omsIdentityEnabled = 'true'
	ELSE
		SET @omsIdentityEnabled = 'false'

	-- If Identity ranges exist set the range value depending on datatype
	IF @omsIdentityEnabled  = 'true'
	BEGIN
		SET  @omsIdentityRange =
		CASE
		-- tinyint
		WHEN ( SELECT TOP 1 SC.[xtype] FROM syscolumns SC JOIN sysobjects SO on SC.[id] = SO.[id] WHERE SO.[name] = @omsArticle ORDER BY SC.[Autoval] DESC) = 48 THEN 25
		-- smallint
		WHEN ( SELECT TOP 1 SC.[xtype] FROM syscolumns SC JOIN sysobjects SO on SC.[id] = SO.[id] WHERE SO.[name] = @omsArticle ORDER BY SC.[Autoval] DESC) = 52 THEN 500
		-- int
		WHEN ( SELECT TOP 1 SC.[xtype] FROM syscolumns SC JOIN sysobjects SO on SC.[id] = SO.[id] WHERE SO.[name] = @omsArticle ORDER BY SC.[Autoval] DESC) = 56 THEN 250000
		--bigint
		WHEN ( SELECT TOP 1 SC.[xtype] FROM syscolumns SC JOIN sysobjects SO on SC.[id] = SO.[id] WHERE SO.[name] = @omsArticle ORDER BY SC.[Autoval] DESC) = 127 THEN 1000000
		END
	END

	-- If identity ranges are used set the threshold value
	IF @omsIdentityEnabled  = 'true'
		SET @omsThreshold = 95
	ELSE
		SET @omsThreshold = NULL

	EXEC dbo.[sp_addmergearticle]
		@publication = @omsPublication ,
		@article = @omsArticle , 
		@source_owner = N'dbo', 
		@source_object = @omsArticle ,
		@type = N'table', 
		@description = null , 
		@column_tracking = N'true', 
		@pre_creation_cmd = N'drop', 
		--@creation_script = null , 
		@article_resolver = null , 
		@subset_filterclause = null , 
		@vertical_partition = N'false', 
		@destination_owner = N'dbo', 
		@auto_identity_range = @omsIdentityEnabled , 
		@pub_identity_range = @omsIdentityRange , 
		@identity_range = @omsIdentityRange , 
		@threshold = @omsThreshold , 
		@verify_resolver_signature = 0 , 
		@allow_interactive_resolver = N'false' , 
		@fast_multicol_updateproc = N'true', 
		@check_permissions = 0 ,
		@force_invalidate_snapshot = 1 
	
	
END


IF @omsSwitch = '2'
BEGIN
	IF ( SELECT [sysadmin] FROM [Master].dbo.[syslogins] WHERE [name] = SUSER_SNAME() ) <> 1
	BEGIN
		RAISERROR ('Only logins with sysadmin rights can execute the snapshot agent' , 15 , 1 )
		RETURN
	END	
	ELSE
	BEGIN
		SET @omsSnapShotName = ( SELECT TOP 1 [name] from [Distribution].dbo.[MSsnapshot_agents] WHERE [publication] = @omsPublication )
		PRINT @omsSnapShotName
		EXEC [msdb].dbo.[sp_start_job] @job_name = @omsSnapShotName
	END
END


IF @omsSwitch = '3'
EXEC dbo.[fwbsAddColumn]
	@Tablename = @omsArticle ,
	@ColumnName = @omsNewField ,
	@ColumnDesc = @omsFieldDesc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsMergeArticles] TO [OMSAdminRole]
    AS [dbo];


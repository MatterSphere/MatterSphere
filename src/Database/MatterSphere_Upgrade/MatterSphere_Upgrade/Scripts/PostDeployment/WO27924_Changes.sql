SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[SystemPolicyConfig]') AND name = N'IX_SystemPolicyConfig_Byte_BitValue')
CREATE NONCLUSTERED INDEX [IX_SystemPolicyConfig_Byte_BitValue] ON [config].[SystemPolicyConfig]
(
	[Byte] ASC,
	[BitValue] ASC,
	[SecurableType] ASC,
	[Permission] ASC
)

GO

IF EXISTS(SELECT * FROM sys.procedures WHERE name = 'sprFileStatusRecord')
DROP PROCEDURE [dbo].[sprFileStatusRecord]
GO

CREATE PROCEDURE [dbo].[sprFileStatusRecord]
(@FILEID BIGINT = NULL)
AS
	BEGIN
SET NOCOUNT ON
	-- Performance May 2017 - Change to identify default schema
if exists (select top 1 1 from sys.synonyms where [schema_id] =schema_id('dbo') and base_object_name like '_config_.%')
	if @FILEID IS NOT NULL
		SELECT 
			F.FileID
			, F.fileStatus
			, FS.fsAccCode
			, FS.fsAlertLevel
			, FS.fsAppCreation
			, FS.fsAssocCreation
			, FS.fsDocModification
			, FS.fsTaskflowEdit
			, FS.fsTimeEntry
		FROM 
			config.DBFile F
			LEFT JOIN dbFileStatus FS ON FS.fsCode = F.fileStatus
		WHERE
			F.fileID = @FILEID
	else
		SELECT 
			F.FileID
			, F.fileStatus
			, FS.fsAccCode
			, FS.fsAlertLevel
			, FS.fsAppCreation
			, FS.fsAssocCreation
			, FS.fsDocModification
			, FS.fsTaskflowEdit
			, FS.fsTimeEntry
		FROM 
			config.DBFile F
			LEFT JOIN dbFileStatus FS ON FS.fsCode = F.fileStatus
else
	if @FILEID IS NOT NULL
		SELECT 
			F.FileID
			, F.fileStatus
			, FS.fsAccCode
			, FS.fsAlertLevel
			, FS.fsAppCreation
			, FS.fsAssocCreation
			, FS.fsDocModification
			, FS.fsTaskflowEdit
			, FS.fsTimeEntry
		FROM 
			DBFile F
			LEFT JOIN dbFileStatus FS ON FS.fsCode = F.fileStatus
		WHERE
			F.fileID = @FILEID
	ELSE
		SELECT 
			F.FileID
			, F.fileStatus
			, FS.fsAccCode
			, FS.fsAlertLevel
			, FS.fsAppCreation
			, FS.fsAssocCreation
			, FS.fsDocModification
			, FS.fsTaskflowEdit
			, FS.fsTimeEntry
		FROM 
			DBFile F
			LEFT JOIN dbFileStatus FS ON FS.fsCode = F.fileStatus
END
GO

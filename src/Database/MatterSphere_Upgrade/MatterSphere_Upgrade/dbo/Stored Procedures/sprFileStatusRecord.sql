CREATE PROCEDURE [dbo].[sprFileStatusRecord]
(@FILEID BIGINT = NULL)
AS
	BEGIN

	-- Performance May 2017 - Change to identify default schema
if exists (select top 1 1 from sys.synonyms where [schema_id] =schema_id('dbo') and base_object_name like '_config_.%')
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
		F.fileID = ISNULL(@FILEID, F.fileID)
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
		DBFile F
		LEFT JOIN dbFileStatus FS ON FS.fsCode = F.fileStatus
	WHERE
		F.fileID = ISNULL(@FILEID, F.fileID)

END

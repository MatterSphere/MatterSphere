CREATE PROCEDURE  [dbo].[sprClientRecord_Files]
	@CLID [bigint],
	@UI [uUICultureInfo] = '{default}',
	@RECORDCOUNT [bigint] = 0
AS

IF @RECORDCOUNT > 0
	BEGIN 
		SELECT TOP (@RECORDCOUNT)
			fileID
			, fileNo
			, filedesc
			, fileStatus
			, F.fileno + ' : ' + F.filedesc AS [fileJointDesc]
			, F.Created
			, F.filetype
			, F.phid
			, F.fileAlertMessage
			, F.fileAlertLevel
		FROM
		[config].[dbFile] F
		WHERE F.clID = @CLID
			AND fileStatus LIKE 'LIVE%'
			AND NOT EXISTS (SELECT TOP 1 1 FROM config.ClientFileAccess(@CLID) AS FA WHERE F.clID = FA.clid AND F.fileID = FA.FileID)
		ORDER BY Created DESC
	END
ELSE
	BEGIN
		SELECT
			fileID
			, fileNo
			, filedesc
			, fileStatus
			, F.fileno + ' : ' + F.filedesc AS [fileJointDesc]
			, F.Created
			, F.filetype
			, F.phid
			, F.fileAlertMessage
			, F.fileAlertLevel
		FROM
			[config].[dbFile] F
		WHERE F.clID = @CLID
			AND NOT EXISTS (SELECT TOP 1 1 FROM config.ClientFileAccess(@CLID) AS FA WHERE F.clID = FA.clid AND F.fileID = FA.FileID)
		ORDER BY Created DESC
 	END
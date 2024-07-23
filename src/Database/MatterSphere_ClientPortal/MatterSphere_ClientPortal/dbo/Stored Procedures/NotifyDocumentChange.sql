CREATE PROCEDURE [dbo].[NotifyDocumentChange]
	@IncludeExternalCreated SMALLINT,
	@IncludeInternalCreated SMALLINT
AS
BEGIN
SET NOCOUNT ON;

DECLARE @MsgSubjectInternal NVARCHAR(MAX);
SET @MsgSubjectInternal = (SELECT emailDocInternalChgSubjectMsg FROM eEmailMessageConfig)

DECLARE @MsgBodyInternal NVARCHAR(MAX);
SET @MsgBodyInternal = (SELECT emailDocInternalChgBodyMsg FROM eEmailMessageConfig)

DECLARE @MsgSubjectExternal NVARCHAR(MAX);
SET @MsgSubjectExternal = (SELECT emailDocExternalChgSubjectMsg FROM eEmailMessageConfig)

DECLARE @MsgBodyExternal NVARCHAR(MAX);
SET @MsgBodyExternal = (SELECT emailDocExternalChgBodyMsg FROM eEmailMessageConfig)

DECLARE @ProfileName NVARCHAR(255)
SET @ProfileName = dbo.GetSpecificData('MCPMAILPROFILE')

DECLARE @DOCID BIGINT, @USERID BIGINT, @USERNAME NVARCHAR(200), @FileNAME NVARCHAR(MAX), @ACCESSTYPE NVARCHAR(20), @IEMAILLIST NVARCHAR(MAX), @XEMAILLIST NVARCHAR(MAX), @SQL NVARCHAR(MAX), @SECOPTIONS SMALLINT, @INTSENT BIT

DECLARE @TblType TABLE (AccessType NVARCHAR(22))
IF @IncludeInternalCreated = 1
INSERT INTO @TblType VALUES ('INTERNAL')
IF @IncludeExternalCreated = 1
INSERT INTO @TblType VALUES ('EXTERNAL')

DECLARE Document_Cursor CURSOR FOR
SELECT [docID],u.usrID, u.usrFullName, u.AccessType, d.docDesc, d.securityoptions, docCreationEmailProcessed
FROM [config].[dbDocument] d
	JOIN [dbo].[dbUser] u ON u.usrid = d.Createdby
WHERE docDeleted = 0
	AND u.AccessType IN (SELECT AccessType FROM @TblType)
	AND ([docCreationEmailProcessed] = 0 AND [docCreationExtEmailProcessed] = 0)
ORDER BY d.Created ASC;

OPEN Document_Cursor;
FETCH NEXT FROM Document_Cursor INTO @DOCID, @USERID, @USERNAME, @ACCESSTYPE, @FileNAME, @SECOPTIONS, @INTSENT;

WHILE @@FETCH_STATUS = 0
BEGIN

	IF @INTSENT = 0
	BEGIN
		SELECT @IEMAILLIST=STUFF((SELECT '; ' + usrEmail
		FROM [dbo].[dbUser] DU
			JOIN [item].[User] IU ON IU.NTLogin = DU.usrADID 
		WHERE  (DU.[usrDocNotifyFeeEarnerManager] = 1)
			AND ISNULL(usrEmail,'') <> ''
			AND DU.usrID <> @USERID
			AND DU.AccessType = 'INTERNAL'
			AND EXISTS (SELECT 1 FROM config.dbfile f JOIN config.dbdocument d ON f.fileid = d.fileID WHERE d.docid = @DOCID AND (f.fileResponsibleID =  DU.usrID OR f.filePrincipleID = DU.usrID))
			AND (EXISTS (SELECT 1 FROM [config].[CheckDocumentAccess] (IU.NTLogin,@DocID) HAVING SUM(CAST(ISNULL(vallow,0) AS SMALLINT)) > 0 AND SUM(CAST(ISNULL(vdeny,0) AS SMALLINT)) = 0 )
				OR EXISTS (SELECT 1 FROM [config].[IsAdministratorTbl] (IU.NTLogin) WHERE isadmin=1))
		FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)') ,1,2,' ')

		IF @IEMAILLIST IS NOT NULL
		BEGIN
			DECLARE @internalBody NVARCHAR(MAX);

			SET @internalBody =
				'<!DOCTYPE html><html xmlns="http://www.w3.org/1999/xhtml" lang="en">' +
				N'<head><meta http-equiv="Content-Type" content="text/html;charset=utf-8"/><title>Client Portal</title></head>' +
				N'<body>' +
				N'' + (SELECT [dbo].[FormatEmailStringReplace] (@MsgBodyInternal, @DOCID)) + '<br/><br/>' +
				N'</body></html>';

			EXEC msdb..sp_send_dbmail @profile_name = @ProfileName,
					@blind_copy_recipients = @IEMAILLIST,
					@subject = @MsgSubjectInternal,
					@body_format = 'HTML',
					@body = @internalBody

		END
			
		UPDATE config.dbDocument SET docCreationEmailProcessed = 1 WHERE docid = @DOCID
	END

	IF @SECOPTIONS = 2
	BEGIN
		SELECT @XEMAILLIST=STUFF((SELECT '; ' + usrEmail
		FROM [dbo].[dbUser] DU
			JOIN [item].[User] IU ON IU.NTLogin = DU.usrADID 
		WHERE  DU.usrDocumentNotification = 1
			AND ISNULL(usrEmail,'') <> ''
			AND DU.usrID <> @USERID
			AND DU.AccessType = 'EXTERNAL' 
			AND (EXISTS (SELECT 1 FROM [config].[CheckDocumentAccess] (IU.NTLogin,@DocID) HAVING SUM(CAST(ISNULL(vallow,0) AS SMALLINT)) > 0 AND SUM(CAST(ISNULL(vdeny,0) AS SMALLINT)) = 0 )
				OR EXISTS (SELECT 1 FROM [config].[IsAdministratorTbl] (IU.NTLogin) WHERE isadmin=1))
		FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)') ,1,2,' ')

		IF @XEMAILLIST IS NOT NULL
		BEGIN
			DECLARE @externalBody NVARCHAR(MAX);

			SET @externalBody =
				'<!DOCTYPE html><html xmlns="http://www.w3.org/1999/xhtml" lang="en">' +
				N'<head><meta http-equiv="Content-Type" content="text/html;charset=utf-8"/><title>Client Portal</title></head>' +
				N'<body>' +
				N'' + @MsgBodyExternal + '<br/><br/>' +
				N'</body></html>';

			EXEC msdb..sp_send_dbmail @profile_name = @ProfileName,
					@blind_copy_recipients = @XEMAILLIST,
					@sensitivity = 'Confidential',
					@subject = @MsgSubjectExternal,
					@body_format = 'HTML',
					@body = @externalBody
		END

		UPDATE config.dbDocument SET docCreationExtEmailProcessed = 1 WHERE docid = @DOCID
	END

	FETCH NEXT FROM Document_Cursor INTO @DOCID, @USERID, @USERNAME, @ACCESSTYPE, @FileNAME, @SECOPTIONS, @INTSENT;
END;

CLOSE Document_Cursor;
DEALLOCATE Document_Cursor;

END

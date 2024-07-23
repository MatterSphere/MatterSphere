



CREATE PROCEDURE [dbo].[NotifyMCPPassWordChanged]
	@ID int,
	@Email nvarchar(250),
	@Run bit
AS
BEGIN

If @Run = 1

BEGIN

UPDATE dbo.dbUser SET usrMCPToken = NULL WHERE usrID = @ID

DECLARE @MsgSubject NVARCHAR(MAX);
SET @MsgSubject = (SELECT emailSuccessPWSubjectMsg FROM eEmailMessageConfig)

DECLARE @MsgBody NVARCHAR(MAX);
SET @MsgBody = (SELECT emailSuccessPWBodyMsg FROM eEmailMessageConfig)

DECLARE @tableHTML  NVARCHAR(MAX) ;

SET @tableHTML =
    '<!DOCTYPE html><html xmlns="http://www.w3.org/1999/xhtml" lang="en">' + 
	N'<head><meta http-equiv="Content-Type" content="text/html;charset=utf-8"/><title>Client Portal</title></head>' +
	N'<body>' +
	N'' + @MsgBody + '<br/><br/>' +
	N'</body></html>' ;

DECLARE @ProfileName NVARCHAR(255)
SET @ProfileName = dbo.GetSpecificData('MCPMAILPROFILE')

    EXEC [msdb].[dbo].sp_send_dbmail
      @profile_name = @ProfileName,
      @recipients = @Email,
      @subject = @MsgSubject,
	  @body_format = 'HTML',
      @body = @tableHTML

	  SELECT 0
END
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NotifyMCPPassWordChanged] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NotifyMCPPassWordChanged] TO [OMSAdminRole]
    AS [dbo];


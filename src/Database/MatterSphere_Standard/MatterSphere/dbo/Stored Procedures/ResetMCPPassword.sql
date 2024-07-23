

CREATE PROCEDURE [dbo].[ResetMCPPassword]

	@ID int,
	@Email nvarchar(250),
	@Host nvarchar(250),
	@Run bit
AS
BEGIN

IF @Run = 1

BEGIN

DECLARE @SMTP_DATE DATETIME
SET @SMTP_DATE = SYSUTCDATETIME()

DECLARE @MCP_Token uniqueidentifier
SET @MCP_Token = NEWID()

DECLARE @MsgSubject NVARCHAR(MAX);
SET @MsgSubject = (SELECT emailForgotPWReqSubjectMsg FROM eEmailMessageConfig)

DECLARE @MsgBody NVARCHAR(MAX);
SET @MsgBody = (SELECT emailForgotPWReqBodyMsg FROM eEmailMessageConfig)

DECLARE @MCPURL NVARCHAR(255)
SET @MCPURL = dbo.GetSpecificData('MCPURL')

DECLARE @tableHTML NVARCHAR(MAX);

SET @tableHTML =
    '<!DOCTYPE html><html xmlns="http://www.w3.org/1999/xhtml" lang="en">' + 
	N'<head><meta http-equiv="Content-Type" content="text/html;charset=utf-8"/><title>Client Portal</title></head><body>' +
	N'' + @MsgBody + '<br/><br/>' +
	N'<a href="' + @MCPURL +'ForgotPassword/ForgotPasswordReset?mcpt=' + CONVERT(varchar(255), @MCP_Token) +'">Click Here to Reset your Password</a>' +
	N'<br><br>If you are unable to click on the URL above, please copy and paste the following URL into your browser</br>' +
	N'<br>' + @MCPURL +'ForgotPassword/ForgotPasswordReset?mcpt=' + CONVERT(varchar(255), @MCP_Token) +
	N'</body></html>' ;

DECLARE @ProfileName NVARCHAR(255)
SET @ProfileName = dbo.GetSpecificData('MCPMAILPROFILE')

DECLARE @result INT
EXEC @result = [msdb].[dbo].sp_send_dbmail
      @profile_name = @ProfileName,
      @recipients = @Email,
      @subject = @MsgSubject,
	  @body_format = 'HTML',
      @body = @tableHTML

IF @result = 0
UPDATE dbo.dbUser SET usrMCPToken = CONVERT(varchar(255), @MCP_Token), usrMCPPWReset = @SMTP_DATE WHERE usrID = @ID

SELECT 0

END
END


SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ResetMCPPassword] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ResetMCPPassword] TO [OMSAdminRole]
    AS [dbo];


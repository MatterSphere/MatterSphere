Print 'Starting eEmailMessageConfigDefault.sql'

if not exists (select * from eEmailMessageConfig)
begin
	INSERT INTO [dbo].[eEmailMessageConfig](
		emailDocInternalChgSubjectMsg,
		emailDocInternalChgBodyMsg,
		emailDocExternalChgSubjectMsg,
		emailDocExternalChgBodyMsg,
		emailForgotPWReqSubjectMsg,
		emailForgotPWReqBodyMsg,
		emailSuccessPWSubjectMsg,
		emailSuccessPWBodyMsg)
	VALUES('New documents have been added to matters',
			'New documents have been added to matters',
			'Message from Law Firm',
			'New documents have been added to your matter(s) on the Client Portal by the law firm.<br/><br/>You can view these by logging into your account.',
			'Client Portal reset password request',
			'A request has been made to change your password. <br/><br/>If you did not make this request please call <b>number</b><br/><br/>If you requested a new password please follow the link below. This Link will remain active for 1 hour.<br/><br/>If not accessed within this time please request password again.',
			'Client Portal Password has been changed',
			'Your Client Portal password has changed. <br/><br/>If you did not request or authorise this, please call <b>number...</b>');
End



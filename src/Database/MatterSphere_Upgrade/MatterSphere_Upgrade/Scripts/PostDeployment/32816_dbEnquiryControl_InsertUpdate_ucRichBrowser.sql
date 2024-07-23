IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 101 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (101, 'ucRichBrowser', 'OMS', 0, 'FWBS.OMS.UI.UserControls.Browsers.ucRichBrowserControl,OMS.UI', NULL, NULL)
END
ELSE
BEGIN
	UPDATE [dbo].[dbEnquiryControl] SET 
		ctrlCode = 'ucRichBrowser', 
		ctrlGroup = 'OMS',
		ctrlSystem = 0,
		ctrlWinType='FWBS.OMS.UI.UserControls.Browsers.ucRichBrowserControl,OMS.UI'  
	WHERE 
		ctrlID = 101
END
GO
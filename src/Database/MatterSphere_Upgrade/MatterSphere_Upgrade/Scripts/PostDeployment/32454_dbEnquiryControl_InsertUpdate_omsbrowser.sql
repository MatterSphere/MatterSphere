IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 75 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (75, 'OMSBROWSER', 'OMS', 0, 'FWBS.Common.UI.Windows.eWebBrowser, EnquiryControls.WinUI', NULL, NULL)
END
ELSE
BEGIN
	UPDATE [dbo].[dbEnquiryControl] SET 
		ctrlCode = 'OMSBROWSER', 
		ctrlGroup = 'OMS',
		ctrlSystem = 0,
		ctrlWinType='FWBS.Common.UI.Windows.eWebBrowser, EnquiryControls.WinUI'  
	WHERE 
		ctrlID = 75
END
GO

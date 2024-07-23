IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 106 )
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (106, 'autosgstTextBox', 'OMS', 0, 'FWBS.OMS.UI.Windows.ucAutoSuggestTextBox,OMS.UI', NULL, NULL)
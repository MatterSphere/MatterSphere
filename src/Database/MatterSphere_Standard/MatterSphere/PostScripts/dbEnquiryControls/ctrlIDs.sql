Print 'Starting dbEnquiryControls\ctrlID.sql'


IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 20004 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 20004 , 'Frame2', 'General' , 0 , 'FWBS.Common.UI.Windows.eXPFrame2, EnquiryControls.WinUI' , NULL , NULL  )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 20005 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 20005 , 'eCaptionLine2', 'General' , 0 , 'FWBS.Common.UI.Windows.eCaptionLine2,EnquiryControls.WinUI' , NULL , NULL  )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 20006 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 20006 , 'eInformation2', 'General' , 0 , 'FWBS.Common.UI.Windows.eInformation2,EnquiryControls.WinUI' , NULL , NULL  )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 101 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (101, 'ucRichBrowser', 'OMS', 0, 'FWBS.OMS.UI.UserControls.Browsers.ucRichBrowserControl,OMS.UI', NULL, NULL)
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 20010 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (20010, 'MatterFldrsTree', 'OMS', 0, 'FWBS.OMS.UI.Windows.ucMatterFoldersTree,OMS.UI', NULL, NULL)
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 75 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 75 , 'OMSBROWSER', 'OMS' , 0 , 'FWBS.Common.UI.Windows.eWebBrowser, EnquiryControls.WinUI' , NULL , NULL  )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 79 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 79 , 'ApplySecUserPol', 'OMS' , 0 , 'FWBS.OMS.Addin.Security.Windows.ucPermissions, Fwbs.Oms.Addin.Security' , NULL , NULL  )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 82 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 82 , 'eEmbedFile' , 'OMS' , 0 , 'FWBS.OMS.UI.Windows.eEmbedFile,OMS.UI' , NULL , NULL )
END
GO


IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 85 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 85 , 'ucOMSTypeEmbed' , 'OMS' , 0 , 'FWBS.OMS.UI.Windows.ucOMSTypeEmbeded, OMS.UI' , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 86 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 86 , 'ucADImport' , 'OMS' , 0 , 'FWBS.OMS.UI.Windows.eADUserSelector, OMS.UI' , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 87 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES ( 87 , 'ucOMSTypeTabs' , 'OMS' , 0 , 'FWBS.OMS.UI.Windows.ucOMSTypeTabs, OMS.UI' , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 89 )
BEGIN
         INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
         VALUES ( 89 , 'XPCmbStLkup' , 'OMS' , 0 , 'FWBS.OMS.UI.Windows.eXPComboBoxStatesLookup, OMS.UI' , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 106 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (106, 'autosgstTextBox', 'OMS', 0, 'FWBS.OMS.UI.Windows.ucAutoSuggestTextBox,OMS.UI', NULL, NULL)
END
GO

IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 108 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (108, 'SearchConflicts', 'OMS', 0, 'FWBS.OMS.UI.Windows.ucSearchConflicts,OMS.UI', NULL, NULL)
END
GO
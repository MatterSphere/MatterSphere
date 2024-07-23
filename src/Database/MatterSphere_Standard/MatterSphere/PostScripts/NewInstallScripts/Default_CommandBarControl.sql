Print 'Starting NewInstallScripts\Default_CommandBarControl.sql'



-- Add default Commandbar controls
IF NOT EXISTS ( SELECT 'MAIN' , 'ABOUT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ABOUT' , '*' , 11 , 0 , NULL , 'msoControlButton' , 1 , NULL , 0 , 'ABOUT' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ACCOUNTS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ACCOUNTS' , '*' , 0 , 1 , 'EXTERNALAPPS' , 'msoControlButton' , 0 , 283 , 0 , 'SCRIPT;ViewAccounts' , 0 , NULL , 'Ispackageinstalled("PARAGONACC")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ACCSLIP' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ACCSLIP' , '*' , 0 , 2 , 'BILLGUIDE' , 'msoControlButton' , 0 , 2148 , 0 , 'TEMPLATESTART;ACCOUNTSSLIP;ASSOC' , 0 , NULL , 'Ispackageinstalled("ACCSLIP")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ADDCOMSIGNATURE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ADDCOMSIGNATURE' , '*' , 3 , 2 , 'SIGLOGO' , 'msoControlButton' , 0 , 31 , 0 , 'ADDSIGNATURE;COMPANY' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ADDFEESIGNATURE' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ADDFEESIGNATURE' , 'Word' , 2 , 2 , 'SIGLOGO' , 'msoControlButton' , 0 , 31 , 0 , 'ADDSIGNATURE;FEEEARNER' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ADDFIELD' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ADDFIELD' , '*' , 0 , 2 , 'ADMIN' , 'msoControlButton' , 0 , 213 , 0 , 'ADDFIELD' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ADDSLOGAN' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ADDSLOGAN' , '*' , 0 , 2 , 'SIGLOGO' , 'msoControlButton' , 0 , 565 , 0 , 'ADDSLOGAN' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ADDTASK' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ADDTASK' , '*' , 2 , 1 , 'OUTLOOK' , 'msoControlButton' , 0 , 68 , 0 , 'SCRIPT;NewOMSTask' , 0 , NULL , 'Ispackageinstalled("TASKS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ADMIN' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ADMIN' , '*' , 0 , 1 , 'TOOLS' , 'msoControlPopup' , 0 , NULL , 0 , 'ADMIN' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ANOLETTHEAD' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ANOLETTHEAD' , '*' , 3 , 2 , 'LETTERS' , 'msoControlButton' , 1 , 176 , 0 , 'SCRIPT;LetterheadPicker' , 0 , 'LETTHEADPICKER' , 'IsPackageInstalled("LETTHEADPICK")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'APPOINTMENT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'APPOINTMENT' , '*' , 0 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 33 , 0 , 'SCRIPT;NewAppointment' , 0 , NULL , 'Ispackageinstalled("APPOINTMENTS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ARCHIVE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ARCHIVE' , '*' , 1 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 225 , 0 , 'SCRIPT;NewArchive' , 0 , NULL , 'Ispackageinstalled("ARCHIVEDDOC")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ASSOCIATE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ASSOCIATE' , '*' , 2 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 176 , 0 , 'NEWASSOC' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'ATTNOTE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'ATTNOTE' , '*' , 0 , 2 , 'NOTES' , 'msoControlButton' , 0 , 2141 , 0 , 'TEMPLATESTART;ATTNOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'BILLGUIDE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'BILLGUIDE' , '*' , 2 , 1 , 'CREATE' , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'BLANKDOC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'BLANKDOC' , '*' , 0 , 2 , 'DOCUMENTS' , 'msoControlButton' , 0 , 112 , 0 , 'TEMPLATESTART;BLANK;ASSOC' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CHECKIN' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CHECKIN' , '*' , 0 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 0 , 1715 , 0 , 'LOCKDOC;IN' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CHECKOUT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CHECKOUT' , '*' , 1 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 0 , 2041 , 0 , 'LOCKDOC;OUT' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CLIENTINFO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CLIENTINFO' , '*' , 0 , 1 , 'OMSVIEW' , 'msoControlButton' , 0 , 64 , 0 , 'CLIENTINFO' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMMANDCENTRE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMMANDCENTRE' , '*' , 9 , 1 , 'OMSVIEW' , 'msoControlButton' , 0 , 176 , 0 , 'COMMANDCENTRE' , 0 , NULL , 'Ispackageinstalled("COMMANDCENTRE")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMPARE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMPARE' , '*' , 7 , 2 , 'DOCUMENTVW' , 'msoControlPopup' , 1 , 304 , 0 , NULL , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMPAREANY' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMPAREANY' , '*' , 5 , 3 , 'COMPARE' , 'msoControlButton' , 0 , 304 , 0 , 'COMPARE;ANY' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMPARELATEST' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMPARELATEST' , '*' , 0 , 3 , 'COMPARE' , 'msoControlButton' , 0 , 304 , 0 , 'COMPARE;LATEST' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMPARERECENT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMPARERECENT' , '*' , 4 , 3 , 'COMPARE' , 'msoControlButton' , 1 , 304 , 0 , 'COMPARE;RECENT' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMPAREVER' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMPAREVER' , '*' , 1 , 3 , 'COMPARE' , 'msoControlButton' , 0 , 304 , 0 , 'COMPARE;VERSION' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMPAREWITH' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMPAREWITH' , '*' , 2 , 3 , 'COMPARE' , 'msoControlButton' , 0 , 304 , 0 , 'COMPARE;FILE' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMPAREWITHCL' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMPAREWITHCL' , '*' , 3 , 3 , 'COMPARE' , 'msoControlButton' , 0 , 304 , 0 , 'COMPARE;CLIENT' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COMPLAINT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COMPLAINT' , '*' , 3 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 276 , 0 , 'SCRIPT;NewComplaint' , 0 , NULL , 'Ispackageinstalled("COMPLAINTS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CONFLICTSEARCH' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CONFLICTSEARCH' , '*' , 4 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 1714 , 0 , 'SCRIPT;ConflictSearch' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CONNECT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CONNECT' , '*' , 0 , 0 , NULL , 'msoControlButton' , 0 , 279 , 0 , 'CONNECT' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CONTMANAGER' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CONTMANAGER' , '*' , 10 , 1 , 'OMSVIEW' , 'msoControlButton' , 0 , 176 , 0 , 'CONTACTMANAGER' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COREREPORTS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COREREPORTS' , '*' , 0 , 2 , 'REPORTS' , 'msoControlButton' , 0 , 176 , 0 , 'SCRIPT;LoadCoreReports' , 0 , NULL , 'IsPackageInstalled("RPTCLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'COURTDOC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'COURTDOC' , '*' , 1 , 2 , 'DOCUMENTS' , 'msoControlButton' , 0 , 2152 , 0 , 'TEMPLATESTART;COURTDOC;ASSOC' , 0 , NULL , 'Ispackageinstalled("COURTDOC")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CREATE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CREATE' , '*' , 3 , 0 , NULL , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CURASSOCINFO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CURASSOCINFO' , '*' , 2 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 1 , 2152 , 0 , 'VIEWCURRENTASSOCIATE' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CURCLINFO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CURCLINFO' , '*' , 3 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 0 , 2152 , 0 , 'VIEWCURRENTCLIENT' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'CURFILEINFO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'CURFILEINFO' , '*' , 4 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 0 , 2152 , 0 , 'VIEWCURRENTFILE' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DATEWIZARD' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DATEWIZARD' , '*' , 5 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 265 , 0 , 'SCRIPT;CreateKeyDate' , 0 , NULL , 'Ispackageinstalled("KEYDATES")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DELETEFIELD' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DELETEFIELD' , '*' , 1 , 2 , 'ADMIN' , 'msoControlButton' , 0 , 21 , 0 , 'DELETEFIELD' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DISCONNECT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DISCONNECT' , '*' , 1 , 0 , NULL , 'msoControlButton' , 0 , 2151 , 0 , 'DISCONNECT' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DOCGETLATEST' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DOCGETLATEST' , '*' , 5 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 1 , 1709 , 0 , 'GETLATESTVERSION' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DOCRECEIPT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DOCRECEIPT' , '*' , 4 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 591 , 0 , 'TEMPLATESTART;RECEIPT;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DOCSAVE&CONT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DOCSAVE&CONT' , '*' , 9 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 1 , 3 , 0 , 'SAVE;CONTINUE' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DOCUMENTS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DOCUMENTS' , '*' , 3 , 1 , 'CREATE' , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DOCUMENTVW' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DOCUMENTVW' , 'Word' , 2 , 1 , 'OMSVIEW' , 'msoControlPopup' , 1 , 1561 , 0 , NULL , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DOCVERSIONS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DOCVERSIONS' , '*' , 6 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 0 , 2522 , 0 , 'OPEN;VERSION' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'DRAFTDOC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'DRAFTDOC' , '*' , 5 , 2 , 'SIGLOGO' , 'msoControlButton' , 1 , 29 , 0 , 'ADDLOGO;DRAFTIMAGE' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'EMAILSUPPORT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'EMAILSUPPORT' , '*' , 5 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 274 , 0 , 'SCRIPT;EmailSupport' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'EXTERNAL1' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'EXTERNAL1' , '*' , 2 , 1 , 'EXTERNALAPPS' , 'msoControlButton' , 0 , 281 , 0 , 'SCRIPT;UnderConstruction' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'EXTERNAL2' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'EXTERNAL2' , '*' , 3 , 1 , 'EXTERNALAPPS' , 'msoControlButton' , 0 , 68 , 0 , 'SCRIPT;UnderConstruction' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'EXTERNALAPPS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'EXTERNALAPPS' , '*' , 7 , 0 , NULL , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'FILE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'FILE' , '*' , 2 , 2 , 'NOTES' , 'msoControlButton' , 1 , 176 , 0 , 'TEMPLATESTART;FILENOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'FILECOPY' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'FILECOPY' , '*' , 7 , 2 , 'SIGLOGO' , 'msoControlButton' , 1 , 176 , 0 , 'ADDLOGO;FILECOPYIMAGE' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'FILEINFO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'FILEINFO' , '*' , 1 , 1 , 'OMSVIEW' , 'msoControlButton' , 0 , 2152 , 0 , 'FILEINFO' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'FINSTATEMENT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'FINSTATEMENT' , '*' , 2 , 2 , 'BILLGUIDE' , 'msoControlButton' , 0 , 2148 , 0 , 'TEMPLATESTART;FINSTATEMENT;ASSOC' , 0 , NULL , 'Ispackageinstalled("ACCSLIP")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'INTRANET' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'INTRANET' , '*' , 1 , 1 , 'EXTERNALAPPS' , 'msoControlButton' , 0 , 29 , 0 , 'SCRIPT;ViewIntranet' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'INVOICING' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'INVOICING' , '*' , 3 , 2 , 'BILLGUIDE' , 'msoControlButton' , 0 , 283 , 0 , 'TEMPLATESTART;INVOICE;ASSOC' , 0 , NULL , 'Ispackageinstalled("BILLING")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'JOBLIST' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'JOBLIST' , '*' , 8 , 1 , 'OMSVIEW' , 'msoControlButton' , 1 , 2152 , 0 , 'SCRIPT;ViewJobList' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'LETTERS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'LETTERS' , '*' , 0 , 1 , 'CREATE' , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'MEETING' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'MEETING' , '*' , 1 , 2 , 'NOTES' , 'msoControlButton' , 0 , 2141 , 0 , 'TEMPLATESTART;MEETINGNOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'MILESTONEINFO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'MILESTONEINFO' , '*' , 6 , 1 , 'OMSVIEW' , 'msoControlButton' , 1 , 798 , 0 , 'SCRIPT;ViewMilestones' , 0 , NULL , 'Ispackageinstalled("MILESTONES")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'MSWEBSITE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'MSWEBSITE' , '*' , 5 , 1 , 'EXTERNALAPPS' , 'msoControlButton' , 0 , 123 , 0 , 'SCRIPT;ViewMicrosoftWeb' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWAPPOINTMENT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWAPPOINTMENT' , '*' , 3 , 1 , 'OUTLOOK' , 'msoControlButton' , 0 , 176 , 0 , 'SCRIPT;NewAppointment' , 0 , NULL , 'Ispackageinstalled("APPOINTMENTS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWCLIENT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWCLIENT' , '*' , 5 , 1 , 'CREATE' , 'msoControlButton' , 1 , 213 , 0 , 'NEWCLIENT' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWCOMPSLIP' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWCOMPSLIP' , '*' , 10 , 1 , 'CREATE' , 'msoControlButton' , 1 , 6 , 0 , 'TEMPLATESTART;COMPSLIP;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWCONTACT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWCONTACT' , '*' , 7 , 1 , 'CREATE' , 'msoControlButton' , 0 , 213 , 0 , 'SCRIPT;CreateContact' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWENTRY' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWENTRY' , '*' , 4 , 1 , 'CREATE' , 'msoControlPopup' , 1 , NULL , 0 , NULL , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWENTRYMAIL' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWENTRYMAIL' , '*' , 13 , 0 , 'NEWENTRY' , 'msoControlButton' , 0 , 2188 , 0 , 'TEMPLATESTART;EMAIL;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWFAX' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWFAX' , '*' , 8 , 1 , 'CREATE' , 'msoControlButton' , 1 , 1707 , 0 , 'TEMPLATESTART;FAX ;ASSOC' , 0 , NULL , 'Ispackageinstalled("FAX")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWFILE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWFILE' , '*' , 6 , 1 , 'CREATE' , 'msoControlButton' , 0 , 213 , 0 , 'NEWFILE' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWMAIL' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWMAIL' , '*' , 3 , 0 , 'OUTLOOK' , 'msoControlButton' , 1 , 2188 , 0 , 'TEMPLATESTART;EMAIL;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWMAIL' , 'Outlook' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWMAIL' , 'Outlook' , 8 , 0 , NULL , 'msoControlButton' , 1 , 2188 , 0 , 'TEMPLATESTART;EMAIL;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWMEMO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWMEMO' , '*' , 9 , 1 , 'CREATE' , 'msoControlButton' , 0 , 274 , 0 , 'TEMPLATESTART;MEMO;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWPRECLIDOC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWPRECLIDOC' , '*' , 12 , 1 , 'CREATE' , 'msoControlButton' , 1 , 281 , 0 , 'SCRIPT;NewPreClient' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWPRECORPCLI' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWPRECORPCLI' , '*' , 13 , 1 , 'CREATE' , 'msoControlButton' , 0 , 281 , 0 , 'SCRIPT;CreatePreClientCorporate' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NEWTIME' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NEWTIME' , '*' , 11 , 1 , 'CREATE' , 'msoControlButton' , 0 , 2146 , 0 , 'SCRIPT;NewTime' , 0 , NULL , 'Ispackageinstalled("TIMERECORDING")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NOTE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NOTE' , '*' , 7 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 593 , 0 , 'TEMPLATESTART;FILENOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'NOTES' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'NOTES' , '*' , 1 , 1 , 'CREATE' , 'msoControlPopup' , 1 , NULL , 0 , 'NOTES' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'OMSVIEW' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'OMSVIEW' , '*' , 2 , 0 , NULL , 'msoControlPopup' , 1 , NULL , 0 , NULL , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'OPENDOC' , 'Outlook' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'OPENDOC' , 'Outlook' , 4 , 0 , NULL , 'msoControlButton' , 1 , 176 , 0 , 'OPEN' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'OUTLOOK' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'OUTLOOK' , '*' , 5 , 0 , NULL , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'PRECEDENTS' , '*;!Outlook' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'PRECEDENTS' , '*;!Outlook' , 7 , 0 , NULL , 'msoControlButton' , 1 , 2144 , 0 , 'PRECEDENTS;ACTIVE' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'PRECEDENTS' , '*;!Word,!Excel' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'PRECEDENTS' , '*;!Word,!Excel' , 10 , 0 , NULL , 'msoControlButton' , 1 , 2144 , 0 , 'PRECEDENTS;MAIN' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'REFRESHFIELDS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'REFRESHFIELDS' , '*' , 6 , 1 , 'TOOLS' , 'msoControlButton' , 1 , 279 , 0 , 'REFRESHFIELDS' , 0 , NULL , 'Ispackageinstalled("DMS")' , '<NULL>' )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'RELINKFIELDS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'RELINKFIELDS' , '*' , 6 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 279 , 0 , 'RELINKFIELDS' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'REMOTEINFO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'REMOTEINFO' , '*' , 7 , 1 , 'OMSVIEW' , 'msoControlButton' , 0 , 2152 , 0 , 'SCRIPT;ViewRemoteInfo' , 0 , NULL , 'Ispackageinstalled("RemoteAccount")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'REMOVEDRAFT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'REMOVEDRAFT' , '*' , 6 , 2 , 'SIGLOGO' , 'msoControlButton' , 0 , 34 , 0 , 'REMOVELOGO;DRAFTIMAGE' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'REMOVEFILECOPY' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'REMOVEFILECOPY' , '*' , 8 , 2 , 'SIGLOGO' , 'msoControlButton' , 0 , 34 , 0 , 'REMOVELOGO;FILECOPYIMAGE' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'REMOVESIG' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'REMOVESIG' , '*' , 4 , 2 , 'SIGLOGO' , 'msoControlButton' , 0 , 34 , 0 , 'REMOVESIG' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'REPORTS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'REPORTS' , '*' , 1 , 1 , 'TOOLS' , 'msoControlPopup' , 0 , NULL , 0 , 'REPORTS' , 0 , NULL , 'IsPackageInstalled("RPTCLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'RISKASSESSMENT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'RISKASSESSMENT' , '*' , 8 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 214 , 0 , 'TEMPLATESTART;RISKASSESSMENT;CLIENT' , 0 , NULL , 'Ispackageinstalled("RISKASS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'SAVEALL' , 'Outlook' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'SAVEALL' , 'Outlook' , 9 , 0 , NULL , 'msoControlButton' , 1 , 271 , 0 , 'SAVEALL' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'SAVEASPREC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'SAVEASPREC' , '*' , 3 , 2 , 'ADMIN' , 'msoControlButton' , 1 , 271 , 0 , 'SAVEASPREC' , 0 , 'PRECEDIT' , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'SHOWDOCVARS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'SHOWDOCVARS' , '*' , 2 , 2 , 'ADMIN' , 'msoControlButton' , 0 , 283 , 0 , 'SHOWDOCVARS' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'SIGLOGO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'SIGLOGO' , '*' , 2 , 1 , 'TOOLS' , 'msoControlPopup' , 0 , NULL , 0 , 'SIGLOGO' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'SIGNDOC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'SIGNDOC' , '*' , 1 , 2 , 'SIGLOGO' , 'msoControlButton' , 1 , 31 , 0 , 'SIGNDOCUMENT' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'SMS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'SMS' , '*' , 9 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 568 , 0 , 'TEMPLATESTART;SMS;ASSOC' , 0 , NULL , 'IsPackageInstalled("SMS")
islicensedfor("SMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TASK' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TASK' , '*' , 10 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 1561 , 0 , 'SCRIPT;NewOMSTask' , 0 , NULL , 'Ispackageinstalled("TASKS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TELIN' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TELIN' , '*' , 3 , 2 , 'NOTES' , 'msoControlButton' , 1 , 275 , 0 , 'TEMPLATESTART;TELEPHONENOTEIN;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TELOUT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TELOUT' , '*' , 4 , 2 , 'NOTES' , 'msoControlButton' , 0 , 275 , 0 , 'TEMPLATESTART;TELEPHONENOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TEMPLATEPROP' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TEMPLATEPROP' , 'Word' , 4 , 2 , 'ADMIN' , 'msoControlButton' , 0 , 283 , 0 , 'TEMPLATEPROP' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TIMEENTRY' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TIMEENTRY' , '*' , 11 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 2146 , 0 , 'SCRIPT;NewTime' , 0 , NULL , 'IsLicensedFor("TIMEREC")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TIMEREPORT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TIMEREPORT' , '*' , 2 , 2 , 'REPORTS' , 'msoControlButton' , 0 , 2152 , 0 , 'SCRIPT;ViewTimeReport' , 0 , NULL , 'Ispackageinstalled("RPTTIMERECORDIN")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TOASSOC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TOASSOC' , '*' , 1 , 2 , 'LETTERS' , 'msoControlButton' , 0 , 176 , 0 , 'TEMPLATESTART;LETTERHEAD;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TOCLIENT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TOCLIENT' , '*' , 0 , 2 , 'LETTERS' , 'msoControlButton' , 0 , 176 , 0 , 'TEMPLATESTART;LETTERHEAD;CLIENT' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'TOOLS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'TOOLS' , '*' , 6 , 0 , NULL , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'UFNINFO' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'UFNINFO' , '*' , 11 , 1 , 'OMSVIEW' , 'msoControlButton' , 1 , 176 , 0 , 'UFNINFORMATION' , 0 , '' , 'IsPackageInstalled("LEGALAID")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'UNDERTAKING' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'UNDERTAKING' , '*' , 12 , 2 , 'NEWENTRY' , 'msoControlButton' , 0 , 362 , 0 , 'SCRIPT;NewUndertaking' , 0 , NULL , 'Ispackageinstalled("UNDERTAKINGS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'UNDODOC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'UNDODOC' , '*' , 8 , 2 , 'DOCUMENTVW' , 'msoControlButton' , 0 , 1716 , 0 , 'LOCKDOC;UNDO' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'USERSETTINGS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'USERSETTINGS' , '*' , 3 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 64 , 0 , 'USERSETTINGS' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'VIEWCALENDAR' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'VIEWCALENDAR' , '*' , 5 , 1 , 'OUTLOOK' , 'msoControlButton' , 1 , 800 , 0 , 'VIEWCALENDAR;USER' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'VIEWFEECALENDAR' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'VIEWFEECALENDAR' , '*' , 6 , 1 , 'OUTLOOK' , 'msoControlButton' , 0 , 800 , 0 , 'VIEWCALENDAR;FEEEARNER' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'VIEWFEETASKS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'VIEWFEETASKS' , '*' , 10 , 1 , 'OUTLOOK' , 'msoControlButton' , 0 , 1561 , 0 , 'VIEWTASKS;FEEEARNER' , 0 , NULL , 'Ispackageinstalled("TASKS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'VIEWINBOX' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'VIEWINBOX' , '*' , 4 , 1 , 'OUTLOOK' , 'msoControlButton' , 1 , 1589 , 0 , 'VIEWINBOX;USER' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'VIEWOMSTASKS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'VIEWOMSTASKS' , '*' , 8 , 1 , 'OUTLOOK' , 'msoControlButton' , 1 , 1561 , 0 , 'SCRIPT;ViewOMSTasks' , 0 , NULL , 'Ispackageinstalled("TASKS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'VIEWOTHCALENDAR' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'VIEWOTHCALENDAR' , '*' , 7 , 1 , 'OUTLOOK' , 'msoControlButton' , 0 , 800 , 0 , 'VIEWCALENDAR;OTHER;USER' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'VIEWOTHTASKS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'VIEWOTHTASKS' , '*' , 11 , 1 , 'OUTLOOK' , 'msoControlButton' , 0 , 1561 , 0 , 'VIEWTASKS;OTHER;USER' , 0 , NULL , 'Ispackageinstalled("TASKS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'MAIN' , 'VIEWTASKS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'MAIN' , 'VIEWTASKS' , '*' , 9 , 1 , 'OUTLOOK' , 'msoControlButton' , 1 , 1561 , 0 , 'VIEWTASKS;USER' , 0 , NULL , 'Ispackageinstalled("TASKS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'ADDBCC' , 'NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'ADDBCC' , 'NEW' , 1 , 1 , 'ATTACHRECIPIENT' , 'msoControlButton' , 0 , 682 , 0 , 'ATTACHRECIPIENT;BCC' , 0 , NULL , 'IsPackageInstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'ADDCC' , 'NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'ADDCC' , 'NEW' , 1 , 0 , 'ATTACHRECIPIENT' , 'msoControlButton' , 0 , 682 , 0 , 'ATTACHRECIPIENT;CC' , 0 , NULL , 'IsPackageInstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'ADDFIELD' , 'DOCUMENT;PRECEDENT;NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'ADDFIELD' , 'DOCUMENT;PRECEDENT;NEW' , 0 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 213 , 0 , 'ADDFIELD' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'ATTACHDOC' , 'DOCUMENT;NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'ATTACHDOC' , 'DOCUMENT;NEW' , 8 , 2 , NULL , 'msoControlButton' , 0 , 283 , 0 , 'ATTACHDOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'ATTACHRECIPIENT' , 'NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'ATTACHRECIPIENT' , 'NEW' , 7 , 0 , NULL , 'msoControlPopup' , 0 , 682 , 0 , NULL , 0 , NULL , 'IsPackageInstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'ATTNOTE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'ATTNOTE' , '*' , 0 , 2 , 'NOTES' , 'msoControlButton' , 0 , 275 , 0 , 'REPLY;ATTNOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'CLIENTINFOITEM' , 'DOCUMENT;CLIENTID' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'CLIENTINFOITEM' , 'DOCUMENT;CLIENTID' , 4 , 0 , NULL , 'msoControlButton' , 1 , 213 , 0 , 'CLIENTINFOITEM' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'DELETEFIELD' , 'DOCUMENT;PRECEDENT;NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'DELETEFIELD' , 'DOCUMENT;PRECEDENT;NEW' , 1 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 21 , 0 , 'DELETEFIELD' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'DETACH' , 'DOCUMENT;SAVED' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'DETACH' , 'DOCUMENT;SAVED' , 6 , 0 , NULL , 'msoControlButton' , 0 , 21 , 0 , 'DETACHDOCVARS' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'FILE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'FILE' , '*' , 2 , 2 , 'NOTES' , 'msoControlButton' , 1 , 176 , 0 , 'REPLY;FILENOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'FILEINFOITEM' , 'DOCUMENT;FILEID' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'FILEINFOITEM' , 'DOCUMENT;FILEID' , 3 , 0 , NULL , 'msoControlButton' , 1 , 213 , 0 , 'FILEINFOITEM' , 0 , NULL , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'FILEIT' , 'DOCUMENT;SAVED' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'FILEIT' , 'DOCUMENT;SAVED' , 8 , 0 , NULL , 'msoControlButton' , 0 , 21 , 0 , 'FILEIT' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'LETTERS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'LETTERS' , '*' , 0 , 1 , 'REPLY' , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'MEETING' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'MEETING' , '*' , 1 , 2 , 'NOTES' , 'msoControlButton' , 0 , 2141 , 0 , 'REPLY;MEETINGNOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'NEWFAX' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'NEWFAX' , '*' , 2 , 1 , 'REPLY' , 'msoControlButton' , 0 , 176 , 0 , 'REPLY;FAX;ASSOC' , 0 , NULL , 'Ispackageinstalled("FAX")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'NEWMAIL' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'NEWMAIL' , '*' , 3 , 1 , 'REPLY' , 'msoControlButton' , 0 , 2188 , 0 , 'REPLY;EMAIL;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'NOTES' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'NOTES' , '*' , 1 , 1 , 'REPLY' , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , NULL , 'Ispackageinstalled("CLMCONTLEGAL")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'PRECEDENTS' , 'DOCUMENT;NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'PRECEDENTS' , 'DOCUMENT;NEW' , 7 , 0 , NULL , 'msoControlButton' , 1 , 2144 , 0 , 'PRECEDENTS;ACTIVE' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'RELINKFIELDS' , 'DOCUMENT;NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'RELINKFIELDS' , 'DOCUMENT;NEW' , 6 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 279 , 0 , 'RELINKFIELDS' , 0 , NULL , 'Ispackageinstalled("ADDAFIELD")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'REPLY' , 'DOCUMENT;ASSOCID' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'REPLY' , 'DOCUMENT;ASSOCID' , 5 , 0 , NULL , 'msoControlPopup' , 1 , NULL , 0 , NULL , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'SAVEAS' , 'DOCUMENT;INWARD' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'SAVEAS' , 'DOCUMENT;INWARD' , 0 , 0 , NULL , 'msoControlButton' , 0 , 271 , 0 , 'SAVEAS' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'SAVEASPREC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'SAVEASPREC' , '*' , 3 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 271 , 0 , 'SAVEASPREC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'SAVEITEM' , 'PRECEDENT;DOCUMENT;INWARD;SAVED' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'SAVEITEM' , 'PRECEDENT;DOCUMENT;INWARD;SAVED' , 2 , 0 , NULL , 'msoControlButton' , 0 , 271 , 0 , 'SAVE' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'SAVESENDITEM' , 'DOCUMENT;NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'SAVESENDITEM' , 'DOCUMENT;NEW' , 1 , 0 , NULL , 'msoControlButton' , 0 , 271 , 0 , 'SAVESENDITEM' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'SHOWDOCVARS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'SHOWDOCVARS' , '*' , 2 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 283 , 0 , 'SHOWDOCVARS' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'TELIN' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'TELIN' , '*' , 3 , 2 , 'NOTES' , 'msoControlButton' , 1 , 275 , 0 , 'REPLY;TELNOTEIN;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'TELOUT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'TELOUT' , '*' , 4 , 2 , 'NOTES' , 'msoControlButton' , 0 , 275 , 0 , 'REPLY;TELNOTEOUT;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'TEMPLATEPROP' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'TEMPLATEPROP' , 'Word' , 4 , 1 , 'TOOLS' , 'msoControlButton' , 0 , 283 , 0 , 'TEMPLATEPROP' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'TOASSOC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'TOASSOC' , '*' , 1 , 2 , 'LETTERS' , 'msoControlButton' , 0 , 176 , 0 , 'REPLY;LETTERHEAD;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'TOCLIENT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'TOCLIENT' , '*' , 0 , 2 , 'LETTERS' , 'msoControlButton' , 0 , 176 , 0 , 'REPLY;LETTERHEAD;CLIENT' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'OUTLOOKITEM' , 'TOOLS' , 'PRECEDENT;DOCUMENT;NEW' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'OUTLOOKITEM' , 'TOOLS' , 'PRECEDENT;DOCUMENT;NEW' , 6 , 0 , NULL , 'msoControlPopup' , 0 , NULL , 0 , NULL , 0 , 'PRECEDIT' , NULL , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'ADDFIELD' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'ADDFIELD' , '*' , 3 , 0 , NULL , 'msoControlButton' , 1 , 213 , 0 , 'ADDFIELD' , 0 , 'PRECEDIT' , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'DELETEFIELD' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'DELETEFIELD' , '*' , 4 , 0 , NULL , 'msoControlButton' , 0 , 21 , 0 , 'DELETEFIELD' , 0 , 'PRECEDIT' , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'INSERTPARASTOP' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'INSERTPARASTOP' , 'Word' , 6 , 0 , NULL , 'msoControlButton' , 0 , 0 , 0 , 'INSERTPARASTOP' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'INSERTSTOP' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'INSERTSTOP' , 'Word' , 5 , 0 , NULL , 'msoControlButton' , 0 , NULL , 0 , 'INSERTSTOP' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'PRECEDENTS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'PRECEDENTS' , '*' , 2 , 0 , NULL , 'msoControlButton' , 0 , 23 , 0 , 'PRECEDENTS;QUICK' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'SAVEASPREC' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'SAVEASPREC' , '*' , 1 , 0 , NULL , 'msoControlButton' , 0 , 271 , 0 , 'SAVEASPREC' , 0 , 'PRECEDIT' , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'SHOWDOCVARS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'SHOWDOCVARS' , '*' , 8 , 0 , NULL , 'msoControlButton' , 1 , 283 , 0 , 'SHOWDOCVARS' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'STRIPFORMATTING' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'STRIPFORMATTING' , 'Word' , 7 , 0 , NULL , 'msoControlButton' , 0 , 0 , 0 , 'STRIPFORMATTING' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'STYLES' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'STYLES' , 'Word' , 0 , 0 , NULL , 'msoControlButton' , 0 , 5757 , 0 , 'STYLES' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'PRECEDENT' , 'TEMPLATEPROP' , 'Word' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'PRECEDENT' , 'TEMPLATEPROP' , 'Word' , 9 , 0 , NULL , 'msoControlButton' , 0 , 283 , 0 , 'TEMPLATEPROP' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'REPORTS' , 'REPORTS' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'REPORTS' , 'REPORTS' , '*' , 1 , 0 , NULL , 'None' , 0 , 0 , 0 , NULL , 0 , NULL , '' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'TIMEREC' , 'FILE' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'TIMEREC' , 'FILE' , '*' , 2 , 0 , NULL , 'msoControlButton' , 1 , 176 , 0 , 'TEMPLATESTART;FILENOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'TIMEREC' , 'MEETING' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'TIMEREC' , 'MEETING' , '*' , 3 , 0 , NULL , 'msoControlButton' , 0 , 2141 , 0 , 'TEMPLATESTART;MEETINGNOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'TIMEREC' , 'NEWTIME' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'TIMEREC' , 'NEWTIME' , '*' , 4 , 0 , NULL , 'msoControlButton' , 1 , 176 , 0 , 'SCRIPT;NewTime' , 0 , NULL , 'IsLicensedFor("TIMEREC")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'TIMEREC' , 'TELIN' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'TIMEREC' , 'TELIN' , '*' , 0 , 0 , NULL , 'msoControlButton' , 0 , 275 , 0 , 'TEMPLATESTART;TELEPHONENOTEIN;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

IF NOT EXISTS ( SELECT 'TIMEREC' , 'TELOUT' , '*' INTERSECT SELECT ctrlCommandBar , ctrlCode , ctrlFilter FROM dbo.dbCommandBarControl )
BEGIN
	INSERT dbo.dbCommandBarControl ( ctrlCommandBar , ctrlCode , ctrlFilter , ctrlOrder , ctrlLevel , ctrlParent , ctrlType , ctrlBeginGroup , ctrlIcon , ctrlHide , ctrlRunCommand , ctrlIncFav , ctrlRole , ctrlCondition , ctrlLicense )
	VALUES ( 'TIMEREC' , 'TELOUT' , '*' , 1 , 0 , NULL , 'msoControlButton' , 0 , 275 , 0 , 'TEMPLATESTART;TELEPHONENOTE;ASSOC' , 0 , NULL , 'Ispackageinstalled("DMS")' , NULL )
END
GO

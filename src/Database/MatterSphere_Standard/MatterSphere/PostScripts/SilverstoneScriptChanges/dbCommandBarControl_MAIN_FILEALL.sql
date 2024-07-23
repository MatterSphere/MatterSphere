Print 'dbCommandBarControl_MAIN_FILEALL.sql'


-- Create the codeLookup
EXEC sprCreateCodeLookup @Type='CBCCAPTIONS', @Code='FILEALL', @Description='File Selected Items', @Help=null, @Notes=null, @AddLink=null
GO
 
-- Create the control
IF NOT EXISTS ( SELECT ctrlID FROM dbo.dbCommandBarControl WHERE ctrlCommandBar = 'MAIN' AND ctrlCode = 'FILEALL' AND ctrlFilter = 'OUTLOOK' )
BEGIN
	INSERT dbCommandBarControl (ctrlCommandBar, ctrlCode, ctrlFilter, ctrlOrder, ctrlLevel, ctrlType, ctrlBeginGroup, ctrlIcon, ctrlHide, ctrlRunCommand, ctrlIncFav)
	VALUES ('MAIN', 'FILEALL', 'Outlook', 11, 0, 'msoControlButton', 'False', 23, 'False', 'FILEALL', 'False')
END
GO



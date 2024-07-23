Print 'dbCommandBarControl_Update_to_ctrlLevel.sql'


 
IF EXISTS ( SELECT ctrlID From dbo.dbCommandBarControl WHERE ctrlCode = 'ATTACHDOC' AND ctrlCommandBar = 'OUTLOOKITEM' )
BEGIN
	UPDATE dbo.dbCommandBarControl SET ctrlLevel = 0 WHERE ctrlCode = 'ATTACHDOC' AND ctrlCommandBar = 'OUTLOOKITEM' AND ctrlLevel <> 0
END




IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 20010 )
BEGIN
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (20010, 'MatterFldrsTree', 'OMS', 0, 'FWBS.OMS.UI.Windows.ucMatterFoldersTree,OMS.UI', NULL, NULL)
END
ELSE
BEGIN
	IF NOT EXISTS(SELECT 'MatterFldrsTree' , 'OMS' , 0 , 'FWBS.OMS.UI.Windows.ucMatterFoldersTree,OMS.UI' INTERSECT SELECT ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 20010)
	UPDATE [dbo].[dbEnquiryControl] SET 
		ctrlCode = 'MttrFldrsTr', 
		ctrlGroup = 'OMS',
		ctrlSystem = 0,
		ctrlWinType='FWBS.OMS.UI.Windows.ucMatterFoldersTree,OMS.UI'  
	WHERE 
		ctrlID = 20010
END

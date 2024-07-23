IF EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 107 AND ctrlCode = 'ePictureBoxV2')
	DELETE dbo.dbDateWizDates WHERE datectrlid = 107
	DELETE dbo.dbEnquiryQuestion WHERE quctrlid = 107
	DELETE dbo.dbEnquiryControl  WHERE ctrlID = 107
GO
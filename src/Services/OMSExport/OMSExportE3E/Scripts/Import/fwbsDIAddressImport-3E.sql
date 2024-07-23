
GO

/****** Object:  StoredProcedure [dbo].[fwbsDIAddressImport]    Script Date: 05/31/2013 15:15:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fwbsDIAddressImport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[fwbsDIAddressImport]
GO


GO

/****** Object:  StoredProcedure [dbo].[fwbsDIAddressImport]    Script Date: 05/31/2013 15:15:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[fwbsDIAddressImport]

AS

DECLARE @Rows bigint , @Error int
SET NOCOUNT ON


-- Insert the Address records
BEGIN TRANSACTION
	INSERT [dbo].[dbAddress]
		(
		[addLine1] ,
		[addLine2] ,
		[addLine3] ,
		[addLine4] ,
		[addLine5] ,
		[addPostCode] ,
		[addCountryOld] ,
		[addCountry] , 
		[addDXCode] ,
		[Created] ,
		[CreatedBy] 
		)
	SELECT 
		CD.[addLine1] , CD.[addLine2] , CD.[addLine3] , CD.[addLine4] , CD.[addLine5] , CD.[addPostCode] , CD.[addCountry] , dbo.GetCountryIDByCode ( default , CD.[addCountry] , default ) , CD.[addDXCode] ,
		getutcdate() , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER') 
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD 
	LEFT JOIN 
		[dbo].[dbAddress] A
	ON
		replace ( coalesce ( CD.[addLine1] , '' ) + coalesce ( CD.[addLine2] , '' ) + coalesce ( CD.[addLine3] , '' ) + coalesce ( CD.[addLine4] , '' ) + coalesce ( CD.[addLine5] , '' ) , ' ' , '' ) =
		replace ( coalesce ( A.[addLine1] , '' ) + coalesce ( A.[addLine2] , '' ) + coalesce ( A.[addLine3] , '' ) + coalesce ( A.[addLine4] , '' ) + coalesce ( A.[addLine5] , '' ) , ' ' , '' ) 
	WHERE
		replace ( coalesce ( A.[addLine1] , '' ) + coalesce ( A.[addLine2] , '' ) + coalesce ( A.[addLine3] , '' ) + coalesce ( A.[addLine4] , '' ) + coalesce ( A.[addLine5] , '' ) , ' ' , '' ) = ''
	GROUP BY
		CD.[addLine1] , CD.[addLine2] , CD.[addLine3] , CD.[addLine4] , CD.[addLine5] , CD.[addPostCode] , CD.[addCountry]  ,CD.[addDXCode] 
	-- Error handling and logging	
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Importing Addresses' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Address records Imported'
		END
COMMIT TRANSACTION

	
-- Update OMSImport table with address ID's
BEGIN TRANSACTION
	UPDATE CD
	SET CD.[OMSaddID] = Coalesce ( A.[addID] , (SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'UKNADDRESS' ) )
	FROM  [OMSImport].[dbo].[ClientDetails] CD JOIN [dbo].[dbAddress] A 
	ON	replace ( coalesce ( CD.[addLine1] , '' ) + coalesce ( CD.[addLine2] , '' ) + coalesce ( CD.[addLine3] , '' ) + coalesce ( CD.[addLine4] , '' ) + coalesce ( CD.[addLine5] , '' ) , ' ' , '' ) =
		replace ( coalesce ( A.[addLine1] , '' ) + coalesce ( A.[addLine2] , '' ) + coalesce ( A.[addLine3] , '' ) + coalesce ( A.[addLine4] , '' ) + coalesce ( A.[addLine5] , '' ) , ' ' , '' )
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Updating Address IDs' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogUpdates] , [LogImportDesc] )
			SELECT  @Rows , 'Address IDs updated in Staging tables'
		END
COMMIT TRANSACTION

SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET NOCOUNT OFF
RETURN




GO



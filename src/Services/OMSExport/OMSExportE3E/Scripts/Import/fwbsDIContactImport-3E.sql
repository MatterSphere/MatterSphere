/****** Object:  StoredProcedure [dbo].[fwbsDIContactImport]    Script Date: 05/09/2013 10:16:49 ******/
DROP PROCEDURE [dbo].[fwbsDIContactImport]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[fwbsDIContactImport]

AS

DECLARE @Rows bigint , @Error int
SET NOCOUNT ON
SET XACT_ABORT ON


-- UPDATED FOR 3E INTEGRATION
-- * EXTRA EXTERNAL ID FOR ENTITY INDEXES ON DBCLIENT AND DBCONTACT
-- * USE 3E OPENTKPR AS THE DBCLIENT FIRM CONTACT (FEEUSRID)

-- Insert the Contact records
BEGIN TRANSACTION
	INSERT [dbo].[dbContact]
		(
		[contTypeCode] ,
		[contName] ,
		[contDefaultAddress] ,
		[contSalut] ,
		[contNotes] ,
		[Created] , 
		[CreatedBy] ,
		[contExtTxtID] , 
		[contExtID]			--FOR 3E ENTITYID
		)
	SELECT 
		[contType] ,
		(SELECT  TOP 1 Coalesce ( ltrim ( Coalesce ( [contTitle] , '' ) + ' ' + rtrim ( ( [contFirstNames] + ' ' + Coalesce ( [contSurname] , '' )) )) , [clName] )  
			FROM [OMSImport].[dbo].[ClientDetails] C  WHERE C.[extContID] = CD.[extcontid] ) ,
		Coalesce ( [OMSaddID] , (SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'UKNADDRESS' ) ) ,
		[contSalut] , 
		[contNotes] , 
		getutcdate() , 
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) ,
		[extContID] ,
		[ENTINDEX]
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD 
	WHERE 
		[DCFlag] IN ( 1 , 2 )
	GROUP BY [extContID] , [contType] , [OMSAddID] , [contSalut] , [contNotes] , ENTINDEX
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Contacts')
			GOTO ERRORHANDLER 
		END
	IF @Rows > 0
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		SELECT  @Rows , 'New Contact records Imported'
	END

	

-- Insert the Contact Addresses records
	INSERT [dbo].[dbContactAddresses]
		(
		[contID] ,
		[contaddID] ,
		[contCode]
		)
	SELECT 
		CO.[contID] , CO.[contDefaultAddress] , 'MAIN'
	FROM 
		[dbo].[dbContact] CO  
	JOIN
		[OMSImport].[dbo].[ClientDetails] CD on CD.[ExtContID]  = CO.[contExtTxtID]
	WHERE 
		CD.[DCFlag] IN ( 1 , 2 )
	GROUP BY  CO.[contID] , CO.[contDefaultAddress]
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Contact Addresses' )
			GOTO ERRORHANDLER 
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Contact Address records Imported'
		END



-- Update the default Contact Address record
	UPDATE C
	SET C.[contDefaultAddress] = CD.[OMSaddID] , C.[updated]  = getdate () , C.[updatedBy] = ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' )
	FROM [dbo].[dbContact] C JOIN [OMSImport].[dbo].[ClientDetails] CD ON C.[ContExtTxtID] = CD.[extContID] 
	WHERE CD.[DCFlag] = 4
	SELECT @Rows = @@Rowcount , @Error = @@Error
		-- Error handling and logging
		IF @Error <> 0
			BEGIN 
				ROLLBACK TRANSACTION
				INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
				VALUES ( 'Error Updating Default Contact Addresses' )
				GOTO ERRORHANDLER 
			END
	IF @Rows > 0
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogUpdates] , [LogImportDesc] )
		SELECT  @Rows , 'Default Contact Address records Updated'
	END



-- Insert Individual Contact records
	INSERT [dbo].[dbContactIndividual]
		(
		[contID] ,
		[contTitle] ,
		[contChristianNames] ,
		[contSurname] ,
		[contSex]
		)
	SELECT 
		CO.[contID] , CD.[contTitle] , CD.[contFirstNames] , CD.[contSurname] , CD.[contSex]
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD  
	JOIN 
		[dbo].[dbContact] CO ON CO.[contExtTxtID] = CD.[ExtContID] 
	WHERE 
		CD.[DCFlag] IN ( 1 , 2 ) AND CD.[contFirstNames] IS NOT NULL
	GROUP BY
		CO.[contID] , CD.[contTitle] , CD.[contFirstNames] , CD.[contSurname] , CD.[contSex]
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Individual Contacts' )		
			GOTO ERRORHANDLER 
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Individual Contact records Imported'
		END
	

-- Insert the Corporate Contact records
	INSERT [dbo].[dbContactCompany]
		(
		[contID] ,
		[contRegCoName]
		)
	SELECT 
		CO.[contID] , CD.[contSurname]
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD 
	JOIN 
		[dbo].[dbContact] CO ON CO.[contExtTxtID] = CD.[ExtContID] 
	WHERE 
		CD.[DCFlag] IN ( 1 , 2 ) AND CD.[contFirstNames] IS NULL
	GROUP BY
		CO.[contID] , CD.[contSurname]
	SELECT @Rows = @@Rowcount , @ERror = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Corporate Contacts' )	
			GOTO ERRORHANDLER 
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Corporate Contact records Imported'
		END
	

-- Insert the 'Home telephone' Contact Numbers
	INSERT [dbo].[dbContactNumbers]
		(
		[contID] ,
		[contNumber] ,
		[contCode] ,
		[contExtraCode] 
		)
	SELECT 
		CO.[contID] , CD.[contTelHome] , 'TELEPHONE' , 'HOME'
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD  
	JOIN 
		[dbo].[dbContact] CO ON CO.[contExtTxtID] = CD.[ExtContID] 
	WHERE 
		CD.[DCFlag] IN ( 1 , 2 ) AND LEN ( CD.[contTelHome] ) > 1
	GROUP BY
		CO.[contID] , CD.[contTelHome] 
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Contact Home Telephone Numbers' )
			GOTO ERRORHANDLER 
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Contact Home Telephone numbers Imported'
		END

	

-- Insert the 'Work telephone' Contact Numbers
	INSERT [dbo].[dbContactNumbers]
		(
		[contID] ,
		[contNumber] ,
		[contCode] ,
		[contExtraCode] 
		)
	SELECT 
		CO.[contID] , CD.[contTelWork] , 'TELEPHONE' , 'WORK'
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD 
	JOIN 
		[dbo].[dbContact] CO ON CO.[contExtTxtID] = CD.[ExtContID] 
	WHERE 
		CD.[DCFlag] IN ( 1 , 2 ) AND LEN ( CD.[contTelWork] ) > 1
	GROUP BY
		CO.[contID] , CD.[contTelWork] 
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Contact Work Telephone Numbers' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Contact Work Telephone numbers Imported'
		END



-- Insert the 'FAX' Contact Numbers
	INSERT [dbo].[dbContactNumbers]
		(
		[contID] ,
		[contNumber] ,
		[contCode] ,
		[contExtraCode] 
		)
	SELECT 
		CO.[contID] , CD.[contFAX] , 'FAX' , 'MAIN'
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD 
	JOIN 
		[dbo].[dbContact] CO ON CO.[contExtTxtID] = CD.[ExtContID] 
	WHERE 
		CD.[DCFlag] IN ( 1 , 2 ) AND LEN ( CD.[contFAX] ) > 1
	GROUP BY
		CO.[contID] , CD.[contFAX] 
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Contact FAX Numbers' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Contact FAX numbers Imported'
		END



-- Insert the 'Mobile telephone' Contact Numbers
	INSERT [dbo].[dbContactNumbers]
		(
		[contID] ,
		[contNumber] ,
		[contCode] ,
		[contExtraCode] 
		)
	SELECT 
		CO.[contID] , CD.[contTelMob] , 'TELEPHONE' , 'MOBILE'
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD 
	JOIN 
		[dbo].[dbContact] CO ON CO.[contExtTxtID] = CD.[ExtContID] 
	WHERE 
		CD.[DCFlag] IN ( 1 , 2 ) AND LEN ( CD.[contTelMob] ) > 1
	GROUP BY
		CO.[contID] , CD.[contTelMob] 
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Contact Mobile Telephone Numbers' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Contact Mobile Telephone numbers Imported'
		END



-- Insert the Contact Email addresses
	INSERT [dbo].[dbContactEmails]
		(
		[contID] ,
		[contEmail] ,
		[contCode]
		)
	SELECT 
		CO.[contID] , CD.[contEmail] , 'MAIN' 
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD 
	JOIN 
		[dbo].[dbContact] CO ON CO.[contExtTxtID] = CD.[ExtContID] 
	WHERE 
		CD.[DCFlag] IN ( 1 , 2 ) AND LEN ( CD.[contEmail] ) > 1
	GROUP BY
		CO.[contID] , CD.[contEmail]
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Contact Emails' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Contact Emails Imported'
		END


-- Insert the Client records
	INSERT [dbo].[dbClient]
		(
		[clNo] ,
		[clAccCode] ,
		[brid] ,		 -- 3E OFFICE
		[clTypeCode] ,	
		[clName] ,
		[feeusrId] ,	-- 3E OPEN TIMEKEEPER
		[createdBy] ,
		[clDefaultContact] ,
		[clSource] ,
		[clUICultureInfo] ,
		[clSearch1] ,
		[clSearch2] ,
		[clSearch3] ,
		[clSearch4] ,
		[clSearch5] ,
		[clextID]		-- 3E CLIENTINDEX
		)


	SELECT
		Q1.[clNo] , Q1.[clAccCode] , Q1.[Branch] , Q1.[clType] , Q1.[clName] , Q1.[feeusrId] , Q1.[createdBy] , C.[ContID] , Q1.[source] , Q1.[culture] , Q1.[search1] , Q1.[search2] , Q1.[search3] ,
		Q1.[search4] , Q1.[search5] , Q1.CLIENTINDEX
	FROM
		(
		SELECT 
			[clNo] , 
			[extContID] as [clAccCode] , 
			coalesce ( brid, ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'CLBRANCH' ) ) as [Branch] , 
			coalesce ( [clType] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'CLTYPE' ) ) as [clType] ,
			[clName] ,
			coalesce ( feeusrid, ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) ) as [feeusrid]  ,
			( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) as [createdBy] , 
			( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'SOURCE' ) as [source]  ,  
			( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERCULTURE' ) as [culture] ,
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 1 ) as [search1] , 
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 2 ) as [search2] , 
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 3 ) as [search3] , 
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 4 ) as [search4] , 
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 5 ) as [search5] ,
			CLIENTINDEX
		FROM 
			[OMSImport].[dbo].[ClientDetails]  
		WHERE 
			[DCFlag] IN ( 1 , 3 ) 
		GROUP BY
			[clNo] , [clName] , [clType] , [extContID] , CLIENTINDEX, feeusrid, brid
		) as Q1
	JOIN	
		(
		SELECT 
			[clNo] , min ( [extContID] ) as [defContID] FROM [OMSImport].[dbo].[ClientDetails] GROUP BY [clNo]
		) as Q2 ON Q1.[clNo] = Q2.[clNo]
	JOIN	-- nearly finished
		 [dbo].[dbContact] C ON Q2.[defContID] = C.[contExtTxtID]

	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Clients' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Client records Imported'
		END

	

-- Create the Client - Contact record
	INSERT [dbo].[dbClientContacts]
		(
		[ClID] ,
		[contID] 
		)
	SELECT 
		C.[clID] , CO.[contID] 
	FROM
		[dbo].[dbClient] C 
	JOIN
		[OMSImport].[dbo].[ClientDetails] CD ON CD.[clNo] = C.[clNo]
	 JOIN
		[dbo].[dbContact] CO ON CD.[extContID] = CO.[contExtTxtID]
	WHERE 
		CD.[DCFlag] IN ( 1 , 2  , 3 ) 
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Creating Client Contacts' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT  @Rows , 'New Client Contact records Imported'
		END
COMMIT TRANSACTION

SET XACT_ABORT OFF
SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET XACT_ABORT OFF
SET NOCOUNT OFF
RETURN

SET QUOTED_IDENTIFIER OFF 



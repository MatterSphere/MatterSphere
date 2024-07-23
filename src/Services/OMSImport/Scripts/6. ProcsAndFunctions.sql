-----------------------------------------------------------------
-- Run on MatterCentre Database
-----------------------------------------------------------------

USE [MatterCentre]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDIAddressImport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDIAddressImport]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDICleanUp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDICleanUp]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDIContactImport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDIContactImport]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDIDocImport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDIDocImport]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDIFeeEarnerImport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDIFeeEarnerImport]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDIFileImport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDIFileImport]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDIReturnOMSFileID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDIReturnOMSFileID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDISetDefaults]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDISetDefaults]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDISetDocDCFlags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDISetDocDCFlags]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDIsetClientDCFlags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDIsetClientDCFlags]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fwbsDIsetFileDCFlags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[fwbsDIsetFileDCFlags]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCountryID]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[GetCountryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSearchField]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[GetSearchField]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE FUNCTION GetCountryID  (@Type uCodeLookup, @Desc nvarchar(1000) , @UI uUICultureInfo)  
RETURNS uCountry AS  
BEGIN 
	declare @ctryID uCountry
		select @ctryID = C.ctryID from dbCountry C JOIN  dbcodelookup CU  on C.ctryCode = CU.cdCode
		where cdtype = @Type and cdDesc = @Desc and @UI Like cdUICultureInfo + '%'
	
	if (@ctryID = '')
		BEGIN
			SET @ctryID = NULL
		END
	return @ctryID
END





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE function dbo.GetSearchField(@name nvarchar(100), @id tinyint)
returns 
nvarchar(100) 
as
begin


declare @tmp nvarchar(100)
declare @return nvarchar(100)

declare @searchfield1 nvarchar(50)
declare @searchfield2 nvarchar(50)
declare @searchfield3 nvarchar(50)
declare @searchfield4 nvarchar(50)
declare @searchfield5 nvarchar(50)

set @tmp = @name
set @tmp = ltrim(replace(@tmp,'Mr ',''))
set @tmp = ltrim(replace(@tmp,'Miss ',''))
set @tmp = ltrim(replace(@tmp,'Mrs ',''))
set @tmp = ltrim(replace(@tmp,'Ms ',''))
set @tmp = ltrim(replace(@tmp,'Dr ',''))
set @tmp = ltrim(replace(@tmp,'and ',''))
set @tmp = ltrim(replace(@tmp,'$ ',''))
set @tmp = @tmp + ' '

set @searchfield1 = rtrim(left(@tmp,charindex(' ',@tmp)))
set @tmp  =ltrim(substring(@tmp,len(@searchfield1)+2,len(@tmp))) --replace(@tmp,@searchfield1 + ' ','')
set @searchfield2 = rtrim(left(@tmp,charindex(' ',@tmp)))
set @tmp  =ltrim(substring(@tmp,len(@searchfield2)+2,len(@tmp)))
set @searchfield3 = rtrim(left(@tmp,charindex(' ',@tmp)))
set @tmp  =ltrim(substring(@tmp,len(@searchfield3)+2,len(@tmp)))
set @searchfield4 = rtrim(left(@tmp,charindex(' ',@tmp)))
set @tmp  =ltrim(substring(@tmp,len(@searchfield4)+2,len(@tmp)))
set @searchfield5 = rtrim(left(@tmp,charindex(' ',@tmp)))
set @tmp  =ltrim(substring(@tmp,len(@searchfield5)+2,len(@tmp)))

while len(@searchfield1) = 1
begin 
	set @searchfield1 = @searchfield2
	set @searchfield2 = @searchfield3
	set @searchfield3 = @searchfield4
	set @searchfield4 = @searchfield5
	set @searchfield5 = ''
end

while len(@searchfield2) = 1 
begin
	set @searchfield2 = @searchfield3
	set @searchfield3 = @searchfield4
	set @searchfield4 = @searchfield5
	set @searchfield5 = ''
end
while len(@searchfield3) = 1
begin
	set @searchfield3 = @searchfield4
	set @searchfield4 = @searchfield5
	set @searchfield5 = ''
end
while len(@searchfield4) = 1
begin
	set @searchfield4 = @searchfield5
	set @searchfield5 = ''
end


 
if @id = 1  
set @return = @searchfield1
if @id = 2 
set @return  = @searchfield2
if @id = 3
set @return  = @searchfield3
if @id = 4 
set @return  = @searchfield4
if @id = 5 
set @return  = @searchfield5

if len(@return) = 0 set @return = null

return @return
end






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
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
		CD.[addLine1] , CD.[addLine2] , CD.[addLine3] , CD.[addLine4] , CD.[addLine5] , CD.[addPostCode] , CD.[addCountry] , dbo.GetCountryID ( 'COUNTRIES' , CD.[addCountry] , '{default}' ) , CD.[addDXCode] ,
		getdate() , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER') 
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROCEDURE [dbo].[fwbsDICleanUp]

AS

DECLARE @Rows bigint
SET NOCOUNT ON

BEGIN TRANSACTION
	DELETE [OMSImport].[dbo].[FileDetails] WHERE [DcFlag] NOT IN ( 8 , 9 )
	IF @@Error <> 0
		GOTO ERRORHANDLER
	DELETE [OMSImport].[dbo].[ClientDetails] WHERE [DcFlag] NOT IN ( 9 )
	IF @@Error <> 0
		GOTO ERRORHANDLER
	DELETE [OMSImport].[dbo].[Documents] WHERE [DcFlag] NOT IN ( 8 , 9 )
	IF @@Error <> 0
		GOTO ERRORHANDLER
	DELETE [OMSImport].[dbo].[FeeEarner] WHERE [DcFlag] NOT IN ( 9 )
	IF @@Error <> 0
		GOTO ERRORHANDLER
	DELETE [OMSImport].[dbo].[CodeLookup] 
	IF @@Error <> 0
		GOTO ERRORHANDLER
	INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
	VALUES ( 'Staging tables truncated' )
	INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
	VALUES ( 'xx- END OF IMPORT -xx' )
COMMIT TRANSACTION

SET NOCOUNT OFF
RETURN

ERRORHANDLER:
ROLLBACK TRANSACTION
INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
VALUES ( 'Error truncating staging tables, disabling SQL Agent job' )
EXEC [msdb].[dbo].[sp_update_job] @job_name = 'fwbsDataImport' , @enabled = 0
SET NOCOUNT OFF
RETURN
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE PROCEDURE [dbo].[fwbsDIContactImport]

AS

DECLARE @Rows bigint , @brID int , @Error int
SET NOCOUNT ON
SET XACT_ABORT ON
SET @brID = ( SELECT [brID] FROM [dbo].[dbRegInfo] )

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
		[contExtTxtID]
		)
	SELECT 
		[contType] ,
		(SELECT  TOP 1 Coalesce ( ltrim ( Coalesce ( [contTitle] , '' ) + ' ' + rtrim ( ( [contFirstNames] + ' ' + Coalesce ( [contSurname] , '' )) )) , [clName] )  
			FROM [OMSImport].[dbo].[ClientDetails] C  WHERE C.[extContID] = CD.[extcontid] ) ,
		Coalesce ( [OMSaddID] , (SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'UKNADDRESS' ) ) ,
		[contSalut] , 
		[contNotes] , 
		getdate() , 
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) ,
		[extContID] 
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD 
	WHERE 
		[DCFlag] IN ( 1 , 2 )
	GROUP BY [extContID] , [contType] , [OMSAddID] , [contSalut] , [contNotes]
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
		[brid] ,
		[clTypeCode] ,	
		[clName] ,
		[feeusrId] ,
		[createdBy] ,
		[clDefaultContact] ,
		[clSource] ,
		[clUICultureInfo] ,
		[clSearch1] ,
		[clSearch2] ,
		[clSearch3] ,
		[clSearch4] ,
		[clSearch5] 
		)


	-- This is the really tricky bit!!!  took me a little time to work this out and then it just came to me, just after Gareth gave me half a bun for his birthday
	SELECT
		Q1.[clNo] , Q1.[clAccCode] , Q1.[Branch] , Q1.[clType] , Q1.[clName] , Q1.[feeusrId] , Q1.[createdBy] , C.[ContID] , Q1.[source] , Q1.[culture] , Q1.[search1] , Q1.[search2] , Q1.[search3] ,
		Q1.[search4] , Q1.[search5]
	FROM
		(
		SELECT 
			[clNo] , [extContID] as [clAccCode] , 1 as [Branch] ,  
			coalesce ( [clType] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'CLTYPE' ) ) as [clType] ,
			[clName] ,
			( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) as [feeusrid]  ,
			( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) as [createdBy] , 
			( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'SOURCE' ) as [source]  ,  
			( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERCULTURE' ) as [culture] ,
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 1 ) as [search1] , 
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 2 ) as [search2] , 
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 3 ) as [search3] , 
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 4 ) as [search4] , 
			[dbo].[GetSearchField] ( rtrim ( [clName] ) , 5 ) as [search5] 
		FROM 
			[OMSImport].[dbo].[ClientDetails]  
		WHERE 
			[DCFlag] IN ( 1 , 3 ) 
		GROUP BY
			[clNo] , [clName] , [clType] , [extContID]
		) as Q1
	JOIN	-- are you still following?
		(
		SELECT 
			[clNo] , min ( [extContID] ) as [defContID] FROM [OMSImport].[dbo].[ClientDetails] GROUP BY [clNo]
		) as Q2 ON Q1.[clNo] = Q2.[clNo]
	JOIN	-- nearly finished
		 [dbo].[dbContact] C ON Q2.[defContID] = C.[contExtTxtID]
	-- what about that then?????  Just think what I could of done with a whole bun

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


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE [dbo].[fwbsDIDocImport]

AS

DECLARE @Rows bigint , @brID int , @Error int , @docArchiveLocation tinyint
SET @brID = ( SELECT brID FROM [dbRegInfo] )
SET @docArchiveLocation = ( SELECT TOP 1 [spID] FROM [dbStorageProvider] WHERE [spCode] = 'SLPFS' )
SET NOCOUNT ON
SET XACT_ABORT ON

-- Import the new document records
BEGIN TRANSACTION
	INSERT dbDocument ( clID , fileID,  docDesc , docWallet , docFileName , docdirID , docDirection , docExtension , CreatedBy , Created ,  docBrID , docIDOld , docAppID , assocID , docArchiveLocation  )
	SELECT
		TAB1.[clID] , TAB1.[OMSfileID] , TAB1.[docDesc] , TAB1.[docWallet] , TAB1.[docFileName] , TAB1.[dirID] , TAB1.[docDirection] , TAB1.[docExtension] , TAB1.[CreatedBy] , TAB1.[Created] , @brID , TAB1.[docID] ,
		TAB1.[docAppID] , TAB2.[assocID] , @docArchiveLocation
	FROM
		(
		SELECT 
			C.[clID] , IMP.[OMSfileID] , IMP.[docDesc] , coalesce ( IMP.[docWallet]  , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'DOCWALLET' ) ) as [docWallet] ,
			CASE 
				WHEN convert ( nvarchar(3) , D.[dirID] ) IS NULL THEN  IMP.[docDir] + '\' + IMP. [docFileName]
				ELSE IMP.[docFileName]
			END as [docFileName] ,
			D.[dirID] , IMP.[docDirection] , IMP.[docExtension] , 
			coalesce (IMP.[CreatedBy] ,  ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) ) as CreatedBy , IMP.[Created] ,  IMP.[clNo] ,
			CASE
				WHEN IMP.[docExtension] IN ( 'doc' , 'dot' , 'rtf','docx','dotx' ) THEN ( SELECT [appID] FROM [dbApplication] WHERE [appCode] = 'WORD' )
				WHEN IMP.[docExtension] IN ( 'msg' , 'otf' ) THEN ( SELECT [appID] FROM [dbApplication] WHERE [appCode] = 'OUTLOOK' )
				WHEN IMP.[docExtension] IN ( 'xls','xlsx' ) THEN ( SELECT [appID] FROM [dbApplication] WHERE [appCode] = 'EXCEL' ) 
				ELSE ( SELECT [appID] FROM [dbApplication] WHERE [appCode] = 'SHELL' ) 
			END as [docAppID] ,
			IMP.[docID]
		FROM
			[OMSImport].[dbo].[Documents] IMP
		LEFT JOIN
			[dbDirectory] D ON D.[dirPath] = IMP.[docDir] AND D.[brID] = @brID
		JOIN
			[dbClient] C ON C.[clNo] = IMP.[clNo] 
		WHERE 
			IMP.[DCFlag] = 1
		) as TAB1
		JOIN
		(
		SELECT 
			min ( A.[assocID] ) as [assocID] , A.[fileID]
		FROM
			[dbAssociates] A
		JOIN
			[OMSImport].[dbo].[Documents] IMP ON A.[fileID] = IMP.[OMSfileID]
		WHERE
			IMP.[DCFlag] = 1
		GROUP BY
			A.[fileID]
		) as TAB2 ON TAB1.[OMSfileID] = TAB2.[fileID]
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( LogImportDesc )
			VALUES ( 'Error Importing Documents')
			GOTO ERRORHANDLER 
		END
	IF @Rows > 0
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		SELECT  @Rows , 'New Document records Imported'
	END
COMMIT TRANSACTION


SET XACT_ABORT OFF
SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET XACT_ABORT OFF
SET NOCOUNT OFF
RETURN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO






CREATE PROCEDURE  [dbo].[fwbsDIFeeEarnerImport]

AS

DECLARE @Rows bigint , @Error int
SET NOCOUNT ON

-- Insert New User records
BEGIN TRANSACTION
	SET IDENTITY_INSERT [dbo].[dbUser] ON
	INSERT [dbo].[dbUser]
		(
		[usrID] ,
		[usrInits] ,
		[usrAlias] ,
		[usrFullName] ,
		[usrADID] ,
		[usrSQLID] ,
		[usrWorksFor],
		[usrXML],
		[brID],
		[usrcurISOCode]
		)
	SELECT
		[usrID] , [Initials] , [Alias] , [FullName] , [NetworkName] , [Alias] , [usrID],
		'<config><settings><property name="Roles" value="" /></settings></config>',
		(SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERBRANCH' ) ,
		(SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERISOCODE' )
	FROM
		[OMSImport].[dbo].[FeeEarner]
	WHERE 
		[DCFlag] = 1
	-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			SET IDENTITY_INSERT [dbo].[dbUser] OFF
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Importing new Users' )
			GOTO ERRORHANDLER
		END
	SET IDENTITY_INSERT [dbo].[dbUser] OFF
	-- Insert the new Fee Earner records
	INSERT [dbo].[dbFeeEarner]
		(
		[feeusrID] ,
		[feeResponsibleTo] ,
		[feeResponsible] ,
		[feecurISOCode] ,
		[feeSignOff],
		[feeActive]
		)
	SELECT
		[usrID]  , - 1 , 0 , (SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERISOCODE' ) , [FullName],1
	FROM
		[OMSImport].[dbo].[FeeEarner]
	WHERE 
		[DCFlag] = 1 and usrType = 'F'
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error handling and logging
	IF @Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Importing new Fee Earners' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			SELECT @Rows , 'New Fee Earner records created'
		END
COMMIT TRANSACTION

SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET NOCOUNT OFF
RETURN




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO






CREATE PROCEDURE [dbo].[fwbsDIFileImport]

AS

DECLARE @Rows bigint , @Error int
SET NOCOUNT ON


-- Insert the new File records
BEGIN TRANSACTION
	INSERT [dbo].[dbFile]
		(
		[clID] ,
		[fileNo] ,
		[fileDesc] ,
		[fileResponsibleID] ,
		[filePrincipleID] ,
		[fileDepartment] ,
		[fileType] ,
		[fileFundCode] ,
		[fileCurISoCode] ,
		[fileStatus] ,
		[Created] ,
		[CreatedBy] ,
		[Updated] ,
		[fileClosed] ,
		[fileSource] ,
		[brID]
		)
	SELECT C.[clID] , FD.[fileNo] , FD.[fileDesc] , 
		coalesce ( FD.[fileResponsibleID] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) ) ,
		coalesce ( FD.[filePrincipleID] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' )  ) ,
		coalesce ( FD.[fileDept] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'DEPT' )  ) ,
		coalesce ( FD.[fileType] + '-' + FD.[fileDept]  , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FILETYPE' )  ) ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FUNDTYPE' ) ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERISOCODE' ) , 
		FD.[fileStatus] ,
		coalesce ( FD.[fileCreated] , getdate() ) ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) ,
		FD.[fileUpdated] ,
		FD.[fileClosed] ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'SOURCE' ) ,
		( SELECT TOP 1 [brID] FROM [dbo].[dbRegInfo] )
	FROM
		[dbo].[dbClient] C 
	JOIN
		[OMSImport].[dbo].[FileDetails] FD ON FD.[clNo] = C.[clNo]
	WHERE 
		FD.[DCFlag] = 1
	-- Error check and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
		IF @Error <> 0 
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Creating new Matters' )
			GOTO ERRORHANDLER
		END
	-- Write to log
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
			VALUES ( @Rows , 'New Matters Created' )
		END
COMMIT TRANSACTION

-- Create the Associate records for the default Contact only
BEGIN TRANSACTION
	 INSERT 
 		[dbo].[dbAssociates] ( [fileID] , [contID] , [assocOrder] , [assocType] , [assocHeading] , [assocdefaultaddID] , [assocSalut] )
 	SELECT 
		F.[fileID] , C.[contID] , 0 , 'CLIENT' , FD.[fileDesc] , NULL , coalesce ( C.[contSalut] , 'Sir/Madam' )
	FROM
		[dbo].[dbFile] F 
	JOIN
		[dbo].[dbClient] CL ON F.[clID] = CL.[clID]
	JOIN
		[dbo].[dbContact] C ON C.[contID] = CL.[cldefaultContact]
	JOIN
		[OMSImport].[dbo].[FileDetails] FD ON FD.[clNo] = CL.[clNo] AND F.[fileNo] = FD.[fileNo]
	WHERE
		FD.[DCFlag] = 1
	-- Error check and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
		IF @Error <> 0 
			BEGIN
				ROLLBACK TRANSACTION
				INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
				VALUES ( 'Error Creating new Associates' )
				GOTO ERRORHANDLER
			END
		-- Write to log
		IF @Rows > 0
			BEGIN
				INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
				VALUES ( @Rows , 'New Associates Created' )
			END
COMMIT TRANSACTION


SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET NOCOUNT OFF
RETURN



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
















CREATE PROCEDURE [dbo].[fwbsDIReturnOMSFileID]

AS

SET NOCOUNT ON
DECLARE @Error int  , @Rows bigint
-- Return the OMS FileID for new document records
BEGIN TRANSACTION
UPDATE 
	D
SET 
	D.[OMSfileID] = F.[fileID]
FROM
	[dbFile] F 
JOIN 
	[dbClient] C ON F.[clId] = C.[clID]
JOIN
	[OMSImport].[dbo].[Documents] D ON D.[clNo] = C.[clNo] AND D.[fileNo] = F.[fileNo]
WHERE 
	D.[DCFlag] = 1
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @Error <> 0
		BEGIN 
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error returning OMS fileID to Import table' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogUpdates] , [LogImportDesc] )
			SELECT  @Rows , 'File IDs updated in Staging tables'
		END
	COMMIT TRANSACTION

SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET NOCOUNT OFF
RETURN

GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROCEDURE [dbo].[fwbsDISetDefaults] 

AS
 
DECLARE @Rows bigint , @Error int
SET NOCOUNT ON
 

--Create default 'Source' for Import
IF  NOT EXISTS ( SELECT [cdID] FROM [dbo].[dbCodeLookup] C JOIN [OMSImport].[dbo].[Defaults] D ON C.[cdType] = D.[defType] AND C.[cdCode] = D.[defCode] WHERE D.[defType] = 'SOURCE' )
BEGIN
	BEGIN TRANSACTION
	 INSERT [dbo].[dbCodeLookup] ( [cdType] , [cdCode] , [cdDesc] )
	 SELECT 'SOURCE' ,  [defCode] , [defDesc] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'SOURCE'
	 -- Error check and logging
	 IF @@Error <> 0 
	 BEGIN
		   ROLLBACK TRANSACTION
		   INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		   VALUES ( 'Error Creating default Source' )
		   GOTO ERRORHANDLER
 	 END
	 -- Write to log
	 INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
 	SELECT 1  , 'New Default File source ' + [defCode] + ' created '  FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'SOURCE'
	COMMIT TRANSACTION
END
 
-- Create default 'Department' for Import
IF NOT EXISTS ( SELECT [deptCode] FROM [dbo].[dbDepartment] DE JOIN [OMSImport].[dbo].[Defaults] D ON DE.[deptCode]  = D.[defCode] WHERE D.[defType] = 'DEPT' )
BEGIN
	 BEGIN TRANSACTION
	 INSERT [dbo].[dbDepartment] ( [deptCode] )
	 SELECT [defCode] FROM  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'DEPT'
	 -- Error check and logging
	 IF @@Error <> 0 
	  BEGIN
		   ROLLBACK TRANSACTION
		   INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		   VALUES ( 'Error Creating default Department' )
		   GOTO ERRORHANDLER
	  END
	IF  NOT EXISTS ( SELECT [cdID] FROM [dbo].[dbCodeLookup] C JOIN [OMSImport].[dbo].[Defaults] D ON C.[cdType] = D.[defType] AND C.[cdCode] = D.[defCode] WHERE D.[defType] = 'DEPT' )
	BEGIN
		  INSERT [dbo].[dbCodeLookup] ( [cdType] , [cdCode] , [cdDesc] )
		  SELECT 'DEPT' ,  [defCode] , [defDesc] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'DEPT'
		  -- Error check and logging
		  IF @@Error <> 0 
		  BEGIN
			   ROLLBACK TRANSACTION
			   INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			   VALUES ( 'Error  Creating default Department Code Lookup' )
			   GOTO ERRORHANDLER
		  END
		 -- Write to log
		 INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		 SELECT 1  , 'New Default Department ' + [defCode] + ' created '  FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'DEPT'
	 END
	COMMIT TRANSACTION
END
 

-- Create default 'User' and Fee Earner for Import
IF  NOT EXISTS ( SELECT [usrID] FROM [dbo].[dbUser] U JOIN [OMSImport].[dbo].[Defaults] D ON U.[usrID]  = D.[defCode] WHERE D.[defType] = 'USER' )
BEGIN
	 BEGIN TRANSACTION
	 SET IDENTITY_INSERT [dbo].[dbUser] ON
	 INSERT  [dbo].[dbUser] ( [usrID] , [usrADID] , [usrSQLID] , [usrFullName] , [usrActive] , [usrWorksFor] , [usrInits] , [usrAlias] , [usrUICultureInfo] , [usrcurISOCode] )
	 SELECT 
	  [defCode] ,
	  [defCode] ,
	  [defCode] ,
	  [defDesc] , 
	  1 ,
	   -1 , 
	  'IMPORT' , 
	  'IMPORT' ,
	  ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERCULTURE' ) ,
	  ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERISOCODE' ) 
	 FROM  
	  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' 
	 -- Error check and logging
	 IF @@Error <> 0 
	 BEGIN
		   ROLLBACK TRANSACTION
		   INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		   VALUES ( 'Error Creating default User' )
		   SET IDENTITY_INSERT [dbo].[dbUser] OFF
		   GOTO ERRORHANDLER
	 END
	 SET IDENTITY_INSERT [dbo].[dbUser] OFF
	 INSERT INTO [dbo].[dbFeeEarner] ( [feeUsrID] , [feeSignOff] , [feeActive] , [feeResponsible] , [feeResponsibleTo] )
	 SELECT Coalesce ( [defCode] , -200 ) , [defDesc] , 1 , 0 , -1 FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' 
	 -- Error check and logging
	 IF @@Error <> 0 
		  BEGIN
			   ROLLBACK TRANSACTION
			   INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			   VALUES ( 'Error Creating default Fee Earner' )
			   GOTO ERRORHANDLER
 		 END
		  INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		 SELECT 1  , 'New Default User and Fee Earner ' + [defCode] + ' created '  FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER'
	 COMMIT TRANSACTION
END
 

-- Create default file type for Import
IF NOT EXISTS ( SELECT [typeCode] FROM [dbo].[dbFileType] FT JOIN [OMSImport].[dbo].[Defaults] D ON D.[defCode]  = FT.[typeCode] WHERE D.[defType] = 'FILETYPE' )
BEGIN
	 BEGIN TRANSACTION
	 INSERT [dbo].[dbFileType] ( [typeCode] , [fileDeptCode] , [fileDefFundCode] , [fileAccCode] , [typeVersion] , [typeSeed] , [typeGlyph] , [typeXML] )
	 SELECT 
	  ( SELECT [defCode] FROM  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FILETYPE' ) ,
	  ( SELECT [defCode] FROM  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'DEPT' ) ,
	  ( SELECT [defCode] FROM  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FUNDTYPE' ) ,
	  ( SELECT [defCode] FROM  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FILETYPE' ) ,
	  1 ,
	  'FI' ,
	  1 ,
	  [typeXML]
	 FROM [dbo].[dbFileType] WHERE [typeCode] = 'TEMPLATE' 
	 -- Error check and logging
	 IF @@Error <> 0 
	  BEGIN
		   ROLLBACK TRANSACTION
		   INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		   VALUES ( 'Error Creating default File type' )
		   GOTO ERRORHANDLER
	  END
	 IF  NOT EXISTS ( SELECT [cdID] FROM [dbo].[dbCodeLookup] C JOIN [OMSImport].[dbo].[Defaults] D ON C.[cdType] = D.[defType] AND C.[cdCode] = D.[defCode] WHERE D.[defType] = 'FILETYPE' )
	 BEGIN
		  INSERT [dbo].[dbCodeLookup] ( [cdType] , [cdCode] , [cdDesc] )
		  SELECT 'FILETYPE' ,  [defCode] , [defDesc] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FILETYPE'
		  -- Error check and logging
		  IF @@Error <> 0 
		  BEGIN
			   ROLLBACK TRANSACTION
			   INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			   VALUES ( 'Error  Creating default File type Code Lookup' )
			   GOTO ERRORHANDLER
		  END
		  -- Write to log
		  INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		  SELECT 1  , 'New Default Filetype ' + [defCode] + ' created '  FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FILETYPE'
	 END
 	COMMIT TRANSACTION
END
 
 
 
-- Insert default 'unknown address'
IF NOT EXISTS ( SELECT [addID] FROM [dbo].[dbAddress] WHERE [addID] = ( SELECT [defCode] FROM  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'UKNADDRESS' ))
BEGIN
	BEGIN TRANSACTION
	SET IDENTITY_INSERT [dbo].[dbAddress] ON
	INSERT [dbo].[dbAddress]
		 (
	 	  [addID] ,
		   [addLine1] ,
		   [Created] ,
		   [CreatedBy]
		   )
	SELECT
		[defCode] ,
		[defDesc] ,
		getdate() ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' )
	FROM
		[OMSImport].[dbo].[Defaults] WHERE [defType] = 'UKNADDRESS' 
	 -- Error check and logging
	IF @@Error <> 0 
	BEGIN
		ROLLBACK TRANSACTION
		SET IDENTITY_INSERT [dbo].[dbAddress] OFF
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error creating Default unknown address ID' )
		GOTO ERRORHANDLER
	END
	SET IDENTITY_INSERT [dbo].[dbAddress] OFF
	INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
	VALUES ( 1 , 'New Default address ID created ' ) 
	COMMIT TRANSACTION
END
 
 
 
--Create default 'Client type' for Import
IF  NOT EXISTS ( SELECT [typeCode] FROM [dbo].[dbClientType] CT JOIN [OMSImport].[dbo].[Defaults] D ON CT.[typeCode] = D.[defCode]  WHERE D.[defType] = 'CLTYPE' )
BEGIN
	BEGIN TRANSACTION
	INSERT [dbo].[dbClientType] ( [typeCode] , [typeVersion] , [typeXML] , [typeGlyph] )
	SELECT
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'CLTYPE' ) ,
		1 ,
		[typeXML] ,
		[typeGlyph]
		FROM
		[dbo].[dbClientType]
	WHERE 
	[typeCode] = 'TEMPLATE' 
	-- Error check and logging
	IF @@Error <> 0 
	BEGIN
		ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error Creating default Client type' )
		GOTO ERRORHANDLER
	END
	-- Write to log
	INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
	SELECT 1  , 'New Default Client type ' + [defCode] + ' created '  FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'CLTYPE'
	IF  NOT EXISTS ( SELECT [cdID] FROM [dbo].[dbCodeLookup] C JOIN [OMSImport].[dbo].[Defaults] D ON C.[cdType] = D.[defType] AND C.[cdCode] = D.[defCode] WHERE D.[defType] = 'CLTYPE' )
	BEGIN
		INSERT [dbo].[dbCodeLookup] ( [cdType] , [cdCode] , [cdDesc] )
		SELECT 'CLTYPE' ,  [defCode] , [defDesc] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'CLTYPE'
		-- Error check and logging
		IF @@Error <> 0 
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error  Creating default Client type Code Lookup' )
			GOTO ERRORHANDLER
		END
		-- Write to log
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		SELECT 1  , 'New Client type ' + [defCode] + ' created '  FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'CLTYPE'
	END
	COMMIT TRANSACTION
END
 

-- Insert new File types
BEGIN TRANSACTION
	INSERT [dbo].[dbFileType] ( [typeCode] , [fileDeptCode] , [fileDefFundCode] , [fileAccCode] , [typeVersion] , [typeSeed] , [typeGlyph] )
	SELECT 
		Distinct ( F.[fileType] + '-' + [fileDept] )as FileType  , 
		coalesce ( [fileDept] , ( SELECT [defCode] FROM  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'DEPT' ) ) as dept ,
		coalesce (( SELECT TOP 1  [fileFundCode] FROM [OMSImport].[dbo].[FileDetails] WHERE [filetype] = F.[filetype] AND [fileDept] =  F.[fileDept] ) , ( SELECT  [defCode] FROM  [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FUNDTYPE' ) ) as fund ,
		F.[fileType] ,
		1 ,
		'FI' ,
		1 
	FROM 
		[OMSImport].[dbo].[FileDetails] F 
	WHERE 
		F.[fileType] + '-' + F.[fileDept] NOT IN ( SELECT [typeCode] FROM [dbo].[dbFileType] ) 
	--Now create XML entries
	DECLARE @ptr varbinary(16)
	DECLARE @ptr2 varbinary(16)
	SELECT @ptr = textptr([typexml]) FROM [dbo].[dbFileType] WHERE [typeCode] = 'TEMPLATE'
	DECLARE @TYPECODE nvarchar(15)
	DECLARE XML_Cursor CURSOR FOR
	SELECT [typeCode] FROM [dbo].[dbFileType] WHERE [typeXML] LIKE '<config/>'
	OPEN XML_Cursor
	FETCH NEXT FROM XML_Cursor INTO @TYPECODE
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		SET @ptr2 = ( SELECT  textptr ( [typeXML] ) FROM [dbFileType] WHERE [typeCode] = @TYPECODE)
		UPDATETEXT [dbo].[dbFileType].[typeXML] @ptr2 0 NULL [dbo].[dbFileType].[typeXML] @ptr
		FETCH NEXT FROM XML_Cursor INTO @TYPECODE
	END
	CLOSE XML_Cursor
	DEALLOCATE XML_Cursor
	-- Error check and logging
	IF @@Error <> 0 
		BEGIN
		ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error  Creating new File types' )
		GOTO ERRORHANDLER
	END
	-- Create new file type codeLookups
	 INSERT [dbo].[dbCodeLookup] ( [cdCode] , [cdDesc] , [cdType]  )
	SELECT 
		Distinct ( F.[fileType] + '-' + F.[fileDept]) ,
		Coalesce ( C.[cdDesc] , F.[fileType] ) ,
		'FILETYPE'
	FROM
		[OMSImport].[dbo].[FileDetails] F 
	LEFT JOIN
		[OMSImport].[dbo].[CodeLookup] C ON F.[fileType] = C.[cdCode] AND C.[cdType] = 'FILETYPE'
	WHERE 
	F.[fileType] + '-' + F.[fileDept] NOT IN ( SELECT [cdCode] FROM [dbo].[dbCodeLookup] WHERE [cdType] = 'FILETYPE' ) 
	SELECT @Rows = @@ROWCOUNT , @Error = @@Error
	-- Error check and logging
	IF @Error <> 0
	BEGIN
		ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error  Creating new File type Code Lookups' )
		GOTO ERRORHANDLER
	END
	-- Update Log
	IF @Rows > 0
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		VALUES ( @Rows , 'New File Types Created' )
	END
COMMIT TRANSACTION
 

-- Insert New Departments
BEGIN TRANSACTION
	INSERT [dbo].[dbDepartment] ( [deptCode] , [deptActive] )
	SELECT DISTINCT ( [fileDept] ) , 1 FROM [OMSImport].[dbo].[FileDetails] WHERE [fileDept] NOT IN (SELECT [deptCode] FROM [dbo].[dbDepartment] )
	-- Error check and logging
	IF @@Error <> 0 
	BEGIN
		ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error Creating new departments' )
		 GOTO ERRORHANDLER
	END
	-- Create new Department codeLookups
	INSERT dbcodelookup ( [cdCode] , [cdDesc] ,  [cdType] )
	SELECT 
		DISTINCT F.[fileDept] , coalesce ( C.[cdDesc] , 'No code set' ) , 'DEPT'
	FROM 
		[OMSImport].[dbo].[FileDetails] F LEFT JOIN [OMSImport].[dbo].[CodeLookup] C ON F.[fileDept] = C.[cdCode]
	WHERE 
		C.[cdType] = 'DEPT' AND [fileDept] NOT IN ( SELECT [cdCode] FROM [dbo].[dbCodeLookup] WHERE [cdType] = 'DEPT' )
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error check and logging
	IF @Error <> 0 
	BEGIN
		ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error Creating new department Code Lookups' )
		GOTO ERRORHANDLER
	END
	-- Write to log
	IF @Rows > 0
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		VALUES ( @Rows , 'New Departments Created' )
	END
COMMIT TRANSACTION
 
 
 
-- Insert New additional Code Lookups
BEGIN TRANSACTION
	INSERT dbcodelookup ( [cdCode] , [cdDesc] ,  [cdType] )
	SELECT 
		C.[cdCode] , C.[cdDesc] , C.[cdType] 
	FROM
		[OMSImport].[dbo].[CodeLookup] C 
	LEFT JOIN
		[dbo].[dbCodeLookup] CL ON CL.[cdType] = C.[cdType] AND CL.[cdCode] = C.[cdCode]
	WHERE
		CL.[cdType] IS NULL AND C.[cdType] NOT IN ( SELECT [defType] FROM [OMSImport].[dbo].[Defaults] )
	SELECT @Rows = @@ROWCOUNT , @Error = @@Error
	-- Error check and logging
	IF @Error <> 0 
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error creating new misc Code Lookups' )
		GOTO ERRORHANDLER
	END
	IF @Rows > 1
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		VALUES ( @Rows , 'New misc Code Lookups Created' )
	END
COMMIT TRANSACTION
 
 
 
-- Insert the new Contact types
BEGIN TRANSACTION
	INSERT [dbo].[dbContactType] ( [typeCode] , [typeVersion] , [typeGlyph] )
	SELECT 
		Distinct ( CD.[contType] ) , 1 , 1 
	FROM
		[OMSImport].[dbo].[ClientDetails] CD
	WHERE 
		CD.[contType] NOT IN ( SELECT [typeCode] FROM [dbo].[dbContactType] )
	--Now create XML entries
	SELECT @ptr = textptr([typexml]) FROM [dbo].[dbContactType] WHERE [typeCode] = 'TEMPLATE'
	DECLARE XML_Cursor CURSOR FOR
	SELECT [typeCode] FROM [dbo].[dbClientType] WHERE [typeXML] LIKE '<config/>'
	OPEN XML_Cursor
	FETCH NEXT FROM XML_Cursor INTO @TYPECODE
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		SET @ptr2 = (SELECT textptr ( [typeXML] ) FROM [dbo].[dbClientType] WHERE  [typeCode] = @TYPECODE )
		UPDATETEXT [dbo].[dbClientType].[typeXML] @ptr2 0 NULL [dbo].[dbClientType].[typeXML] @ptr
		FETCH NEXT FROM XML_Cursor INTO @TYPECODE
	END
	CLOSE XML_Cursor
	DEALLOCATE XML_Cursor
	-- Error check and logging
	IF @@Error <> 0 
	BEGIN
	ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error  Creating new Contact types' )
		GOTO ERRORHANDLER
	END
	-- Insert the new coodelookups
	INSERT dbcodelookup ( [cdCode] , [cdDesc] ,  [cdType] )
	SELECT 
		Distinct ( CD.[contType] ) , Coalesce ( CL.[cdDesc] , CD.[contType] )  , 'CONTTYPE' 
	FROM
		[OMSImport].[dbo].[ClientDetails] CD
	LEFT JOIN 
		[OMSImport].[dbo].[CodeLookup] CL ON CD.[contType] = CL.[cdCode] AND CL.[cdType] = 'CONTTYPE'
	WHERE 
		CD.[contType] NOT IN ( SELECT [cdCode] FROM [dbo].[dbCodeLookup] WHERE [cdType] = 'CONTTYPE' )
	SELECT @Rows = @@Rowcount , @Error = @@Error
	-- Error checking and logging
	IF @@Error <> 0 
	BEGIN
		ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error  Creating new Contact type Code Lookups' )
		GOTO ERRORHANDLER
	END
	IF @Rows > 1
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		VALUES ( @Rows , 'New Contact types Created' )
	END
COMMIT TRANSACTION
 

-- Insert new Client types
BEGIN TRANSACTION
	INSERT 
	[dbo].[dbClientType] ( [typeCode] , [typeVersion] , [typeGlyph] )
	SELECT 
		Distinct ( CD.[clType] ) , 1 , 1 
	FROM 
		[OMSImport].[dbo].[ClientDetails] CD
	WHERE 
		CD.[clType] NOT IN ( SELECT [typeCode] FROM [dbo].[dbClientType] ) 
	--Now create XML entries
	SELECT @ptr = textptr([typexml]) FROM [dbo].[dbClientType] WHERE [typeCode] = 'TEMPLATE'
	DECLARE XML_Cursor CURSOR FOR
	SELECT [typeCode] FROM [dbo].[dbClientType] WHERE [typeXML] LIKE '<config/>'
	OPEN XML_Cursor
	FETCH NEXT FROM XML_Cursor INTO @TYPECODE
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		SET @ptr2 = ( SELECT  textptr ( [typeXML] ) FROM [dbClientType] WHERE [typeCode] = @TYPECODE)
		UPDATETEXT [dbo].[dbClientType].[typeXML] @ptr2 0 NULL [dbo].[dbClientType].[typeXML] @ptr
		FETCH NEXT FROM XML_Cursor INTO @TYPECODE
	END
	CLOSE XML_Cursor
	DEALLOCATE XML_Cursor
	-- Error check and logging
	IF @@Error <> 0 
	BEGIN
		ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error  Creating new Client types' )
		GOTO ERRORHANDLER
	END
	-- Create new Client type codeLookups
	INSERT [dbo].[dbCodeLookup] ( [cdCode] , [cdDesc] , [cdType]  )
	SELECT 
		Distinct ( CD.[clType] ) ,
		Coalesce ( C.[cdDesc] , CD.[clType] ) ,
		'CLTYPE'
	FROM
		[OMSImport].[dbo].[ClientDetails] CD
	LEFT JOIN
		[OMSImport].[dbo].[CodeLookup] C ON CD.[clType] = C.[cdCode] AND C.[cdType] = 'CLTYPE'
	WHERE 
		CD.[clType] NOT IN ( SELECT [cdCode] FROM [dbo].[dbCodeLookup] WHERE [cdType] = 'CLTYPE' ) AND CD.[clType] <> ''
	SELECT @Rows = @@ROWCOUNT , @Error = @@Error
	-- Error check and logging
	IF @Error <> 0
	BEGIN
		ROLLBACK TRANSACTION
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
		VALUES ( 'Error  Creating new Client type Code Lookups' )
		GOTO ERRORHANDLER
	END
	-- Update Log
	IF @Rows > 0
	BEGIN
		INSERT [OMSImport].[dbo].[ImportLog] ( [LogInserts] , [LogImportDesc] )
		VALUES ( @Rows , 'New Client Types Created' )
	END
COMMIT TRANSACTION
 

SET NOCOUNT OFF
RETURN
 
ERRORHANDLER:
SET NOCOUNT OFF
RETURN
GO






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE [dbo].[fwbsDISetDocDCFlags]

AS

DECLARE @Rows bigint , @Error int
SET NOCOUNT ON


-- Set the document table flag for orphan document records.
BEGIN TRANSACTION
	UPDATE 
		D
	SET 
		D.[DCFlag] = 8
	FROM 
		[OMSImport].[dbo].[Documents] D 
	LEFT JOIN
		[dbClient] C ON C.[clNo] = D.[clNo]
	LEFT JOIN
		[dbFile] F ON F.[fileNo] = D.[fileNo] AND F.[clId] = C.[clID]
	WHERE 
		C.[clNo] IS NULL OR F.[fileNo] IS NULL	
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting Document DCFlag 8' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogWarnings] , [LogImportDesc] )
			SELECT @Rows , 'Orphan Document records exist'
		END
COMMIT TRANSACTION
	


-- Set the Document table flag for new Documents
BEGIN TRANSACTION
	UPDATE 
		D
	SET 
		D.DCFlag = 1 
	FROM 
		[OMSImport].[dbo].[Documents] D 
	LEFT JOIN
		[dbDocument] DO ON D.[docID] = DO.[docIDOld] 
	WHERE 
		DO.[docID] IS NULL AND D.[DCFlag] IS NULL
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting Document DCFlag 1' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION


-- Set the document flag for existing records 
BEGIN TRANSACTION
	UPDATE 
		D
	SET 
		D.DCFlag = 9 
	FROM 
		[OMSImport].[dbo].[Documents] D 
	JOIN
		[dbDocument] DO ON D.[docID] = DO.[docIDOld] 
	WHERE 
		D.[DCFlag] IS  NULL OR D.[DCFlag] NOT IN ( 1 , 8 )
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting Document DCFlag 9' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogWarnings] , [LogImportDesc] )
			SELECT @Rows , 'Duplicate Document records exist'
		END
COMMIT TRANSACTION



SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET NOCOUNT OFF
RETURN





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE PROCEDURE [dbo].[fwbsDIsetClientDCFlags]

AS

DECLARE @Rows bigint , @Error int
SET NOCOUNT ON


-- Set FeeEarner table flag for new records
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[FeeEarner]
	SET [DCFlag] = 1
	WHERE   [OMSImport].[dbo].[FeeEarner].[usrID] NOT IN ( SELECT [usrID] FROM [dbo].[dbUser] ) AND  [OMSImport].[dbo].[FeeEarner].[Alias] NOT IN ( SELECT [usrAlias] FROM [dbo].[dbUser] ) 
	-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting User DCFlag 1' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION


-- Set FeeEarner table flag for duplicate records
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[FeeEarner]
	SET [DCFlag] = 9 , [Imported] = getdate()
	WHERE (  [OMSImport].[dbo].[FeeEarner].[usrID] IN ( SELECT [usrID] FROM [dbo].[dbUser] ) OR  [OMSImport].[dbo].[FeeEarner].[Alias] IN ( SELECT [usrAlias] FROM [dbo].[dbUser] ))  
		--AND (  [OMSImport].[dbo].[FeeEarner].[DCFlag] <> 9 OR [OMSImport].[dbo].[FeeEarner].[DCFlag] IS NULL )
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting User DCFlag 9' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogWarnings] , [LogImportDesc] )
			SELECT @Rows , 'Duplicate Fee Earner records exist'
		END
COMMIT TRANSACTION


-- Set ClientDetails table DCFlag for new Client and Contact records
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[ClientDetails]
	SET [DCFlag] = 1
	WHERE  [OMSImport].[dbo].[ClientDetails].[clNo] NOT IN ( SELECT [clNo] FROM [dbo].[dbClient] ) AND  [OMSImport].[dbo].[ClientDetails].[extcontID] NOT IN (  SELECT [contExtTxtID] FROM [dbo].[dbContact] WHERE [contExtTxtID] IS NOT NULL )
	-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting ClientDetails DCFlag 1' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION


-- Set ClientDetails table DCFlag for new Contact on existing Client records
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[ClientDetails]
	SET [DCFlag] = 2
	WHERE  [OMSImport].[dbo].[ClientDetails].[clNo]  IN ( SELECT [clNo] FROM [dbo].[dbClient] ) AND  [OMSImport].[dbo].[ClientDetails].[extContID] NOT IN ( SELECT [contExtTxtID] FROM [dbo].[dbContact] WHERE [contExtTxtID] IS NOT NULL )
	-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting ClientDetails DCFlag 2' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION


-- Set ClientDetails table DCFlag for new Client but existing Contact records
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[ClientDetails]
	SET [DCFlag] = 3
	WHERE  [OMSImport].[dbo].[ClientDetails].[clNo] NOT IN ( SELECT [clNo] FROM [dbo].[dbClient] ) AND  [OMSImport].[dbo].[ClientDetails].[extContID] IN ( SELECT [contExtTxtID] FROM [dbo].[dbContact] WHERE [contExtTxtID] IS NOT NULL)
	-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting ClientDetails DCFlag 3' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION


-- Set ClientDetails table DCFlag update Contact defaultAddress records
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[ClientDetails]
	SET [DCFlag] = 4
	WHERE   [OMSImport].[dbo].[ClientDetails].[extContID] IN ( SELECT [contExtTxtID] FROM [dbo].[dbContact] WHERE [contDefaultAddress] <> [OMSImport].[dbo].[ClientDetails].[OMSaddID] )
		AND  [OMSImport].[dbo].[ClientDetails].[clNo] IN ( SELECT [clNo] FROM [dbo].[dbClient] ) 
	-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting ClientDetails DCFlag 4' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION

/*		NOT USED 
-- Set ClientDetails table DCFlag for new Contact records with no associated Client
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[ClientDetails]
	SET [DCFlag] = 5
	WHERE  [OMSImport].[dbo].[ClientDetails].[clNo] IS NULL AND  [OMSImport].[dbo].[ClientDetails].[extcontID] NOT IN (  SELECT [contExtTxtID] FROM [dbo].[dbContact] WHERE [contExtID] IS NOT NULL )
	-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting ClientDetails DCFlag 5' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION
*/

-- Set ClientDetails table DCFlag for duplicate records
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[ClientDetails]
	SET [DCFlag] = 9 , [Imported] = getdate()
	WHERE   [OMSImport].[dbo].[ClientDetails].[extContID] IN ( SELECT [contExtTxtID] FROM [dbo].[dbContact] WHERE [contDefaultAddress] = [OMSImport].[dbo].[ClientDetails].[OMSaddID] )
		AND  [OMSImport].[dbo].[ClientDetails].[clNo] IN ( SELECT [clNo] FROM [dbo].[dbClient] ) 
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Rows = @@Error
	IF @Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting ClientDetails DCFlag 9' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogWarnings] ,  [LogImportDesc] )
			SELECT @Rows , 'Duplicate Client\Contact records exist'
		END 
COMMIT TRANSACTION



SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET NOCOUNT OFF
RETURN



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROCEDURE [dbo].[fwbsDIsetFileDCFlags]

AS

DECLARE @Rows bigint , @Error int
SET NOCOUNT ON


-- Set the FileDetails table DCFlag for Duplicate File records
BEGIN TRANSACTION
	UPDATE FD
	SET FD.[DCFlag] = 9 , FD.[Imported] = getdate()
	FROM [OMSImport].[dbo].[FileDetails] FD
	JOIN [dbo].[dbClient] C ON FD.[clNo] = C.[clNo]
	WHERE FD.[fileNo] IN ( SELECT F1.[fileNo] FROM [dbo].[dbFile] F1 JOIN [dbo].[dbClient] C1 ON F1.[clID] = C1.[clID] WHERE C1.[clNo] = C.[clNo] )
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting FileDetails DCFlag 9' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogWarnings] , [LogImportDesc] )
			SELECT @Rows , 'Duplicate file records exist'
		END
COMMIT TRANSACTION


-- Set the FileDetails table DCFlag for new File records
BEGIN TRANSACTION
	UPDATE FD
	SET [DCFlag] = 1
	FROM [OMSImport].[dbo].[FileDetails] FD
	JOIN [dbo].[dbClient] C ON FD.[clNo] = C.[clNo]
	WHERE FD.[fileNo] NOT IN ( SELECT F1.[fileNo] FROM [dbo].[dbFile] F1 JOIN [dbo].[dbClient] C1 ON F1.[clID] = C1.[clID] WHERE C1.[clNo] = C.[clNo] )
		-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting FileDetails DCFlag 1' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION


-- Set the FileDetails table DCFlag for File records with no Client
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[FileDetails]
	SET [DCFlag] = 8 , [Imported] = getdate()
	WHERE  [OMSImport].[dbo].[FileDetails].[clNo] NOT IN ( SELECT [clNo] FROM [dbo].[dbClient] ) AND (  [OMSImport].[dbo].[FileDetails].[DCFlag] <> 8 OR  [OMSImport].[dbo].[FileDetails].[DCFlag] IS NULL )
	-- Error handling and logging
	SELECT @Rows = @@Rowcount , @Error = @@Error
	IF @Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting FileDetails DCFlag 8' )
			GOTO ERRORHANDLER
		END
	IF @Rows > 0
		BEGIN
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogWarnings] , [LogImportDesc] )
			SELECT @Rows , 'Orphan file records exist'
		END
COMMIT TRANSACTION


/* 

Not using file updates


-- Set the FileDetails table DCFlag for updates to File record
BEGIN TRANSACTION
	UPDATE [OMSImport].[dbo].[FileDetails]
	SET [DCFlag] = 6 , [Imported] = getdate()
	FROM [OMSImport].[dbo].[FileDetails] 
	JOIN [dbo].[dbClient] ON [OMSImport].[dbo].[FileDetails].[clNo] = [dbo].[dbClient].[clNo]
	JOIN [dbo].[dbFile] ON [dbo].[dbClient].[clID] = [dbo].[dbFile].[clID] AND[OMSImport].[dbo].[FileDetails].[fileNo] = [dbo].[dbFile].[fileNo]
	Set @Rows = @@Rowcount
	-- Error handling and logging
	IF @@Error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			INSERT [OMSImport].[dbo].[ImportLog] ( [LogImportDesc] )
			VALUES ( 'Error Setting FileDetails DCFlag 6' )
			GOTO ERRORHANDLER
		END
COMMIT TRANSACTION


*/



SET NOCOUNT OFF
RETURN

ERRORHANDLER:
SET NOCOUNT OFF
RETURN



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


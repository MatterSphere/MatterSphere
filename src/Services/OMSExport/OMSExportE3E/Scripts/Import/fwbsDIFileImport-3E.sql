/****** Object:  StoredProcedure [dbo].[fwbsDIFileImport]    Script Date: 05/09/2013 10:16:49 ******/
DROP PROCEDURE [dbo].[fwbsDIFileImport]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[fwbsDIFileImport]

AS

DECLARE @Rows bigint , @Error int
SET NOCOUNT ON


-- UPDATED FOR 3E INTEGRATION
-- * EXTRA EXTERNAL ID FOR MATTER INDEX



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
		[brID] ,
		FILEEXTLINKID	-- FOR 3E MATTINDEX
		)
	SELECT C.[clID] , FD.[fileNo] , FD.[fileDesc] , 
		coalesce ( FD.[fileResponsibleID] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) ) ,
		coalesce ( FD.[filePrincipleID] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' )  ) ,
		coalesce ( FD.[fileDept] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'DEPT' )  ) ,
		coalesce ( FD.[fileType] , ( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FILETYPE' )  ) ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'FUNDTYPE' ) ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USERISOCODE' ) , 
		FD.[fileStatus] ,
		coalesce ( FD.[fileCreated] , getutcdate() ) ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'USER' ) ,
		FD.[fileUpdated] ,
		FD.[fileClosed] ,
		( SELECT [defCode] FROM [OMSImport].[dbo].[Defaults] WHERE [defType] = 'SOURCE' ) ,
		( SELECT TOP 1 [brID] FROM [dbo].[dbRegInfo] ) ,
		MattIndex
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

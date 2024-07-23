
CREATE PROCEDURE [dbo].[sprArchiveDocumentData] ( @docID bigint
												 , @logMessage nvarchar (200)
												 , @usrID int = -1)
as
	declare @proceed bit 
	SET @Proceed = 1
	declare @logType nvarchar (15) 
	set @logtype = 'DMSARCHIVED'		
	declare @logCode nvarchar (15) 
	set @logcode = NULL	
	
	begin			
		insert into dbDocumentLog 
		(
			docID
			, verID
			, logType
			, logCode
			, usrID
			, logTime
			, LogData
		)
		values
		(
			@docID
			, null
			, @logType		
			, @logCode
			, @usrID
			, GETDATE()
			, @logMessage
		)
	end
	
	
	
	
		
		-- PRINT N'Error = ' + CAST(@@ERROR AS NVARCHAR(8));

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprArchiveDocumentData] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprArchiveDocumentData] TO [OMSAdminRole]
    AS [dbo];


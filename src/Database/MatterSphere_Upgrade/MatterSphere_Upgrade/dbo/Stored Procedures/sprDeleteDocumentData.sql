
CREATE PROCEDURE [dbo].[sprDeleteDocumentData] ( @docID bigint
												 , @logMessage nvarchar (200)
												 , @usrID int = -1)
as
	declare @proceed bit
	SET @proceed = 1
	declare @logType nvarchar (15) 
	SET @logtype = 'DMSDELETED'		
	declare @logCode nvarchar (15) 
	SET @logcode = NULL
	
	begin tran transDelete

	if exists (select * from dbDocumentVersionPreview where verID in (select verID from dbDocumentVersion where docID = @docID))
	begin
		delete from dbDocumentVersionPreview 
		where verID in (select verID from dbDocumentVersion where docID = @docID)

		if @@error <> 0
		begin	
			rollback tran transDelete	
			set @proceed = 0
		end
	end


	if exists (select * from dbDocumentPreview where docID = @docID)
	begin
		delete from dbDocumentPreview 
		where docID = @docID

		if @@error <> 0
		begin	
			rollback tran transDelete	
			set @proceed = 0
		end
	end

	
	if exists (select * from dbDocumentEmail where docID = @docID)
	begin
		delete from dbDocumentEmail 
		where docID = @docID

		if @@error <> 0
		begin	
			rollback tran transDelete	
			set @proceed = 0
		end
	end


	if exists (select * from dbDocumentAssociation where DocumentID = @docID)
	begin
		delete from dbDocumentAssociation 
		where DocumentID = @docID

		if @@error <> 0
		begin	
			rollback tran transDelete	
			set @proceed = 0
		end
	end


	if exists (select * from dbDocumentStorage where docID = @docID)
	begin
		delete from dbDocumentStorage
		where docID = @docID

		if @@error <> 0
		begin	
			rollback tran transDelete	
			set @proceed = 0
		end
	end

	
	if exists (select * from dbDocument where docID = @docID)
	begin
		update dbDocument 
		set docFlags = docFlags | 1
			, docDeleted = 1
			, docdirID = NULL
			, docFileName = ''
		where docID = @docID

		if @@error <> 0
		begin	
			rollback tran transDelete	
			set @proceed = 0
		end
	end


	if (@proceed = 1)
	begin
		commit tran tranDelete
	
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
	
	--- *** use goto
	--- *** return -1
	
	
		
		-- PRINT N'Error = ' + CAST(@@ERROR AS NVARCHAR(8));

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDeleteDocumentData] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDeleteDocumentData] TO [OMSAdminRole]
    AS [dbo];


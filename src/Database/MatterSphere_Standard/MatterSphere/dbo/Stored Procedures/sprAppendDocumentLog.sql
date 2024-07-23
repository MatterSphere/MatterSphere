CREATE PROCEDURE [dbo].[sprAppendDocumentLog] ( @docID bigint
												, @verID uniqueidentifier
												, @logType nvarchar
												, @logCode nvarchar
												, @usrID int
												, @logTime datetime )

as
	insert into dbDocumentLog
	(
		docID
		, verID
		, logType
		, logCode
		, usrID
		, logTime
	)
	values
	(
		@docID
		, @verID
		, @logType
		, @logCode
		, @usrID
		, @logTime
	)



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAppendDocumentLog] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAppendDocumentLog] TO [OMSAdminRole]
    AS [dbo];


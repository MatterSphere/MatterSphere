
CREATE PROCEDURE [dbo].[sprInserttoDocumentArchiveRun] ( @runStartTime datetime
														,@runguid uniqueidentifier														
														, @runid bigint out)
as

	insert into dbDocumentArchiveRun
	(
		runStartTime
		,runguid		
	)
	values
	(
		@runStartTime,
		@runguid	
	)

 
 select @runid=RunID 
 from dbDocumentArchiveRun 
 where runguid=@runguid

 return @runid

  

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprInserttoDocumentArchiveRun] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprInserttoDocumentArchiveRun] TO [OMSAdminRole]
    AS [dbo];



CREATE PROCEDURE [dbo].[sprAppendDocumentArchiveRun] ( @runguid uniqueidentifier
														, @runEndTime datetime
														, @runTotalDocs int
														, @runProcessedDocs int
														, @runProcessedFiles int
														, @runCompleted	bit
														, @runMessage nvarchar(max)
														, @runProcessedArchiveInfoIDs nvarchar(max)
														, @delimiter nvarchar(1) = ','
														 )
as


update dbDocumentArchiveRun
set runEndTime=@runEndTime,
    runTotalDocs=@runTotalDocs,
	runProcessedDocs=@runProcessedDocs,
	runProcessedFiles=@runProcessedFiles,
	runCompleted=@runCompleted,
	runMessage=@runMessage
where runguid=@runguid
	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAppendDocumentArchiveRun] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAppendDocumentArchiveRun] TO [OMSAdminRole]
    AS [dbo];


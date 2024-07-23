
CREATE PROCEDURE [dbo].[schSearchDocArchiveRun]
(	@UI uUICultureInfo = '{default}'   
	,@COMPLETED bit = null
	,@STARTDATE datetime = null
	,@ENDDATE datetime
)  
AS

select
	r.runid
	,r.runStartTime
	,r.runEndTime
	,r.runTotalDocs
	,r.runProcessedDocs
	,r.runCompleted
	,r.runMessage
	,r.runProcessedFiles
	,CASE runCompleted WHEN 0 THEN 64 ELSE null END AS [Image]
from
	dbdocumentarchiverun r
where
	runCompleted = ISNULL(@COMPLETED, runCompleted)
	and r.runStartTime between isnull(@startdate, runStartTime) and isnull(@enddate, runStartTime)
order by 
	r.runStartTime desc


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocArchiveRun] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocArchiveRun] TO [OMSAdminRole]
    AS [dbo];


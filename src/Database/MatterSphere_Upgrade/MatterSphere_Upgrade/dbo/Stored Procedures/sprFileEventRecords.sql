CREATE PROCEDURE [dbo].[sprFileEventRecords] (@FILEID bigint,  @UI uUICultureInfo = '{default}')  AS

--EVENTS made changes to fileevents query because its keeping lot of data in memory along with file.
select   dbfileevents.*, COALESCE(CL1.cdDesc, '~' + NULLIF(dbfileevents.evtype, '') + '~') as [evTypeDesc] , usrfullname 
from dbfileevents 
left join dbuser USR on evusrid = USR.usrid 
LEFT JOIN dbo.GetCodeLookupDescription ( 'FILEEVENT', @UI ) CL1 ON CL1.[cdCode] =  dbfileevents.evtype
where fileid = @FILEID
order by evwhen desc



CREATE PROCEDURE [dbo].[sprFileRecord] (@FILEID bigint,  @UI uUICultureInfo = '{default}')  AS
-- FILE
select * from dbfile where fileid = @FILEID
--EVENTS made changes to fileevents query because its keeping lot of data in memory along with file.
select  top 0 *, dbo.GetCodeLookupDesc('FILEEVENT', evtype, @UI) as [evTypeDesc] , usrfullname 
from dbfileevents 
left join dbuser USR on evusrid = USR.usrid 
where fileid = @FILEID
order by evwhen desc
--PHASES
select * from dbfilephase where fileid = @FILEID and phactive = 1 order by Created


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFileRecord] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFileRecord] TO [OMSAdminRole]
    AS [dbo];


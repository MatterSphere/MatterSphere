

CREATE PROCEDURE [dbo].[sprTimeGetWipWithDates]
(
	@FILEID bigint,
	@DATE1 datetime = null,
	@DATE2 datetime = null
)

 AS

declare @SQL nvarchar(1000)
declare @WHERE nvarchar(1000)
set @WHERE = ''
set @SQL = '

select 
	fileid, 
	sum(timecharge) as sumoftimecharge
from
	dbtimeledger T 
where
	T.fileid = @FILEID and 
	T.timebilled = 0 '

if isdate(@DATE1) = 1 and isdate(@DATE2) = 1 
	set @WHERE = ' and T.timerecorded >= @DATE1 and T.timerecorded < @DATE2 '
if isdate(@DATE1) = 0 and isdate(@DATE2) = 1
	set @WHERE = ' and T.timerecorded < @DATE2 '

set @SQL = @SQL + @WHERE + ' group by T.fileid'
print @sql
exec sp_executesql @SQL, N'@FILEID bigint, @DATE1 datetime, @DATE2 datetime', @FILEID, @DATE1, @DATE2

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeGetWipWithDates] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeGetWipWithDates] TO [OMSAdminRole]
    AS [dbo];


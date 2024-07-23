

CREATE PROCEDURE [dbo].[sprTimeWriteOff] 
(
	@FILEID bigint,			--File ID
	@BILLREF nvarchar(30) ='',	--Update time entry to this billnumber
	@AMOUNT money = 0,		--Amount to write off
	@DATE1 datetime = null,	--If allocating between date ranges....
	@DATE2 datetime = null
)

AS

declare @WHERE nvarchar(1000) 
declare @sql nvarchar(1000)

set @WHERE = ''

if @FILEID <>0 
BEGIN
	
	if isdate(@date1) = 1 AND isdate(@date2) = 1 	 --Date 1 and 2 entered	
		set @where = ' (T.timerecorded >= @DATE1 AND T.timerecorded < @DATE2 ) '
	if isdate(@date1) = 0 and isdate(@date2) = 1  	-- Date 2 (up to date) entered only
		set @where = ' T.timerecorded < @DATE2 '
	if isdate(@date1) = 0 and isdate(@date2) = 0 	--No dates entered so do all!
		set @where = ''
	
	if @where <>''  set @where = ' AND ' + @where 
	set @sql = 'UPDATE T  SET T.TIMEBILLED = 1, T.TIMEBILLNO = @BILLREF FROM DBTIMELEDGER T WHERE T.FILEID = @FILEID AND T.TIMEBILLED = 0  ' + @WHERE
	
	exec sp_executesql @sql, N'@fileid bigint, @billref nvarchar(30), @DATE1 DATETIME, @DATE2 DATETIME',@fileid, @billref, @DATE1, @DATE2
	
END
select @@rowcount, @@rowcount

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeWriteOff] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeWriteOff] TO [OMSAdminRole]
    AS [dbo];


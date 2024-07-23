

CREATE FUNCTION [dbo].[GetConflictResults] 
(
	@FILEID bigint,
	@UI nvarchar(10) = ''
)
RETURNS nvarchar(1000) AS  
BEGIN 
	DECLARE @TMPTYPE NVARCHAR(1000)
	DECLARE @TMPDESC NVARCHAR(1000)
	DECLARE @BUILD NVARCHAR(1000)

	--Set the Cursor
	declare cur cursor for
	select 
		coalesce(
			dbo.GetCodeLookupDesc('FILEEVENT',evType,'') 
			+ ' by ' + (select usrfullname from dbuser where usrid = FE.evUsrID)
			+ ' on ' + convert(nvarchar(20),evWhen) 
			+ '-' + convert(nvarchar(200),evExtended)
			,dbo.GetCodeLookupDesc('FILEEVENT',evType,'')) as evTypeDesc, 
		evDesc 
	from 
		dbfileevents FE
	where 
		fileid =@FILEID and 
		(evtype = 'CONFLICTSKIPPED' or 
		evType = 'CONFLICTDONE')

	set @BUILD = ''

	OPEN cur
	--Get First Record
	FETCH NEXT FROM cur INTO @TMPTYPE, @TMPDESC
	--Now Loop Through
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		SET @BUILD = @BUILD + coalesce(@TMPTYPE,'') + coalesce(@TMPDESC,'0')
		FETCH NEXT FROM cur INTO @TMPTYPE, @TMPDESC
	END
	--Finish Looping so tidy up.
	CLOSE cur
	DEALLOCATE cur
	

	return @BUILD

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConflictResults] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConflictResults] TO [OMSAdminRole]
    AS [dbo];


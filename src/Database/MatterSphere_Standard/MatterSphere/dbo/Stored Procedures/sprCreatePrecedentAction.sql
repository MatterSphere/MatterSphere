

CREATE PROCEDURE [dbo].[sprCreatePrecedentAction]
	@PRECID bigint, 
	@VERSION uniqueidentifier = null, 
	@ACTION nvarchar(15), 
	@SUBACTION nvarchar(15) = null, 
	@USRID int, 
	@DATA nvarchar(100) = null
AS

insert into dbprecedentlog(precid, verid, logtype, logcode, usrid, logdata) 
values(@precid, @version, @action, @subaction, @usrid, @data);

select * from dbprecedentlog where precid = @PRECID




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreatePrecedentAction] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreatePrecedentAction] TO [OMSAdminRole]
    AS [dbo];


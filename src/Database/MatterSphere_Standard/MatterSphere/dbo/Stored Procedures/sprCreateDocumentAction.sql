

CREATE PROCEDURE [dbo].[sprCreateDocumentAction]
	@DOCID bigint, 
	@VERSION uniqueidentifier = null, 
	@ACTION nvarchar(15), 
	@SUBACTION nvarchar(15) = null, 
	@USRID int, 
	@DATA nvarchar(100) = null
AS

insert into dbdocumentlog(docid, verid, logtype, logcode, usrid, logdata) 
values(@docid, @version, @action, @subaction, @usrid, @data);

select * from dbdocumentlog where docid = @DOCID



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreateDocumentAction] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreateDocumentAction] TO [OMSAdminRole]
    AS [dbo];


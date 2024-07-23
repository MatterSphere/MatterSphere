

CREATE PROCEDURE [dbo].[sprRaiseError] (@Code uCodeLookup, @UI uUICultureInfo = '{default}',  @Message nvarchar(500) output ) AS
declare @severity tinyint
	
select @severity = msgseverity, 
	@Message = dbo.GetCodeLookupDesc('MESSAGE', msgcode, @UI) 
from dbmessage 
where msgcode = @Code
return IsNull(@severity, 16)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRaiseError] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRaiseError] TO [OMSAdminRole]
    AS [dbo];


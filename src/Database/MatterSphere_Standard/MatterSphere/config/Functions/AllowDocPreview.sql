

CREATE FUNCTION [config].[AllowDocPreview] ( @DocID as bigint )

RETURNS bit

AS
BEGIN
DECLARE @USERID nvarchar(200) 
set @USERID = (select [config].[GetUserLogin]())

DECLARE @ALLOWACCESS bit

       IF exists (select 1 from sys.tables where name = 'dbDocument' and [schema_id] = SCHEMA_ID('config'))
       begin
              set @ALLOWACCESS = 0
              IF EXISTS (select 1 from item.[User] u where u.NTLogin = @USERID)
              begin
                     set @ALLOWACCESS = 1
                     if  EXISTS (select 1 FROM [config].[CheckDocumentAccess] (@USERID,@DocID) where VDeny = 1 or (VDeny is null and VAllow is null) and DocumentID is not null)
                           set @ALLOWACCESS = 0
              end
       end
       else
       begin
              set @ALLOWACCESS = 1       
       end

       RETURN @ALLOWACCESS
END




GO
GRANT EXECUTE
    ON OBJECT::[config].[AllowDocPreview] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[AllowDocPreview] TO [OMSAdminRole]
    AS [dbo];


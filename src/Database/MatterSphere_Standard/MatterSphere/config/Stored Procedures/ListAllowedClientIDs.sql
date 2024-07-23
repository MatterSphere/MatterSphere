

-- Returns Client IDs that you are allowed to see, based on your security permissions

CREATE PROCEDURE [config].[ListAllowedClientIDs]
       @ClientIDs nvarchar(max)
AS
BEGIN	   
       set nocount on;
       select items from dbo.SplitStringToTable(@ClientIDs, ',')  r 
       except
       select ClID from [config].[ClientAccess] () DA where  da.[Deny] = 1  
end

GO
GRANT EXECUTE
    ON OBJECT::[config].[ListAllowedClientIDs] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListAllowedClientIDs] TO [OMSAdminRole]
    AS [dbo];


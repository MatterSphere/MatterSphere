

-- Returns contactt IDs that you are allowed to see, based on your security permissions

CREATE PROCEDURE [config].[ListAllowedContactIDs]
       @ContactIDs nvarchar(max)
AS
BEGIN	   
       set nocount on;
       select items from dbo.SplitStringToTable(@ContactIDs, ',')  r 
       except
       select ContactID from [config].[ContactAccess] () DA where  da.[Deny] = 1  
end

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[config].[ListAllowedContactIDs] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListAllowedContactIDs] TO [OMSAdminRole]
    AS [dbo];


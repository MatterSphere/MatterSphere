

CREATE PROCEDURE [dbo].[fwbsServerStatus]
AS

EXEC master..xp_msver

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsServerStatus] TO [OMSAdminRole]
    AS [dbo];


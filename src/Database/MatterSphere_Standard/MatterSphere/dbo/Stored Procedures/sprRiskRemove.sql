

CREATE PROCEDURE [dbo].[sprRiskRemove]
(@fileid bigint) 
 AS
 
update dbriskheader set riskactive = 0 where fileid = @FILEID
 
SELECT @@ROWCOUNT, @@ROWCOUNT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRiskRemove] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRiskRemove] TO [OMSAdminRole]
    AS [dbo];


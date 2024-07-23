

-- Author:		<Conrad McLaughlin>
-- Create date: <22 June 2009>
-- Description:	<This will return who the contact works for>
-- =========================================================
CREATE PROCEDURE [dbo].[fdsprContactLinks_WorksFor]
(
	@ASSOCID AS BigInt
)	
AS

BEGIN

	DECLARE @CONTID As Bigint
	SET @CONTID = (SELECT CONTID FROM dbAssociates WHERE assocID = @ASSOCID)
	PRINT @CONTID
	
	DECLARE @CONTLINKID As Bigint
	SET @CONTLINKID = (select contlinkid from dbContactLinks where contlinkcode = 'EMPR' and contID = @CONTID)
	PRINT @CONTLINKID
	
	SELECT contname from dbContact where contID = @CONTLINKID
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_WorksFor] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_WorksFor] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[sprClientStatusPopulateTable]
(@CREATEDBY INT = -101)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert client statuses that do not exist into the clientstatus table
	INSERT INTO dbClientStatus (csCode, csTimeEntry, csFileCreation, csAccCode, Created, CreatedBy)
	(
		SELECT DISTINCT
			 cdcode as csCode
			, 1 as csTimeEntry
			, 1 as csFileCreation
			, NULL as csAccCode
			, getutcdate() as Created
			, @CREATEDBY as CreatedBy
		FROM
			dbcodelookup c
		WHERE
			cdtype = 'CLIENTSTATUS'
			and cdcode not in (select csCode from dbclientstatus)
	)
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientStatusPopulateTable] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClientStatusPopulateTable] TO [OMSAdminRole]
    AS [dbo];


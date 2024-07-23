

CREATE PROCEDURE [dbo].[sprFileStatusPopulateTable]
	(@CREATEDBY UCREATEDBY = -101)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into dbFileStatus (fsCode, fsTimeEntry, fsDocModification, fsAppCreation, fsAssocCreation, fsTaskflowEdit, fsAccCode, fsAlertLevel, Created, CreatedBy)

		select distinct
			 cdcode as fsCode
			, 1 as fsTimeEntry
			, 1 as fsDocModification
			, 1 as fsAppCreation
			, 1 as fsAssocCreation
			, 1 as fsTaskflowEdit
			, NULL as fsAccCode
			, case	
				when cdcode = 'DEAD' then 0
				when cdcode like 'LIVE%' then -1
			  else 1 end as fsAlertLevel
			, getutcdate() as Created
			, @CREATEDBY as CreatedBy
		from 
			dbcodelookup c
		where 
			cdtype = 'FILESTATUS'
			and cdcode not in (select fsCode from dbfilestatus)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFileStatusPopulateTable] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFileStatusPopulateTable] TO [OMSAdminRole]
    AS [dbo];


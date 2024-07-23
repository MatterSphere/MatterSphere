

CREATE PROCEDURE [dbo].[fwbsDeleteContact] 
	@contid bigint, 
	@conttypecode nvarchar(15), 
	@newcontid bigint = NULL

AS
SET NOCOUNT ON

IF (SELECT Count(contid) from dbo.dbContact WHERE contid = @contid AND conttypecode = @conttypecode) = 0 
BEGIN
	RAISERROR('Problem deleting Contact...Security not passed or Contact does not exist',16,1)
	RETURN
END

IF (SELECT COUNT(*) FROM dbo.dbClient WHERE clDefaultContact = @contid AND @newcontid IS NULL) > 0
BEGIN
	RAISERROR('Problem Deleting Contact record. The Contact is the default contact to one or more Clients', 16 , 1)
	RETURN
END

DECLARE @deleted_guid UNIQUEIDENTIFIER = (SELECT rowguid FROM dbo.dbContact WHERE contID = @contid)
INSERT dbo.dbContactMergeDelete ( OldContID, ContTypeCode, NewContID, UsrID, DeletedGuid, EntryDate)
SELECT @contid, @conttypecode, @newcontid, usrID, @deleted_guid, GETDATE() FROM dbo.dbUser WHERE usrADID = SUSER_SNAME()

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsDeleteContact] TO [OMSAdminRole]
    AS [dbo];


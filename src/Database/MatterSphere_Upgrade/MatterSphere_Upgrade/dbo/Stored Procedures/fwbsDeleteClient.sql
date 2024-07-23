

CREATE  PROCEDURE [dbo].[fwbsDeleteClient] 
	@clno nvarchar(20), 
	@clid bigint, 
	@includecontact bit = 0

AS
SET NOCOUNT ON

DECLARE @contid bigint

IF (SELECT Count(*) FROM dbo.dbClient WHERE clno = @clno and clid = @clid) = 0 
BEGIN
	RAISERROR('Problem deleting Client...Security not passed or Client does not exist',16,1)
	RETURN
END 

IF (SELECT Count(fileid) FROM dbo.dbfile F JOIN dbo.dbclient C ON C.clid = F.clid WHERE C.clno = @clno) >0 
BEGIN
	RAISERROR('Problem deleting Client...Files exist for the Client',16,1)
	RETURN
END 

IF (@includecontact = 1)
	SET @contid = ( SELECT clDefaultContact FROM dbo.dbClient WHERE clNo = @clno )

DECLARE @deleted_guid UNIQUEIDENTIFIER = (SELECT rowguid FROM dbo.dbClient WHERE clID = @clid)
INSERT dbo.dbClientDelete ( ClID, DefaultContactID, UsrID, DeletedGuid, EntryDate)
SELECT @clid , @contid, usrID, @deleted_guid, GETDATE() FROM dbo.dbUser WHERE usrADID = SUSER_SNAME()

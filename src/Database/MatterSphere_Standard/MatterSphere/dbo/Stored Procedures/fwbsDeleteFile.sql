

CREATE PROCEDURE [dbo].[fwbsDeleteFile] (@clid bigint, @fileid bigint, @newfileid bigint = null)

AS
SET NOCOUNT ON
--@clid is the clid for the file to be deleted
--@fileid is the fileid for the file to be deleted
--@newfileid is the fileid to move data to if select (ie remap the file information to another file)


IF (@clid = -1 AND @fileid = -1 AND @newfileid = -1)
BEGIN
	SELECT NULL as Value, 'Delete File in Design Mode' as Message
	RETURN
END

 
IF (SELECT Count(fileid) FROM dbfile WHERE fileid = @fileid and clid = @clid) = 0
BEGIN
	RAISERROR('Problem deleting File...Security not passed or File does not exist',16,1)
	RETURN
END 
ELSE
BEGIN
	IF ( SELECT COUNT(fileID) FROM dbo.dbFile WHERE fileID = @fileid) = 0
	BEGIN
		PRINT 'Cannot relink. Cannot find file matching @newfileID parameter'
		RETURN
	END
	ELSE
	BEGIN
		DECLARE @deleted_guid UNIQUEIDENTIFIER = (SELECT rowguid FROM dbFile WHERE fileID = @fileid)
		INSERT dbo.dbMatterMergeDelete ( OldFileID, NewFileID, ClID, UsrID, DeletedGuid, EntryDate )
		SELECT @fileid, @newfileid, @clid, usrID, @deleted_guid, GETDATE() FROM dbo.dbUser WHERE usrADID = SUSER_SNAME()
	END
END


SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsDeleteFile] TO [OMSAdminRole]
    AS [dbo];


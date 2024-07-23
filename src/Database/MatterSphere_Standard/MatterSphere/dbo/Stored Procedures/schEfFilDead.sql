

CREATE PROCEDURE [dbo].[schEfFilDead]
        @STARTDATE DATETIME = null,
        @ENDDATE DATETIME = null
AS
SET NOCOUNT ON

SELECT  dbClient.clNo, dbClient.clName, dbFile.fileID, dbFile.fileNo, dbFile.fileDesc, dbFile.FileStatus, evType, evDesc, evwhen 
FROM dbFile
JOIN
	( 
		SELECT Row_number () OVER (Partition by FileID Order By evWhen ) as RowNumber , FileID, evType, evDesc, evWhen FROM dbo.dbFileEvents WHERE  evType = 'CHGFILESTATUS' 
		AND evWhen >=coalesce(@STARTDATE,'20090101')
		AND evWhen <=coalesce(@ENDDATE, GetUTCDate())
		AND evDesc = 'Live (LIVE) -> Dead (DEAD)'
	) 
		Y ON Y.FileID = dbo.dbFile.FileID
	
LEFT JOIN (Select fileID from dbo.dbFileEvents WHERE evType = 'EFQUESTSENT' group by fileID) X on X.fileID = dbFile.fileID
INNER JOIN dbClient on dbClient.clID = dbFile.clID
AND dbFile.FileStatus = 'Dead'
AND X.fileID is NULL
AND Y.RowNumber = 1


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schEfFilDead] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schEfFilDead] TO [OMSAdminRole]
    AS [dbo];


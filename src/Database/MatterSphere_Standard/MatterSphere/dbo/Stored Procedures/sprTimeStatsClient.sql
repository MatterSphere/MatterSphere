

CREATE PROCEDURE [dbo].[sprTimeStatsClient] (@CLID bigint,  @UI uUICultureInfo = '{default}')  
AS

SELECT     F.FileNo, T.fileID, timeStatus, timeBilled
, timeBand, SUM(timeMins) AS TotMins, COUNT(ID) AS NumTrans, SUM(timeCost) AS TotCost, SUM(timeCharge) AS TotCharge, 
                      SUM(timeUnits) AS TotUnits
FROM         dbo.dbTimeLedger T LEFT OUTER JOIN dbo.dbfile F on F.fileid = T.fileid
WHERE T.clid = @CLID 
GROUP BY F.FileNo, T.fileID, timeStatus, timeBand , timeBilled

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeStatsClient] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeStatsClient] TO [OMSAdminRole]
    AS [dbo];


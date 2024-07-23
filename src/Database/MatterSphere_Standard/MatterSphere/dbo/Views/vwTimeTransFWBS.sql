

CREATE VIEW[dbo].[vwTimeTransFWBS]
AS
SELECT     dbo.dbClient.clNo, dbo.dbFile.fileNo AS MatNo, CASE WHEN datepart(hh, timerecorded) = 23 AND datepart(mi, timerecorded) = 0 THEN dateadd(hh, 1, timerecorded) 
                      ELSE dbo.getstartdate(timerecorded) END AS DateCreated, FeeEarner.usrInits AS Fe, dbo.dbActivities.actAccCode AS ActCode, dbo.dbTimeLedger.timeDesc AS Narra, 
                      dbo.dbTimeLedger.timeMins AS Minutes, dbo.dbTimeLedger.timeCost AS CostValue, dbo.dbTimeLedger.timeCharge AS ChargeValue, 
                      dbo.dbTimeLedger.timeUnits AS Quantity, dbo.dbTimeLedger.timeTransferred AS Imported, dbo.dbTimeLedger.ID AS TimeID, 
                      dbo.dbTimeLedger.timeTransferredDate AS ImportedDate, isnumeric(dbo.dbFile.fileNo) AS numberfileno
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbTimeLedger ON dbo.dbFile.fileID = dbo.dbTimeLedger.fileID INNER JOIN
                      dbo.dbUser AS FeeEarner ON dbo.dbTimeLedger.feeusrID = FeeEarner.usrID INNER JOIN
                      dbo.dbActivities ON dbo.dbTimeLedger.timeActivityCode = dbo.dbActivities.actCode
WHERE     (LEN(FeeEarner.usrInits) <= 4) AND (isnumeric(dbo.dbTimeLedger.timeActivityCode) = 1) AND (dbo.dbTimeLedger.timeMins <= 32767) AND 
                      (dbo.dbTimeLedger.timeMins >= - 32768) AND (dbo.dbTimeLedger.timeTransferred = 0) AND (isnumeric(dbo.dbFile.fileNo) = 1)

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwTimeTransFWBS] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwTimeTransFWBS] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwTimeTransFWBS] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwTimeTransFWBS] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwTimeTransFWBS] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwTimeTransFWBS] TO [OMSApplicationRole]
    AS [dbo];




CREATE VIEW[dbo].[vwRptTimeSheet]
AS
SELECT     dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbTimeLedger.timeActivityCode, dbo.dbTimeLedger.timeLegalAidCat, dbo.dbTimeLedger.timeLegalAidGrade, 
                      dbo.dbTimeLedger.timeDesc, dbo.dbTimeLedger.timeRecorded, dbo.dbTimeLedger.timeUnits, dbo.dbTimeLedger.timeMins, 
                      dbo.dbTimeLedger.timeActualMins, dbo.dbTimeLedger.timeFormat, dbo.dbTimeLedger.timeCost, dbo.dbTimeLedger.timeActualCost, 
                      dbo.dbTimeLedger.timeCharge, dbo.dbTimeLedger.timeBand, dbo.dbTimeLedger.timeBilled, dbo.dbTimeLedger.timeBillNo, 
                      dbo.dbTimeLedger.timeStatus, dbo.dbTimeLedger.timeTransferred, dbo.dbUser.usrInits, dbo.dbUser.usrFullName, dbo.dbTimeLedger.feeusrID, 
                      dbo.dbTimeLedger.Created
FROM         dbo.dbUser INNER JOIN
                      dbo.dbFeeEarner ON dbo.dbUser.usrID = dbo.dbFeeEarner.feeusrID INNER JOIN
                      dbo.dbTimeLedger INNER JOIN
                      dbo.dbFile ON dbo.dbTimeLedger.fileID = dbo.dbFile.fileID INNER JOIN
                      dbo.dbClient ON dbo.dbTimeLedger.clID = dbo.dbClient.clID ON dbo.dbFeeEarner.feeusrID = dbo.dbTimeLedger.feeusrID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwRptTimeSheet] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwRptTimeSheet] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwRptTimeSheet] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwRptTimeSheet] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwRptTimeSheet] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwRptTimeSheet] TO [OMSApplicationRole]
    AS [dbo];


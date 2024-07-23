

CREATE VIEW[dbo].[vwDBTimeLedger]
AS
SELECT     dbo.dbTimeLedger.ID AS ID, dbo.dbTimeLedger.fileID AS MatterID, dbo.dbTimeLedger.timeActivityCode AS TimeActivityCode, 
                      dbo.dbTimeLedger.timeDesc AS TimeDesc, dbo.dbTimeLedger.timeRecorded AS TimeRecorded, FeeEarner.usrInits AS TimeFeeEarner, 
                      [User].usrInits AS TimeCreatedBy, dbo.dbTimeLedger.timeUnits AS TimeUnits, dbo.dbTimeLedger.timeMins AS TimeMins, 
                      dbo.dbTimeLedger.timeFormat AS TimeFormat, dbo.dbTimeLedger.timeCost AS TimeCost, dbo.dbTimeLedger.timeCharge AS TimeCharge, 
                      dbo.dbTimeLedger.timeBand AS TimeBand, dbo.dbTimeLedger.timeBilled AS TimeBIlled, dbo.dbTimeLedger.timeBillNo AS TimeBillNo, 
                      dbo.dbTimeLedger.timeTransferred AS TimeTransferred, NULL AS TimeCurrency
FROM         dbo.dbTimeLedger INNER JOIN
                      dbo.dbUser FeeEarner ON dbo.dbTimeLedger.feeusrID = FeeEarner.usrID INNER JOIN
                      dbo.dbUser [User] ON dbo.dbTimeLedger.Createdby = [User].usrID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBTimeLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBTimeLedger] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBTimeLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBTimeLedger] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBTimeLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBTimeLedger] TO [OMSApplicationRole]
    AS [dbo];


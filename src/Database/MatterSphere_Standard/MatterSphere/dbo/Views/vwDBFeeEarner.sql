

CREATE VIEW[dbo].[vwDBFeeEarner]
AS
SELECT     dbo.dbUser.usrInits AS FeeInits, 0 AS FeeNumber, dbo.dbUser.usrFullName AS FeeFullName, dbo.dbUser.usrExtension AS FeeExtension, 
                      dbo.dbUser.usrDDI AS FeeDirectNumber, dbo.dbUser.usrDDIFax AS FeeDirectFax, dbo.dbFeeEarner.feeSignOff, dbo.dbFeeEarner.feeDepartment, 
                      dbUser_1.usrInits AS FeeResponsible, dbo.dbFeeEarner.feeResponsible AS FeeResp, dbo.dbUser.usrEmail AS FeeEmail, dbo.dbFeeEarner.feeCost, 
                      dbo.dbFeeEarner.feeRateBand1, dbo.dbFeeEarner.feeRateBand2, dbo.dbFeeEarner.feeRateBand3, dbo.dbFeeEarner.feeRateBand4, 
                      dbo.dbFeeEarner.feeRateBand5, dbo.dbFeeEarner.feeLAGrade AS FeeLegalAidGrade, dbo.dbFeeEarner.feeLARef AS FeeLegalAidRef, 
                      dbo.dbFeeEarner.feeTargetHoursWeek AS FeeTargetHoursWK, dbo.dbFeeEarner.feeTargetHoursDay, dbo.dbFeeEarner.feeCVFile, '' AS FeeTeamName, 
                      dbo.dbFeeEarner.feeAddRef, dbo.dbFeeEarner.feeAddSignOff, dbo.dbFeeEarner.feeActive, dbo.dbFeeEarner.feeCDSStartNum, 
                      dbo.dbFeeEarner.feeAssistant, '' AS FeeLanguage
FROM         dbo.dbFeeEarner INNER JOIN
                      dbo.dbUser ON dbo.dbFeeEarner.feeusrID = dbo.dbUser.usrID INNER JOIN
                      dbo.dbUser dbUser_1 ON dbo.dbFeeEarner.feeResponsibleTo = dbUser_1.usrID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBFeeEarner] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBFeeEarner] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBFeeEarner] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBFeeEarner] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBFeeEarner] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBFeeEarner] TO [OMSApplicationRole]
    AS [dbo];


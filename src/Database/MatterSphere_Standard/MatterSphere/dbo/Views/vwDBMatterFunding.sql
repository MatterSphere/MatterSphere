

CREATE VIEW[dbo].[vwDBMatterFunding]
AS
SELECT     dbo.dbFile.fileID AS ID, dbo.dbFile.fileID AS MatterID, dbo.dbClient.clNo, dbo.dbFile.fileNo AS MatNo, dbo.dbFile.fileFundCode AS FTCode, 
                      dbo.dbFile.fileFundRef AS FTRef, dbo.dbFile.fileCreditLimit AS FTCreditLimit, dbo.dbFile.fileBanding AS FTBanding, 
                      dbo.dbFile.fileAgreementDate AS FTAgreementDate, dbo.dbFile.fileQuoteSent AS FTQuoteSent, dbo.dbFile.fileLACategory AS FTLACategory, NULL 
                      AS FTWIP, NULL AS FTBTD, dbo.dbFile.fileFranCode AS FTFranCode, dbo.dbFile.fileEstimate AS FTEstimate, 
                      dbo.dbFile.fileComplexity AS FTComplexity, dbo.dbFile.fileFundingExtension AS FTFundingExtension, 
                      dbo.dbFile.fileFundingExtReason AS FTFundintExtReason, dbo.dbFile.fileOriginalLimit AS FTOriginalLimit, dbo.dbFile.fileScope AS FTScope, 
                      dbo.dbFile.fileLastEstimate AS FTLastEstimate, dbo.dbFile.fileCost AS FTCost, dbo.dbFile.fileRatePerUnit AS FTRatePerUnit, 
                      dbo.dbFile.filecurISOCode AS FTCurrency, dbo.dbFile.fileWarningPerc AS FTWarningPerc
FROM         dbo.dbFile INNER JOIN
                      dbo.dbClient ON dbo.dbFile.clID = dbo.dbClient.clID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBMatterFunding] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterFunding] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterFunding] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterFunding] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBMatterFunding] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBMatterFunding] TO [OMSApplicationRole]
    AS [dbo];


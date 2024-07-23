

CREATE VIEW[dbo].[vwDBMatters]
AS
SELECT     dbo.dbFile.fileID AS MatterID, 
					-- dbo.dbFile.OLDMatterID AS JETMATTERID, REMOVED AS PART OF THE DATABASE CLEAN UP OF OLD COLUMNS
					  dbo.dbClient.clNo AS ClientNumber, dbo.dbFile.clID, 
                      COALESCE (dbo.dbFile.fileExtLinkTxtID, dbo.dbFile.fileAccCode) AS MatAccCode, dbo.dbFile.fileNo AS MatNo, 
                      dbo.dbContact.contName AS MatAddressee, dbo.dbContact.contSalut AS MatSalut, LEFT(dbo.dbFile.fileDesc, 90) AS MatDesc, 
                      LEFT(MatPrinciple.usrInits, 12) AS MatPrinciple, MatResponsible.usrInits AS MatResponsible, dbo.dbFile.fileDepartment AS MatDept, 
                      dbo.dbBranch.brCode AS MatBranch, dbo.dbFile.fileFundCode AS MatFundType, LEFT(dbo.dbFile.fileType, 12) AS MatType, 
                      dbo.dbFile.Created AS MatCreated, MatCreatedBy.usrInits AS MatCreatedBy, dbo.dbFile.Updated AS MatLastUpdated, 
                      dbo.dbFile.Updatedby AS MatLastUpdatedBy, dbo.dbFile.fileClosed AS MatClosed, dbo.dbFile.fileClosedby AS MatClosedBy, '' AS MatConflictDone, 
                      0 AS MatConflictFound, '' AS MatConflictCheck, dbo.dbFile.fileStatus AS MatStatus, '' AS MatClientCareLetter, '' AS MatConfLetter, '' AS MatExpLetter, 
                      dbo.dbFile.fileNeedExport AS MatNeedExport, NULL AS MatMileStoneUpdated, NULL AS MatMileStoneUpdatedBy, '
                       FILE ' AS Expr1, 
                      dbo.dbMSData_OMS2K.MSCode AS MatMSCode, NULL AS MatLinkTable, dbo.dbFileLegal.MatLAContract, dbo.dbFileLegal.MatLAMatType, 
                      dbo.dbFileLegal.MatLAPartI, dbo.dbFileLegal.MatLAPartII, dbo.dbFileLegal.MatEndPoint, dbo.dbFile.fileNotes AS MatNotes, 
                      dbo.dbFile.fileAllowExternal AS MatAllowExternal, '' AS MatExtPassword, '' AS MatExtEmail, '' AS MatExtInfo, NULL AS MatExtReq, 
                      '' AS MatExtReqBy, NULL AS MatExtLast, NULL AS MatLinkID, CONVERT(ntext, dbo.dbFile.fileExternalNotes) AS MatClientNotes, 0 AS MatSMSEnabled, 
                      '' AS MatPassword, dbo.dbFileLegal.MatLAUFN, '' AS MatLanguage, '' AS MatRiskCode, '' AS MatTeam, 0 AS MatOffline, 0 AS MatOfflineID, 
                      0 AS MatAdrNum, 0 AS MatDescExclForClient, 'CLIENT' AS MatClientIs, '' AS MatTheirRef, '' AS MatRef
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbUser MatPrinciple ON dbo.dbFile.filePrincipleID = MatPrinciple.usrID INNER JOIN
                      dbo.dbUser MatResponsible ON dbo.dbFile.fileResponsibleID = MatResponsible.usrID INNER JOIN
                      dbo.dbContact ON dbo.dbClient.clDefaultContact = dbo.dbContact.contID INNER JOIN
                      dbo.dbBranch ON dbo.dbFile.brID = dbo.dbBranch.brID LEFT OUTER JOIN
                      dbo.dbUser MatCreatedBy ON dbo.dbFile.CreatedBy = MatCreatedBy.usrID LEFT OUTER JOIN
                      dbo.dbFileLegal ON dbo.dbFile.fileID = dbo.dbFileLegal.fileID LEFT OUTER JOIN
                      dbo.dbMSData_OMS2K ON dbo.dbFile.fileID = dbo.dbMSData_OMS2K.fileID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatters] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatters] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBMatters] TO [OMSApplicationRole]
    AS [dbo];


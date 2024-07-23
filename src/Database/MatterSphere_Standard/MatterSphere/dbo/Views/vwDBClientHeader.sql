

CREATE VIEW[dbo].[vwDBClientHeader]
AS
SELECT     
 dbo.dbClient.clID AS sqlCLID, 
 dbo.dbClient.clID, 
 --dbo.dbContact.OLDClid AS JETCLID, REMOVED AS PART OF DATABASE CLEAN UP OF OLD COLUMNS
 dbo.dbClient.clNo AS CLNO, 
 dbo.dbClient.cltypeCode, 
 LEFT(dbo.dbClient.clName, 40) AS CLName, 
 NULL AS CLCOName, 
 dbo.dbClient.clAccCode, 
 dbo.dbClient.clSearch1, 
 dbo.dbClient.clSearch2, 
 dbo.dbClient.clSearch3, 
 dbo.dbClient.clSearch4, 
 dbo.dbClient.clSearch5, 
 CONT.contAddressee AS clAddressee, 
 LEFT(dbo.dbAddress.addLine1, 40) AS clAdd1, 
 LEFT(dbo.dbAddress.addLine2, 40) AS clAdd2, 
 LEFT(dbo.dbAddress.addLine3, 40) AS clAdd3, 
 LEFT(dbo.dbAddress.addLine4, 40) AS clAdd4, 
 LEFT(dbo.dbAddress.addLine5, 40) as cladd5,
 case 
  when ctrycode <> 'ENGLAND' then COALESCE(CL1.cdDesc, '~' + NULLIF(dbo.dbCountry.ctryCode, '') + '~') else '' end AS clCountry, 
 LEFT(dbo.dbAddress.addPostcode, 10) AS clPostcode, 
 dbo.dbAddress.addDXCode AS clDXCode, 
 (SELECT TOP 1 CONTNUMBER FROM DBCONTACTNUMBERS CN1 WHERE CN1.CONTID = CONT.CONTID AND CN1.CONTCODE = 'TELEPHONE' AND CN1.CONTEXTRACODE = 'WORK' AND CN1.CONTACTIVE = 1 ORDER BY CN1.CONTORDER ) AS clTel1,  --Office
 (SELECT TOP 1 CONTNUMBER FROM DBCONTACTNUMBERS CN2 WHERE CN2.CONTID = CONT.CONTID AND CN2.CONTCODE = 'TELEPHONE' AND CN2.CONTEXTRACODE = 'HOME' AND CN2.CONTACTIVE = 1 ORDER BY CN2.CONTORDER ) AS clTel2,  --Home
 (SELECT TOP 1 CONTNUMBER FROM DBCONTACTNUMBERS CN3 WHERE CN3.CONTID = CONT.CONTID AND CN3.CONTCODE = 'FAX' AND CN3.CONTACTIVE = 1 ORDER BY CN3.CONTORDER ) AS clFax, 
 (SELECT TOP 1 CONTNUMBER FROM DBCONTACTNUMBERS CN4 WHERE CN4.CONTID = CONT.CONTID AND CN4.CONTCODE = 'MOBILE' AND CN4.CONTACTIVE = 1 ORDER BY CN4.CONTORDER ) AS clMob, 
 (SELECT TOP 1 CONTEMAIL FROM DBCONTACTEMAILS CM1 WHERE CM1.CONTID = CONT.CONTID AND CM1.CONTCODE = 'MOBILE' AND CM1.CONTACTIVE = 1 ORDER BY CM1.CONTORDER ) AS clEmail, 
 CONT.contSalut AS clSalut, 
 dbo.dbClient.clMarket, 
 dbo.dbClient.clSource, 
 dbo.dbBranch.brCode AS clBranch, 
 FirmContactLink.usrInits AS clFirmContact, 
 dbo.dbContactIndividual.contDOB AS clDOB, 
 dbo.dbContactIndividual.contSex AS clSex, 
 dbo.dbContactIndividual.contReligion AS clReligion, 
 dbo.dbContactIndividual.contNationality AS clNationality, 
 dbo.dbContactIndividual.contNINumber AS clNINum, 
 dbo.dbClient.Created AS clCreated, 
 CreatedLink.usrInits AS clCreatedBy, 
 dbo.dbClient.Updated AS clUpdated, 
 UpdatedLink.usrInits AS clUpdatedBy, 
 dbo.dbClient.clNotes, 
 CONT.contXMASCard AS 
 clXmasCard, 
 dbo.dbClient.clNeedExport, 
 dbo.dbClient.clStop, 
 dbo.dbClient.clStopReason, 
 dbo.dbClient.clActiveComplaints, 
 dbo.dbClient.clActiveUndertakings, 
 dbo.dbClient.clRegisterCount, 
 dbo.dbContactIndividual.contIdentity AS clIdent, 
 dbo.dbContactIndividual.contIdentity2 AS clIdent2, 
 dbo.dbContactIndividual.contDOD AS clDOD, 
 dbo.dbContactIndividual.contPOB AS clPOB, 
 CONT.contID
FROM
 dbo.dbAddress 
INNER JOIN
 dbo.dbContact CONT ON dbo.dbAddress.addID = CONT.contDefaultAddress 
RIGHT OUTER JOIN
 dbo.dbClient ON CONT.contID = dbo.dbClient.clDefaultContact 
LEFT OUTER JOIN
 dbo.dbUser CreatedLink ON dbo.dbClient.CreatedBy = CreatedLink.usrID 
LEFT OUTER JOIN
 dbo.dbBranch ON dbo.dbClient.brID = dbo.dbBranch.brID 
LEFT OUTER JOIN
 dbo.dbUser FirmContactLink ON dbo.dbClient.feeusrID = FirmContactLink.usrID 
LEFT OUTER JOIN
 dbo.dbUser UpdatedLink ON dbo.dbClient.UpdatedBy = UpdatedLink.usrID 
LEFT OUTER JOIN
 dbo.dbContactIndividual ON dbo.dbClient.clDefaultContact = dbo.dbContactIndividual.contID 
LEFT OUTER JOIN
 dbo.dbCountry ON dbo.dbAddress.addCountry = dbo.dbCountry.ctryID
LEFT JOIN 
dbo.GetCodeLookupDescription('COUNTRIES', '{DEFAULT}') CL1 ON CL1.cdCode = dbo.dbCountry.ctryCode

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBClientHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBClientHeader] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBClientHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBClientHeader] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBClientHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBClientHeader] TO [OMSApplicationRole]
    AS [dbo];


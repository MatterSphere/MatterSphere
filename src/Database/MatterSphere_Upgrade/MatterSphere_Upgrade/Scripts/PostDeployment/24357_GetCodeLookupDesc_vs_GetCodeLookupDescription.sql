SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.GetCodeLookupDescription') AND type in (N'IF', N'TF' , N'FN'))
	DROP FUNCTION [dbo].[GetCodeLookupDescription]
GO

CREATE FUNCTION [dbo].[GetCodeLookupDescription] ( @type uCodeLookup , @ui uUICultureInfo )
RETURNS Table
RETURN
(
	WITH c AS(
	SELECT cdCode 
		, MAX(CASE cdUICultureInfo WHEN @ui THEN [cdDesc] END) AS base
		, MAX(CASE WHEN cdUICultureInfo = '{default}' THEN [cdDesc] END) AS [default]
	FROM [dbo].[dbCodeLookup] 
	WHERE cdType = @type AND cdUICultureInfo IN (@ui, '{default}')
	GROUP BY cdCode
	)
	SELECT @type as cdType
		, [cdCode] 
		, CASE WHEN base IS NOT NULL THEN base WHEN base IS NULL AND [default] IS NOT NULL THEN [default] ELSE '~' +[cdCode] + '~' END as cdDesc
	FROM c
)

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.MaskToPermissions') AND type in (N'IF', N'TF' , N'FN'))
	DROP FUNCTION [config].[MaskToPermissions]
GO

CREATE FUNCTION [config].[MaskToPermissions] ( @allowMask binary(32) , @denyMask binary(32) )
RETURNS table

RETURN
(

	SELECT
		OPC.[SecurableType] ,
		COALESCE(GCL.cdDesc, '~' + NULLIF(OPC.Permission, '') + '~') as Permission , 
		Permission as PermissionCode ,
		CASE WHEN Substring ( @allowMask , OPC.[Byte] , 1 ) & OPC.[BitValue] = OPC.[BitValue] THEN 1 ELSE 0 END as [Allow] ,
		CASE WHEN Substring ( @denyMask , OPC.[Byte] , 1 ) & OPC.[BitValue] = OPC.[BitValue] THEN 1 ELSE 0 END as [Deny] ,
		OPC.[Byte],
		OPC.[BitValue],
		OPC.[MajorType],
		OPC.[NodeLevel]
	FROM
		[config].[ObjectPolicyConfig] OPC
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'PERMISSION' ,'{default}' ) GCL ON GCL.[cdCode] = OPC.[Permission]
	WHERE 
		SecurableType IS NOT NULL 
	AND
		[Permission] IS NOT NULL
	AND
		( SELECT [config].[GetSecurityLevel]() & OPC.SecurityLevel ) <> 0
)

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.SystemMaskToPermissions') AND type in (N'IF', N'TF' , N'FN'))
	DROP FUNCTION [config].[SystemMaskToPermissions]
GO

CREATE FUNCTION [config].[SystemMaskToPermissions] ( @allowMask binary(32) , @denyMask binary(32) )
RETURNS table

RETURN
(
	SELECT
		OPC.[SecurableType] ,
		COALESCE(GCL.cdDesc, '~' + NULLIF(OPC.Permission, '') + '~') as Permission ,
		Permission as PermissionCode ,
--		GCL.[cdDesc] ,
		CASE WHEN Substring ( @allowMask , OPC.[Byte] , 1 ) & OPC.[BitValue] = OPC.[BitValue] THEN 1 ELSE 0 END as [Allow] ,
		CASE WHEN Substring ( @denyMask , OPC.[Byte] , 1 ) & OPC.[BitValue] = OPC.[BitValue] THEN 1 ELSE 0 END as [Deny] ,
		OPC.[Byte],
		OPC.[BitValue],
		OPC.[MajorType],
		OPC.[NodeLevel]
	FROM
		[config].[SystemPolicyConfig] OPC
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'PERMISSION', '{default}') GCL ON GCL.[cdCode] = OPC.[Permission]
	WHERE 
		SecurableType IS NOT NULL 
	AND
		[Permission] IS NOT NULL
)
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.GetFolderCodesForClientDocumentTree') AND type in (N'IF', N'TF' , N'FN'))
	DROP FUNCTION [dbo].[GetFolderCodesForClientDocumentTree]
GO

CREATE FUNCTION [dbo].[GetFolderCodesForClientDocumentTree] (@clientID BIGINT, @UI NVARCHAR(15))
RETURNS TABLE
AS
RETURN
(
SELECT
	CAST(fc.FolderCode AS NVARCHAR(15)) AS FolderCode
	, CAST(fc.FolderGuid AS UNIQUEIDENTIFIER) AS FolderGUID
	, CAST(COALESCE(cl1.cdDesc, '~' + NULLIF(fc.FolderCode, '') + '~') AS NVARCHAR(100)) AS FolderDescription 
FROM dbo.dbFileFolder fc WITH(NOLOCK) 
	LEFT JOIN dbo.GetCodeLookupDescription('DFLDR_MATTER', @UI) cl1 ON cl1.cdCode = fc.FolderCode
WHERE EXISTS(SELECT 1 FROM dbo.dbfile WHERE clID = @clientID AND fileID = fc.fileID)
)
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.GetFolderCodesForContactDocumentTree') AND type in (N'IF', N'TF' , N'FN'))
	DROP FUNCTION [dbo].[GetFolderCodesForContactDocumentTree]
GO

CREATE FUNCTION [dbo].[GetFolderCodesForContactDocumentTree] (@contID BIGINT, @UI NVARCHAR(15))
RETURNS TABLE
AS
RETURN
(
SELECT
	CAST(fc.FolderCode AS NVARCHAR(15)) AS FolderCode
	, CAST(fc.FolderGuid AS UNIQUEIDENTIFIER) AS FolderGUID
	, CAST(COALESCE(cl1.cdDesc, '~' + NULLIF(fc.FolderCode, '') + '~') AS NVARCHAR(100)) AS FolderDescription 
FROM dbo.dbFileFolder fc WITH(NOLOCK) 
	LEFT JOIN dbo.GetCodeLookupDescription('DFLDR_MATTER', @UI) cl1 ON cl1.cdCode = fc.FolderCode
WHERE EXISTS(SELECT 1 FROM dbo.dbfile f INNER JOIN dbo.dbAssociates a ON f.fileID = a.fileID WHERE a.contID = @contID AND f.fileID = fc.fileID)
)
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.vwDBActivities') AND type in (N'V'))
	DROP VIEW [dbo].[vwDBActivities]
GO

CREATE VIEW [dbo].[vwDBActivities]
AS
SELECT     actCode AS ActivityCode, actAccCode AS ActivityAccCode, COALESCE(CL1.cdDesc, '~' + NULLIF(actCode, '') + '~') AS ActivityDesc, 
                      actChargeable AS ActivityChargeable, actFixedRateLegal AS ActivityFixedRate, actTemplateMatch AS ActivityTemplateMatch, 
                      actActive AS ActivityActive, actFixedValue AS ActivityFixedValue
FROM         dbo.dbActivities
LEFT JOIN 
	dbo.GetCodeLookupDescription('TIMEACTCODE', 'en-gb') CL1 ON CL1.cdCode = actCode

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSApplicationRole]
    AS [dbo];

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.vwDBClientHeader') AND type in (N'V'))
	DROP VIEW [dbo].[vwDBClientHeader]
GO

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

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.vwDBMatterType') AND type in (N'V'))
	DROP VIEW [dbo].[vwDBMatterType]
GO



CREATE VIEW[dbo].[vwDBMatterType]
AS
SELECT     typeCode AS MatterType, LEFT(COALESCE(CL1.cdDesc, '~' + NULLIF(typeCode, '') + '~'), 50) AS MatTypeDesc, fileDeptCode AS MatDepartment, 
                      fileAccCode AS MatAccNum, COALESCE (fileDefBankCode, N'1') AS MatDefBankCode, COALESCE (fileDefOffBankCode, N'1') 
                      AS MatDefOffBankCode
FROM         dbo.dbFileType
LEFT JOIN 
dbo.GetCodeLookupDescription('FILETYPE', 'en-gb') CL1 ON CL1.cdCode = typeCode
GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSApplicationRole]
    AS [dbo];


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.vwFolders') AND type in (N'V'))
	DROP VIEW [dbo].[vwFolders]
GO

CREATE VIEW[dbo].[vwFolders]
AS
SELECT     flID AS ID, dbo.GetCodeLookupDesc(N'FOLDERS', flCode, '{DEFAULT}') AS FolderName, COALESCE(CL1.cdDesc, '~' + NULLIF(flType, '') + '~')
                      AS Type, N'' AS Item1, N'' AS Item2, N'' AS Item3
FROM         dbo.dbFolderStructure
LEFT JOIN 
dbo.GetCodeLookupDescription('FOLDERS', '{DEFAULT}') CL1 ON CL1.cdCode = flCode
WHERE     (ftIntType = N'FORMS')

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwFolders] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwFolders] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwFolders] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwFolders] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwFolders] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwFolders] TO [OMSApplicationRole]
    AS [dbo];


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportClients]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportFeeEarners]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportFeeEarners]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportMatterTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportMatterTypes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportMatters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportMatters]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportTime]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportTimeActivity]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportTimeActivity]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportUsers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILUpdateClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILUpdateClients]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILUpdateMatters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILUpdateMatters]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE [dbo].[sprMILExportClients]
AS

SET NOCOUNT ON

SELECT 
CL.clID,
CL.clNo,
left((case when coalesce(CL.clName,'') = '' then 'NoName' else CL.clName end),40) as COMPANY_NAME, --name is required
(select top 1 contEmail from dbContactEmails where contid = CL.clDefaultContact order by contorder) as InternetAddr,
left((select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'TELEPHONE' order by contorder),20) as PhoneNumber,
left((select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'FAX' order by contorder),20) as FaxNumber,
--(case when C.contTypecode = 'ORGANISATION' then 'O' else 'P' end) as NameType,
Case Convert(xml,CT.typexml).value('(/Config/Settings/@GeneralType)[1]', 'nvarchar(50)') when 'Company' then 'O' else 'P' end as NAME_TYPE,
(case when coalesce(CI.contTitle,'') = '' then ' ' else CI.contTitle end) as TITLE,
--CI.contInitials,
left((case when Coalesce(CI.contChristianNames,'') = '' then ' ' else CI.contChristianNames end),20)  as FIRST_NAME,
(case when Coalesce(CI.contSurname,'') = '' then ' ' else CI.contSurname end) as SURNAME,
CI.contDOB,
case 
	when CL.clsourcecontact is not null then 'CONTA' 
	when CL.clsourceuser is not null then 'USER'
	else left(CL.clSource,5) 
END as clSource, -- maps to ClassCode table HBL_CLNT_CLASS
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline1 ELSE CLIADD.addLine1 END AS ADDRESS_LINE_1,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline2 ELSE CLIADD.addLine2 END AS ADDRESS_LINE_2,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline3 ELSE CLIADD.addLine3 END AS ADDRESS_LINE_3,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline4 ELSE CLIADD.addLine4 END AS TOWN_CITY,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline5 ELSE CLIADD.addLine5 END AS COUNTY,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addpostcode ELSE CLIADD.addpostcode END AS POSTCODE,
case WHEN CL.clDefaultAddress is null THEN C2.ctryid ELSE C1.ctryid END AS ctryid, --these need to get in syn with HBL_country
CUP.cdDesc as COUNTRY,
C.contSalut as SALUTATION,
CL.clNo as COMPANY_NUMBER,
CL.clNo as CLIENT_ID,
CN1.contNumber as PHONE_NO ,
CN2.contNumber as MOBILE_NO ,
CN3.contNumber as FAX_NO ,
CE.contEmail as EMAIL_ADDRESS,
'N' as DELETED_FLAG,
'N' as ARCHIVED_FLAG,
'Y' as INTEREST_FLAG,
'Y' as UK_DOMICILE_FLAG,
'Y' as VAT_RESIDENT_FLAG,
'Y' as AUTO_BILL_FLAG,
'BUS' as ADDRESS_TYPE


FROM         
	dbo.dbClient CL 
left JOIN 
	dbo.dbAddress CLIADD ON CL.clDefaultAddress = CLIADD.addID
left join 
	dbcountry C1 on CLIADD.addCountry = C1.ctryID  
join 
	dbcontact C on C.contid = CL.clDefaultContact
join
	dbCodeLookup CUP on C1.ctryCode = CUP.cdCode
join 
	dbcontacttype CT on C.contTypeCode = CT.typeCode
join 
	dbAddress CONTADD on CONTADD.addid = C.contDefaultAddress
left join 
	dbcountry C2 on CONTADD.addCountry = C2.ctryID
join 
	dbBranch B on B.brID = CL.brid
join 
	dbuser U on U.usrID = CL.feeusrID
join 
	dbuser UC on UC.usrid = CL.Createdby
left join 
	dbContactIndividual CI on CI.contID = CL.clDefaultContact
inner join 
	dbFeeEarner F on f.feeusrid = CL.feeusrid
left join 
	dbDepartment DEPTFEE on DEPTFEE.deptCode = F.feeDepartment
LEFT JOIN
	( SELECT contID , contNumber  FROM dbContactNumbers WHERE contCode = 'TELEPHONE' AND contExtraCode = 'HOME' AND contOrder = 0 AND contActive = 1  ) CN1 ON CN1.contID = C.contID
LEFT JOIN
	( SELECT contID , contNumber  FROM dbContactNumbers WHERE contCode = 'MOBILE' AND contExtraCode = 'HOME' AND contOrder = 0 AND contActive = 1  ) CN2 ON CN2.contID = C.contID
LEFT JOIN
	( SELECT contID , contNumber  FROM dbContactNumbers WHERE contCode = 'FAX' AND contExtraCode = 'HOME' AND contOrder = 0 AND contActive = 1  ) CN3 ON CN3.contID = C.contID
LEFT JOIN
	( SELECT contID , contEmail  FROM dbContactEmails WHERE contCode = 'HOME' AND contOrder = 0 AND contActive = 1  ) CE ON CE.contID = C.contID

WHERE 
	CL.clneedexport = 1 and 
	(CL.clextID is null or CL.clextID = 0) and 
	U.usrextid is not null 



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE [dbo].[sprMILExportFeeEarners]
AS
SET NOCOUNT ON

SELECT 
	F.feeusrID as FeeUsrID ,
	UPPER ( U.usrInits ) as FeeEarnerCode ,
	U.usrFullName as FeeEarnerName ,
	0 as Grade ,
	( SELECT TOP 1 regTimeUnitValue FROM dbRegInfo ) as MinsPerUnit ,
	0 as IsPartner ,
	0 as CostRate ,
	0 as ChargeRate ,
	0 as DailyTargetTime ,
	0 as DailyTargetValue ,
	U.usrAlias as UserCode

FROM
	dbUser U 
JOIN
	dbFeeEarner F ON U.usrID = F.feeUsrID
WHERE 
	F.feeExtID IS NULL
AND
Len ( U.usrInits ) <= 4

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE  PROCEDURE [dbo].[sprMILExportMatterTypes]
AS
SET NOCOUNT ON

DECLARE @vSQL nvarchar(2000) , @vLinkedIndigoServer nvarchar(400)
SET @vLinkedIndigoServer = ( SELECT spData FROM dbSpecificData WHERE spLookup = 'INDIGOLNKSVR' )
SET @vSQL = '
SELECT 
	F.FileAccCode as WorkTypeCode ,
	( SELECT TOP 1 dbo.GetCodeLookupDesc ( ''FILETYPE'' , TypeCode , ''{default}'' ) FROM dbFileType F2 WHERE F2.FileAccCode = F.FileACcCode )as Description
FROM
	( SELECT DISTINCT FileAccCode FROM dbFileType WHERE FileAccCode IS NOT NULL AND IsNumeric(FileAccCode) = 1) F
LEFT JOIN
	' + @vLinkedIndigoServer + '.dbo.WorkTypes WT ON WT.WorkTypeCode = F.fileAccCode
WHERE
	WT.WorkTypeCode IS NULL'


EXEC ( @vSQL )
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE [dbo].[sprMILExportMatters]
AS
SET NOCOUNT ON

SELECT  
F.fileID,
F.fileNo,
C.clextID,
--getdate() as OpenDate, Modified DMS 23/Feb/2005 time entries need to be later than created date
F.Created as OpenDate,
left(F.fileDesc,40) as MatterName,  --cannnot be over 40 characters
F.fileDesc,
F.fileExternalNotes,
F.fileStatus,
F.fileStatus as StatusCode, --'OPEN' as StatusCode, --get in sync 5 characters max
FY.fileacccode as FileType, --'BOS' as FileType,  --get in sync  5 characters max
case 
	when F.filesourcecontact is not null then 'CONTA' 
	when F.filesourceuser is not null then 'USER'
	else F.fileSource 
END as fileSource, -- maps to ClassCode table HBL_CLNT_CLASS, -- get in sync  5 characters max
F.fileEstimate,
F.fileAccCode,
B.brCode,
D.deptAccCode as OU_ID_OWNER, -- CMS Dept Code F.fileDepartment,
FTACCCODE AS fileFundCode, --CMSPROF
'N' as DELETED_FLAG,
'N' as ARCHIVED_FLAG,
'N' as CLOSED_FLAG,
'Y' as WIP_ALLOWED_TIME,
'Y' as WIP_ALLOWED_DISB,
'Y' as WIP_ALLOWED_EXP,
'Y' as MATTER_PREFIX,
--	UP.usrInits as OU_ID_OWNER, it now user D.deptAccCode as above
F.fileNo as MATTER_NUM,
F.fileNo as MATTER_ID,
C.clID as CLIENT_ID,
C.clName as COMPANY_NAME,
F.fileDesc as MATTERDESCRIPTION,
FY.fileAccCode as WORK_TYPE,
F.fileNotes as USER_NOTES,
UR.usrInits as FE_ID_SR,
UP.usrInits as FE_ID_EA

FROM         
dbo.dbClient C JOIN dbo.dbFile F ON C.clid = F.clID
inner join 
	dbfundtype FT on FT.ftcode = F.filefundcode and FT.ftCurISOCode = f.filecurisocode
inner join 
	dbDepartment D on f.filedepartment = D.deptcode
left join dbuser UO on F.CreatedBy = UO.usrID
left join dbuser UM on F.filemanagerid = UM.usrID
left join dbuser UP on F.fileprincipleID = UP.usrID
left join dbuser UR on F.FileResponsibleID = UR.usrID join dbBranch B on B.brid = F.brid
left join dbfiletype FY on FY.typecode = F.filetype
--build clause that only includes files for clients that exist within CMS and live files

WHERE 
	F.fileNeedExport = 1 and 
	F.fileExtLinkID is null and 
	C.clextID is not null







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE [dbo].[sprMILExportTime] 
AS
SET NOCOUNT ON

SELECT
	T.ID as ID ,
	CL.clNo as ClientNo ,
	F.fileNo as MatterID ,
	T.timeRecorded as DueDate ,
	0 as YearNo ,
	0 as PeriodNo , 
	UPPER ( U.usrInits ) as FeID ,
	T.timeActivityCode as ActivityType ,
	T.timeMins as Minutes ,
	0 as Quantity ,	
	T.timeUnits as Units ,
	0 as CostRate ,
	ROUND( T.TimeCost , 2 ) as CostValue ,
	0 as ChargeRate ,
	ROUND ( T.TimeCharge , 2 ) as ChargeValue ,
	T.timeDesc as ItemDescription ,
	T.timeRecorded as TimeStarted ,
	T.timeRecorded as TimeEnded

FROM
	dbTimeLedger T
JOIN
	dbClient CL ON T.clID = CL.clID
JOIN
	dbFile F ON T.fileID = F.fileID
JOIN
	dbUser U ON U.usrID = T.feeUsrid 
AND
	Isnumeric ( T.timeActivityCode ) = 1
AND
	Len ( U.usrInits ) < = 4
AND
	T.timeTransferred = 0
AND
	CL.ClExtID IS NOT NULL
AND	
	F.FileExtLinkID IS NOT NULL
ORDER BY 
	T.Created


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROCEDURE [dbo].[sprMILExportTimeActivity]
AS
SET NOCOUNT ON

DECLARE @vSQL nvarchar(2000) , @vLinkedIndigoServer nvarchar(400)
SET @vLinkedIndigoServer = ( SELECT spData FROM dbSpecificData WHERE spLookup = 'INDIGOLNKSVR' )
SET @vSQL = '
SELECT
	A.actCode as ActivityCode ,
	LEFT ( dbo.GetCodeLookupDesc ( ''TIMEACTCODE'' , A.actCode , ''{default}'' ) , 50 ) as Description ,
	A.actChargeable as Chargeable ,
	0 as UseQuantity
FROM
	dbActivities A
LEFT JOIN
	' + @vLinkedIndigoServer + '.dbo.Activities AT ON AT.ActivityCode = A.actCode
WHERE
	IsNumeric(A.actCode) = 1
AND 
	AT.ActivityCode IS NULL'
EXEC ( @vSQL )

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE [dbo].[sprMILExportUsers]
AS
SELECT 
	U.usrID as usrID ,
	U.usrAlias as userCode ,
	U.usrFullName as UserName ,
	U.usrInits as FeeEarnerCode
FROM
	dbUser U
WHERE
	Len ( U.usrInits ) <= 4
AND
	U.usrExtID IS NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE [dbo].[sprMILUpdateClients]
AS

SET NOCOUNT ON

SELECT TOP 1000
	CL.clID as clID ,
	CL.clNo as ClientNo ,
	CL.clName as ClientName ,
	-- This field is a sort field and may need to be taylored to customer requirements. For now we will use clName
	LEFT ( CL.clName , 50 )as ClientKey ,
	CC.contRegCoName as CompanyName ,
	'' as PartnerCode ,
	UPPER ( U.usrInits ) as FeeEarnerCode ,
	CI.contDOB as DateOfBirth ,
	CI.contNINumber as NINumber ,
	C.contSalut as Salutation ,
	CI.contNOK as NextOfKin ,
	CL.clTypeCode as ClientType ,
	CL.clSource as BusinessSource ,
	CL.clMarket as Mailshot ,
	0 as TermsOfEngagement ,
	'' as AssociatedClient ,
	0 as ConflictSearchDone ,
	'' as ConflictingClients ,
	'' as ConnectedClients ,
	0 as CreditControl ,
	0 as MoneyLaunderingChecked ,
	'' as MoneyLaunderingNotes ,
	left(clName, 50) as Addressee,
	CASE WHEN clDefaultAddress IS NULL THEN AD2.AddLine1 ELSE AD1.AddLine1 END as AddressLine1 ,
	CASE WHEN clDefaultAddress IS NULL THEN ltrim(left(coalesce(AD2.AddLine2, '') + ' ' + coalesce(AD2.AddLine3, ''), 50)) ELSE ltrim(left(coalesce(AD1.AddLine2, '') + ' ' + coalesce(AD1.AddLine3, ''), 50)) END as AddressLine2 ,
	CASE WHEN clDefaultAddress IS NULL THEN AD2.AddLine4 ELSE AD1.AddLine4 END as Town ,
	CASE WHEN clDefaultAddress IS NULL THEN AD2.AddLine5 ELSE AD1.AddLine5 END as County ,
	CASE WHEN clDefaultAddress IS NULL THEN AD2.AddPostCode ELSE AD1.AddPostCode END as PostCode ,
	CN1.contNumber as PhoneNo ,
	CN2.contNumber as MobileNo ,
	CN3.contNumber as FaxNo ,
	CE.contEmail as EmailAddress

FROM
	( SELECT * FROM dbClient CL WHERE clExtID IS NOT NULL AND CL.clNeedExport = 1) CL
LEFT JOIN
	dbUser U ON U.usrID = CL.feeUsrID
LEFT JOIN
	dbContactCompany CC ON CC.contID = CL.clDefaultContact
LEFT JOIN
	dbContactIndividual CI ON CI.contID = CL.clDefaultContact
LEFT JOIN
	dbContact C ON C.contID = CL.clDefaultContact
LEFT JOIN
	dbAddress AD1 ON AD1.AddID = CL.clDefaultAddress
LEFT JOIN
	dbAddress AD2 ON AD2.AddID = C.contDefaultAddress
LEFT JOIN
	( SELECT contID , contNumber  FROM dbContactNumbers WHERE contCode = 'TELEPHONE' AND contExtraCode = 'HOME' AND contOrder = 0 AND contActive = 1  ) CN1 ON CN1.contID = C.contID
LEFT JOIN
	( SELECT contID , contNumber  FROM dbContactNumbers WHERE contCode = 'MOBILE' AND contExtraCode = 'HOME' AND contOrder = 0 AND contActive = 1  ) CN2 ON CN2.contID = C.contID
LEFT JOIN
	( SELECT contID , contNumber  FROM dbContactNumbers WHERE contCode = 'FAX' AND contExtraCode = 'HOME' AND contOrder = 0 AND contActive = 1  ) CN3 ON CN3.contID = C.contID
LEFT JOIN
	( SELECT contID , contEmail  FROM dbContactEmails WHERE contCode = 'HOME' AND contOrder = 0 AND contActive = 1  ) CE ON CE.contID = C.contID
WHERE 
	IsNumeric ( CC.contRegNo ) = 1 OR CC.contRegNo IS NULL
AND 
	Len ( U.usrInits ) <= 4

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROCEDURE [dbo].[sprMILUpdateMatters]
AS
SET NOCOUNT ON

SELECT 
	F.fileID as FileID ,
	CL.clNo as ClientNo ,
	F.fileNo as MatterNo ,
	F.fileDesc as MatterDescription ,
	UPPER ( U2.usrInits ) as PartnerCode ,
	UPPER ( U1.usrInits ) as FeeEarnerCode ,
	F.fileLACategory as LegalAidCategory ,
	CASE fileFundCode
		WHEN 'LEGALAID' THEN F.fileFundRef
	ELSE NULL
	END as CertificateNo ,
	CASE fileFundCode
		WHEN 'LEGALAID' THEN F.fileAgreementDate 
	ELSE NULL
	END as LegalAidDate ,
	FT.typeCode as WorkTypeCode ,
	convert ( varchar (4096) , F.fileNotes  ) as UserNotes

FROM
	dbClient CL
JOIN
	dbFile F ON F.clID = CL.clID
JOIN
	dbUser U1 ON U1.usrID = F.filePrincipleID
JOIN
	dbUser U2 ON U2.usrID = F.fileResponsibleID
JOIN
	dbFileType FT ON FT.typeCode = F.fileType
WHERE
	IsNumeric ( FT.typeCode ) = 1
AND
	IsNumeric ( F.fileType ) = 1
AND
	IsNumeric ( FT.fileAccCode ) = 1
AND
	Len( U1.usrInits ) <= 4
AND
	Len( U2.usrInits ) <= 4
AND
	F.fileNeedExport = 1
AND
	F.fileExtlinkID IS NOT NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



/*
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportClients]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportMatters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportMatters]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILUpdateClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILUpdateClients]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILUpdateMatters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILUpdateMatters]
GO
*/


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprMILExportTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprMILExportTime]
GO

/*

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON 
GO


CREATE           procedure dbo.sprMILExportClients
AS

-----------------------------------------------------------------------------------------------------------------------

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
'BUS' as ADDRESS_TYPE,
'TRUNC(SYSDATE)' as LDATE_CREATED,
'TRUNC(SYSDATE)' as LDATE_ACTIVE,
'TRUNC(SYSDATE)' as LDATE


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




CREATE          procedure dbo.sprMILExportMatters
as

---------------------------------------------------------------------------------------------------------------------

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
'N' as MATTER_PREFIX,
UP.usrInits as OU_ID_OWNER,
--F.fileNo as MATTER_NUM,
F.fileNo as MATTER_ID,
--C.clID as CLIENT_ID,
C.clName as COMPANY_NAME,
F.fileDesc as MATTERDESCRIPTION,
FY.fileAccCode as WORK_TYPE,
F.fileNotes as USER_NOTES,
UR.usrInits as FE_ID_SR,
UP.usrInits as FE_ID_EA,
C.clexttxtID as SUB_NUM,
C.clexttxtID + '.' + F.fileNo as MATTER_COMBINED_ID_NUM

FROM         
dbo.dbClient C JOIN dbo.dbFile F ON C.clID = F.clID
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
	C.clexttxtID is not null


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



CREATE   procedure sprMILUpdateClients
AS

SELECT 
CL.clID,
CL.clNo,
CL.clextid,
left((case when coalesce(CL.clName,'') = '' then 'NoName' else CL.clName end),40) as COMPANY_NAME, --name is required
(select top 1 contEmail from dbContactEmails where contid = CL.clDefaultContact order by contorder) as InternetAddr,
left((select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'TELEPHONE' order by contorder),20) as PhoneNumber,
left((select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'FAX' order by contorder),20) as FaxNumber,
Case Convert(xml,CT.typexml).value('(/Config/Settings/@GeneralType)[1]', 'nvarchar(50)') when 'Company' then 'O' else 'P' end as NameType,
(case when coalesce(CI.contTitle,'') = '' then ' ' else CI.contTitle end) as TITLE,
--CI.contInitials,
left((case when Coalesce(CI.contChristianNames,'') = '' then ' ' else CI.contChristianNames end),20)  as FIRST_NAME, -- CMS does not allow empty string for first name
(case when Coalesce(CI.contSurname,'') = '' then ' ' else CI.contSurname end) as SURNAME, -- CMS does not allow empty string for surname
CI.contDOB,
'N' as Inactive, --always set to Inactive = N
'OPEN' as StatusCode,
B.brCode,-- oCDC.Offc Need to get this value in sync 'LON' as brCode
DEPTFEE.deptAccCode as Dept, -- oCDC.Dept Need to find out what to use clients do not have departments & may not have file (maybe sub query on last file
'FIRM' as Prof, -- oCDC.Prof profit centre code need to find out which to use maybe feeearner department (maybe subqyery on last file)
'' as clGroup, --maps to ClientCatCode table HBL_CLNT_CAT - THIS IS NOT REQUIRED
case 
	when CL.clsourcecontact is not null then 'CONTA' 
	when CL.clsourceuser is not null then 'USER'
	else CL.clSource 
END as clSource, -- maps to ClassCode table HBL_CLNT_CLASS
CL.cltypeCode, --maps to TypeCode table HBL_CLNT_TYPE limited in size to 5
CL.Created,
'STD' as TaskSetCode,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline1 ELSE CLIADD.addLine1 END AS ADDRESS_LINE_1,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline2 ELSE CLIADD.addLine2 END AS ADDRESS_LINE_2,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline3 ELSE CLIADD.addLine3 END AS ADDRESS_LINE_3,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline4 ELSE CLIADD.addLine4 END AS TOWN_CITY,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline5 ELSE CLIADD.addLine5 END AS COUNTY,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addpostcode ELSE CLIADD.addpostcode END AS POSTCODE,
case WHEN CL.clDefaultAddress is null THEN C2.ctryid ELSE C1.ctryid END AS ctryid, --these need to get in syn with HBL_country
'MAIN' as AddrTypeCode, -- MAIN, REMIT, MKTG, TA, BILL
getdate() as EffectiveDate,
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
'BUS' as ADDRESS_TYPE,
'TRUNC(SYSDATE)' as LDATE_CREATED,
'TRUNC(SYSDATE)' as LDATE_ACTIVE,
'TRUNC(SYSDATE)' as LDATE,
CL.clexttxtID as SUB_NUM
  
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

WHERE CL.clneedexport = 1 
	and CL.clextID is not null 	
	and U.usrExtID is not null 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



CREATE      procedure sprMILUpdateMatters
as

SELECT
F.fileID,
F.fileextlinkid,
F.fileNo,
C.clextID,
left(F.fileDesc,40) as MatterName,  --cannnot be over 40 characters
F.fileDesc,
F.fileExternalNotes,
F.fileStatus as fileStatus, --'OPEN' as StatusCode, --get in sync 5 characters max
FY.fileacccode as FileType, --'BOS' as FileType,  --get in sync  5 characters max
case 
	when F.filesourcecontact is not null then 'CONTA' 
	when F.filesourceuser is not null then 'USER'
	else F.fileSource  --left(F.fileSource, 5) 
END as fileSource, -- maps to ClassCode table HBL_CLNT_CLASS, -- get in sync  5 characters max
F.fileEstimate,
FY.fileAccCode,
'1' as FeeBillFormat, -- =  1,2,3 --havn't found out what this is used for yet
'N' as Contingency,
'N' as GenDisbFlag, -- = "Y";  // Y, N
'Y' as ProrateTime,
'Y' as AllowTime,   --Default value of Y used
D.deptAccCode as fileDepartment, -- CMS Dept Code F.fileDepartment,
FTACCCODE AS fileFundCode, --CMSPROF
'T' as TimeInc,
f.filestatus  as StatusCode,
f.updated as Updated,
F.fileExtLinkTxtID as MATTER_NUM,
C.clID as CLIENT_ID,
F.fileDesc as MATTERDESCRIPTION,
F.fileNotes as MATTER_NOTES,
FY.fileAccCode as WORK_TYPE,
C.clName as COMPANY_NAME,
F.fileNotes as USER_NOTES,
UR.usrInits as FE_ID_SR,
UP.usrInits as FE_ID_EA,
C.clexttxtID as SUB_NUM


FROM         
dbo.dbClient C JOIN dbo.dbFile F ON C.clid = F.clID
inner join 
	dbfundtype FT on FT.ftcode = F.filefundcode and FT.ftCurISOCode = f.filecurisocode
inner join 
	dbDepartment D on f.filedepartment = D.deptcode
left join dbuser UO on F.CreatedBy = UO.usrID
left join dbuser UM on F.filemanagerid = UM.usrID
left join dbuser UP on F.fileprincipleID = UP.usrID
left join dbuser UR on F.FileResponsibleID = UR.usrID 
left join dbuser UC on F.fileClosedby = UC.usrID
left join dbfiletype FY on FY.typecode = F.filetype
join dbBranch B on B.brid = F.brid

WHERE F.fileNeedExport = 1 and F.fileExtLinkTxtID is not null
and f.filestatus = 'LIVE'

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

*/

CREATE PROCEDURE [dbo].[sprMILExportTime]

AS
SET NOCOUNT ON

SELECT
		T.ID, 
		UPPER(U.usrInits) AS FeID, 
		CONVERT(varchar(20), T.timeRecorded, 106) AS DueDate, 
		F.fileNo AS MatterID, 
		A.actAccCode AS ActivityType, 
        T.timeDesc AS ItemDescription, 
        '' AS LitigationStageID, 
        '' AS EventID, 
        '' AS LAStructure, 
        T.timeUnits AS Units
FROM         
		dbTimeLedger AS T INNER JOIN
        dbClient AS CL ON T.clID = CL.clID INNER JOIN
        dbFile AS F ON T.fileID = F.fileID INNER JOIN
        dbUser AS U ON U.usrID = T.feeusrID INNER JOIN
        dbActivities AS A ON A.actCode = T.timeActivityCode AND Isnumeric(T.timeActivityCode) = 1 AND LEN(U.usrInits) <= 4 AND T.timeTransferred = 0
ORDER BY 
		T.Created
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

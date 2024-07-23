if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSCheckLenghts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSCheckLenghts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[Fwbs].[sprCMSExportClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Fwbs].[sprCMSExportClients]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[Fwbs].[sprCMSExportFeeEarners]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Fwbs].[sprCMSExportFeeEarners]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSExportFinancials]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSExportFinancials]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[Fwbs].[sprCMSExportMatters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Fwbs].[sprCMSExportMatters]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSExportTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSExportTime]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[Fwbs].[sprCMSExportUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Fwbs].[sprCMSExportUsers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[Fwbs].[sprCMSUpdateClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Fwbs].[sprCMSUpdateClients]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[Fwbs].[sprCMSUpdateMatters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Fwbs].[sprCMSUpdateMatters]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE procedure dbo.sprCMSCheckLenghts
as
select 
	* 
from 
	dbcodelookup 
where 
	cdtype = 'FILESTATUS' and
	len(cdcode)>5

select 
	* 
from 
	dbcodelookup 
where 
	cdtype = 'SOURCE' and
	len(cdcode)>5 and
	cdCode <>'CONTACT'

select 
	*
from
	dbDepartment 
where 
	deptacccode is null or
	len(deptacccode)>5

select 
	* 
from 
	dbFileType
where
	fileacccode is null or
	len(fileacccode)>5

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

setuser N'Fwbs'
GO

/*
-- Author: D Baldwin
-- Date Created: 17/11/2004
-- Purpose:  to enable export of clients to CMS
-- Modified By: 
-- Changes: 

*/
CREATE procedure Fwbs.sprCMSExportClients
AS
-- This intial check is for the demo Yet to be confirmed if this will remian in the fiished proc as
-- we may not have direct access to the CMS database only the Web Services
-- It updates any clients that have been exported but the update to the OMS table clextid has failed
update 
	dbclient 
set 
	clextid = client_uno
from 
	dbclient C, salesdemo..hbm_client CMSCL
where
	c.clno = CMSCL.client_code and
	c.clextid is null
-----------------------------------------------------------------------------------------------------------------------

SELECT 
CL.clID,
CL.clNo,
(case when coalesce(CL.clName,'') = '' then 'NoName' else CL.clName end) as clName, --name is required
(select top 1 contEmail from dbContactEmails where contid = CL.clDefaultContact order by contorder) as InternetAddr,
(select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'TELEPHONE' order by contorder) as PhoneNumber,
(select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'FAX' order by contorder) as FaxNumber,
(case when C.contTypecode = 'ORGANISATION' then 'O' else 'P' end) as NameType,
(case when coalesce(CI.contTitle,'') = '' then ' ' else CI.contTitle end) as contTitle,
--CI.contInitials,
(case when Coalesce(CI.contChristianNames,'') = '' then ' ' else CI.contChristianNames end)  as contChristianNames, -- CMS does not allow empty string for first name
(case when Coalesce(CI.contSurname,'') = '' then ' ' else CI.contSurname end) as contSurname, -- CMS does not allow empty string for surname
CI.contDOB,
case when c.conttypecode = 'ORGANISATION' then null else
	case 
		when CI.contMaritalStatus = 'M' then 'M'
		when CI.contMaritalStatus = 'S' then 'S'
		when CI.contMaritalStatus = 'D' then 'D'
		when CI.contMaritalStatus = 'W' then 'W'
		else 'S'
	end
END as contMaritalStatus,
CI.contSex,
'N' as Inactive, --always set to Inactive = N   --(case when CL.clPreClient = 1 then 'Y' else 'N' end) as Inactive,
'OPEN' as StatusCode, --Default of OPEN required for new clients - All Preclients are exported to CMS
B.brCode,-- oCDC.Offc Need to get this value in sync 'LON' as brCode
DEPTFEE.deptAccCode as Dept, -- oCDC.Dept Need to find out what to use clients do not have departments & may not have file (maybe sub query on last file
'FIRM' as Prof, -- MUST EXIST IN CMS!!!!!   oCDC.Prof profit centre code need to find out which to use maybe feeearner department (maybe subqyery on last file)
'' as clGroup, --maps to ClientCatCode table HBL_CLNT_CAT - THIS IS NOT REQUIRED
case 
	when CL.clsourcecontact is not null then 'CONTA' 
	when CL.clsourceuser is not null then 'USER'
	else CL.clSource 
END as clSource, -- maps to ClassCode table HBL_CLNT_CLASS
CL.cltypeCode, --maps to TypeCode table HBL_CLNT_TYPE
CL.Created,
UC.usrextid as OpenEmplUno,
U.usrExtID  as RespEmplUno,
U.usrExtID  as BillEmplUno,
'STD' as TaskSetCode, --This is required for the API - leave as STD
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline1 ELSE CLIADD.addLine1 END AS addline1,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline2 ELSE CLIADD.addLine2 END AS addline2,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline3 ELSE CLIADD.addLine3 END AS addline3,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline4 ELSE CLIADD.addLine4 END AS addline4,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline5 ELSE CLIADD.addLine5 END AS addline5,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addpostcode ELSE CLIADD.addpostcode END AS addpostcode,
case WHEN CL.clDefaultAddress is null THEN C2.ctryid ELSE C1.ctryid END AS ctryid, --these need to get in syn with HBL_country
'MAIN' as AddrTypeCode, -- MAIN, REMIT, MKTG, TA, BILL
getdate() as EffectiveDate

FROM         
	dbo.dbClient CL 
left JOIN 
	dbo.dbAddress CLIADD ON CL.clDefaultAddress = CLIADD.addID
left join 
	dbcountry C1 on CLIADD.addCountry = C1.ctryID  
join 
	dbcontact C on C.contid = CL.clDefaultContact
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
--only include clients that have not been exported and the length of client number is 6 or more characters and fee earner has been located within CMS
WHERE 
	CL.clneedexport = 1 and 
	(CL.clextID is null or CL.clextID = 0) and 
	U.usrextid is not null and
	(clsource is null or len(clsource) <=5) and
	len(clTypeCode) <=5
GO
setuser
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

setuser N'Fwbs'
GO


--NOT USED AS WE NOW USE ALL USERS




/*
-- Author: D Baldwin
-- Date Created: 22/11/2004
-- Purpose: 	used to identify fee earners that have not been set up in CMS system yet
--		Currently unable to insert into CMS so have to create exterally and then find.
-- Modified By: 
-- Changes: 
*/
CREATE Procedure sprCMSExportFeeEarners
AS

select
F.feeusrID,
---'123' as usrInits,
U.usrInits,
U.usrFullName

from dbFeeEarner F join dbuser U on F.feeusrID = U.usrID
--not sure what current efault is so 
where feeExtID is null or feeExtID = 0
GO
setuser
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*
-- Author: D Baldwin
-- Date Created: 23/Dec/2004 
-- Purpose: Retrun relevant financial entries to export --currently no CMS imnplementation returns null
-- Modified By: 
-- Changes: 
*/

CREATE PROCEDURE [dbo].[sprCMSExportFinancials] AS
return;

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

setuser N'Fwbs'
GO


/*
-- Author: D Baldwin
-- Date Created: 17/11/2004
-- Purpose:  To enable export of Matters to CMS
-- Modified By: 
-- Changes: 
*/

CREATE   procedure FWBS.sprCMSExportMatters
as
-- This intial check is for the demo Yet to be confirmed if this will remian in the fiished proc as
-- we may not have direct access to the CMS database only the Web Services
-- It updates any files that have been exported but the update to the OMS table clextid has failed
update 
	dbfile 
set 
	fileextlinkid = matter_uno
from 
	dbFile F, dbclient C, salesdemo..hbm_client CMSCL, salesdemo..hbm_matter CMSMAT
where
	f.clid = c.clid and 
	c.clno = cmscl.client_code and 
	cmscl.client_uno = cmsmat.client_uno and 
	f.fileno = cmsmat.matter_number and
	fileextlinkid is null
---------------------------------------------------------------------------------------------------------------------

SELECT 
F.fileID,
F.fileNo,
C.clextID,
getdate() as OpenDate,
left(F.fileDesc,40) as MatterName,  --cannnot be over 40 characters
F.fileDesc,
F.fileExternalNotes,
F.fileStatus as fileStatus, --'OPEN' as StatusCode, --get in sync 5 characters max
F.fileacccode as FileType, --'BOS' as FileType,  --get in sync  5 characters max
UO.usrExtID as OpenEmplUno,
case 
	when F.filesourcecontact is not null then 'CONTA' 
	when F.filesourceuser is not null then 'USER'
	else F.fileSource 
END as fileSource, -- maps to ClassCode table HBL_CLNT_CLASS, -- get in sync  5 characters max
F.fileEstimate,
F.fileAccCode,
'1' as FeeBillFormat, -- =  1,2,3 --Leave as 1 as bill format not used in OMS.  CMS API does not allow nulls.
'N' as Contingency, --Default value of N used
'N' as GenDisbFlag, --Default value of N used
'Y' as ProrateTime, --Default value of Y used
'Y' as AllowTime,   --Default value of Y used
UM.usrExtID as AssignEmplUno,  --No exposed properties
UP.usrExtID as BillEmplUno, --No exposed properties
UR.usrExtID as RespEmplUno, -- No Exposed property 
B.brCode,
D.deptAccCode as fileDepartment, -- CMS Dept Code F.fileDepartment,
FTACCCODE AS fileFundCode, --CMSPROF
'T' as TimeInc


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

--build clause that only includes files for clients that exist within CMS and live files
WHERE F.fileNeedExport = 1 and F.fileExtLinkID is null and C.clextID is not null
GO
setuser
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/*
-- Author: D Baldwin
-- Date Created: 8/12/2004
-- Purpose:  export time record to CMS
-- Modified By: 
-- Changes: 
*/
CREATE  PROCEDURE [dbo].[sprCMSExportTime] AS
SELECT 
	T.ID,
	A.actacccode as timeactivitycode,
	T.timeDesc,
	T.timeActualMins,
	T.timeCharge,
	T.timeMins,
	'N' as CalculateWip,
	F.fileExtLinkID,
	'N' as Printed,
	F.filecurISOCode,
	U.usrextid,
	T.timeRecorded,
	T.created --not used now use timeRecorded
FROM 
	dbTimeLedger T join dbFile F on T.fileID = F.fileID
join 
	dbUser U on U.usrid = T.feeusrid
join 
	dbActivities A on a.actcode = T.timeactivitycode
WHERE 
	timeTransferred = 0 and  
	F.fileExtLinkID is not null and 
	U.usrextid is not null and
	len(actacccode) <=6





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

setuser N'Fwbs'
GO

/*
-- Author: D Baldwin
-- Date Created: 22/11/2004
-- Purpose: 	To identify all users within the CMS system
-- Modified By: 
-- Changes: 
*/
CREATE Procedure sprCMSExportUsers
AS

select
U.usrID,
U.usrInits,
U.usrFullName

from dbuser U where (usrExtID is null or usrextid = 0) and usrID > 0


GO
setuser
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

setuser N'Fwbs'
GO

/*
-- Author: D Baldwin
-- Date Created: 17/11/2004
-- Purpose:  To enable update of CMS clients
-- Modified By: 
-- Changes: 
*/

CREATE procedure sprCMSUpdateClients
AS

SELECT 
CL.clID,
CL.clNo,
CL.clextid,
(case when coalesce(CL.clName,'') = '' then 'NoName' else CL.clName end) as clName, --name is required
(select top 1 contEmail from dbContactEmails where contid = CL.clDefaultContact order by contorder) as InternetAddr,
(select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'TELEPHONE' order by contorder) as PhoneNumber,
(select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'FAX' order by contorder) as FaxNumber,
(case when C.contTypecode = 'ORGANISATION' then 'O' else 'P' end) as NameType,
(case when coalesce(CI.contTitle,'') = '' then ' ' else CI.contTitle end) as contTitle,
--CI.contInitials,
(case when Coalesce(CI.contChristianNames,'') = '' then ' ' else CI.contChristianNames end)  as contChristianNames, -- CMS does not allow empty string for first name
(case when Coalesce(CI.contSurname,'') = '' then ' ' else CI.contSurname end) as contSurname, -- CMS does not allow empty string for surname
CI.contDOB,
case when c.conttypecode = 'ORGANISATION' then null else
	case 
		when CI.contMaritalStatus = 'M' then 'M'
		when CI.contMaritalStatus = 'S' then 'S'
		when CI.contMaritalStatus = 'D' then 'D'
		when CI.contMaritalStatus = 'W' then 'W'
		else 'S'
	end
END as contMaritalStatus,
CI.contSex,
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
UC.usrextid as OpenEmplUno,
U.usrExtID  as RespEmplUno,
U.usrExtID  as BillEmplUno,
'STD' as TaskSetCode,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline1 ELSE CLIADD.addLine1 END AS addline1,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline2 ELSE CLIADD.addLine2 END AS addline2,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline3 ELSE CLIADD.addLine3 END AS addline3,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline4 ELSE CLIADD.addLine4 END AS addline4,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline5 ELSE CLIADD.addLine5 END AS addline5,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addpostcode ELSE CLIADD.addpostcode END AS addpostcode,
case WHEN CL.clDefaultAddress is null THEN C2.ctryid ELSE C1.ctryid END AS ctryid, --these need to get in syn with HBL_country
'MAIN' as AddrTypeCode, -- MAIN, REMIT, MKTG, TA, BILL
getdate() as EffectiveDate
  
FROM         
	dbo.dbClient CL 
left JOIN 
	dbo.dbAddress CLIADD ON CL.clDefaultAddress = CLIADD.addID
left join 
	dbcountry C1 on CLIADD.addCountry = C1.ctryID  
join 
	dbcontact C on C.contid = CL.clDefaultContact
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

WHERE CL.clneedexport = 1 
	and CL.clextID is not null 
	and U.usrExtID is not null and
	(clsource is null or len(clsource) <=5) and
	len(clTypeCode) <=5
GO
setuser
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

setuser N'Fwbs'
GO

/*
-- Author: D Baldwin
-- Date Created: 17/11/2004
-- Purpose:  To enable update of CMS clients
-- Modified By: 
-- Changes: 
*/

CREATE procedure sprCMSUpdateMatters
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
F.fileacccode as FileType, --'BOS' as FileType,  --get in sync  5 characters max
UO.usrExtID as OpenEmplUno,
case 
	when F.filesourcecontact is not null then 'CONTA' 
	when F.filesourceuser is not null then 'USER'
	else F.fileSource 
END as fileSource, -- maps to ClassCode table HBL_CLNT_CLASS, -- get in sync  5 characters max
F.fileEstimate,
F.fileAccCode,
'1' as FeeBillFormat, -- =  1,2,3 --havn't found out what this is used for yet
'N' as Contingency,
'N' as GenDisbFlag, -- = "Y";  // Y, N
'Y' as ProrateTime,
'Y' as AllowTime,   --Default value of Y used
UM.usrExtID as AssignEmplUno,  --No exposed properties
UP.usrExtID as BillEmplUno, --No exposed properties
UR.usrExtID as RespEmplUno, -- No Exposed property 
B.brCode,
D.deptAccCode as fileDepartment, -- CMS Dept Code F.fileDepartment,
FTACCCODE AS fileFundCode, --CMSPROF
'T' as TimeInc,
F.fileClosed as CloseDate,
UC.usrextID as CloseEmplUno

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
join dbBranch B on B.brid = F.brid

WHERE F.fileNeedExport = 1 and F.fileExtLinkID is not null
GO
setuser
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSExportClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSExportClients]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSExportFeeEarners]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSExportFeeEarners]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSExportFinancials]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSExportFinancials]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSExportMatters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSExportMatters]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSExportTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSExportTime]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSExportUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSExportUsers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSUpdateClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSUpdateClients]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprCMSUpdateMatters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprCMSUpdateMatters]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





/*
-- Author: D Baldwin
-- Date Created: 17/11/2004
-- Purpose:  to enable export of clients to CMS
-- Modified By: 
-- Changes: 

*/
CREATE           procedure dbo.sprCMSExportClients
AS

declare @CMSDBNAME nvarchar(50)
declare @sql nvarchar(1000)

select top 1 @CMSDBNAME = coalesce(spData,'') from dbSpecificData where spLookup = 'CMSDBNAME' order by brid

--this should return [linkedserver].[database]


set @sql = '
update 
	dbclient 
set 
	clextid = client_uno
from 
	dbclient C, ' + @CMSDBNAME + '.dbo.hbm_client CMSCL
where
	c.clno = CMSCL.client_code collate SQL_Latin1_General_CP1_CI_AS and
	c.clextid is null
'
exec sp_executesql @sql


-----------------------------------------------------------------------------------------------------------------------

SELECT 
CL.clID,
CL.clNo,
left((case when coalesce(CL.clName,'') = '' then 'NoName' else CL.clName end),40) as clName, --name is required
(select top 1 contEmail from dbContactEmails where contid = CL.clDefaultContact order by contorder) as InternetAddr,
left((select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'TELEPHONE' order by contorder),20) as PhoneNumber,
left((select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'FAX' order by contorder),20) as FaxNumber,
(case when C.contTypecode = 'ORGANISATION' then 'O' else 'P' end) as NameType,
(case when coalesce(CI.contTitle,'') = '' then ' ' else CI.contTitle end) as contTitle,
--CI.contInitials,
left((case when Coalesce(CI.contChristianNames,'') = '' then ' ' else CI.contChristianNames end),20)  as contChristianNames, -- CMS does not allow empty string for first name
(case when Coalesce(CI.contSurname,'') = '' then ' ' else CI.contSurname end) as contSurname, -- CMS does not allow empty string for surname
CI.contDOB,
case when c.conttypecode = 'ORGANISATION' then null else
	case 
		when CI.contMaritalStatus = 'M' then 'M'
		when CI.contMaritalStatus = 'S' then 'S'
		when CI.contMaritalStatus = 'D' then 'D'
		when CI.contMaritalStatus = 'W' then 'W'
		else 'S'
	end -- to cope with CMS only acceps the 4 variations of code
END as contMaritalStatus,
coalesce(CI.contSex,'P') as contSex,
'N' as Inactive, --always set to Inactive = N   --(case when CL.clPreClient = 1 then 'Y' else 'N' end) as Inactive,
'OPEN' as StatusCode, --Default of OPEN required for new clients - All Preclients are exported to CMS
B.brCode,-- oCDC.Offc Need to get this value in sync 'LON' as brCode
DEPTFEE.deptAccCode as Dept, -- oCDC.Dept Need to find out what to use clients do not have departments & may not have file (maybe sub query on last file)
'FIRM' as Prof, -- MUST EXIST IN CMS!!!!!   oCDC.Prof profit centre code need to find out which to use maybe feeearner department (maybe sub query on last file)
'' as clGroup, --maps to ClntCatCode table HBL_CLNT_CAT - THIS IS NOT REQUIRED
case 
	when CL.clsourcecontact is not null then 'CONTA' 
	when CL.clsourceuser is not null then 'USER'
	else left(CL.clSource,5) 
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
'MAIN' as AddrTypeCode,
-- 1 as DefaultAddress,  -- optional boolean value
getutcdate() as EffectiveDate,
'OMS' as clContTypeCode, --CONT_TYPE_CODE
30 as ReminderDays,
3 as pbFormatUNO, --pre bill format 
3 as BlFormatUno, --invoice format
2 as RsFormatUno, -- reminder statement format
'UKOT' as jurisdicCode ,
2 as RateLevel,
'S' as TimeInc,
7 as ColEmplUno


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
	U.usrextid is not null 
	--(clsource is null or len(clsource) <=5) and
	--len(clTypeCode) <=5  covered by data check now DMB 11 - Feb -2005
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
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



/*
-- Author: D Baldwin
-- Date Created: 17/11/2004
-- Purpose:  To enable export of Matters to CMS
-- Modified By: 
-- Changes: 
*/

CREATE          procedure dbo.sprCMSExportMatters
as
-- This intial check is for the demo Yet to be confirmed if this will remian in the fiished proc as
-- we may not have direct access to the CMS database only the Web Services
-- It updates any files that have been exported but the update to the OMS table clextid has failed

/*
update dbfile set fileextlinkid = matter_uno
from dbFile F, dbclient C, [cmsaccs].cmsnet.dbo.hbm_client CMSCL, [cmsaccs].cmsnet.dbo.hbm_matter CMSMAT
where f.clid = c.clid and c.clno = cmscl.client_code and cmscl.client_uno = cmsmat.client_uno and f.fileno = cmsmat.matter_number and fileextlinkid is null 
*/
declare @CMSDBNAME nvarchar(50)
declare @sql nvarchar(1000)

select top 1 @CMSDBNAME = coalesce(spData,'') from dbSpecificData where spLookup = 'CMSDBNAME' order by brid
--this should return server name and database if on a linked server or just the database name
--eg  [SERVERNAME].[DATABASENAME].
--    [DATABASENAME].
--    NB must end in a .


set @sql = '
update 
	dbfile 
set 
	fileextlinkid = matter_uno
from 
	dbFile F, dbclient C, ' + @CMSDBNAME + '.dbo.hbm_client CMSCL, ' + @CMSDBNAME + '.dbo.hbm_matter CMSMAT
where
	f.clid = c.clid and 
	c.clno = cmscl.client_code and 
	cmscl.client_uno = cmsmat.client_uno and 
	f.fileno = cmsmat.matter_number and
	fileextlinkid is null and 
	isnumeric(fileno) =1'

exec sp_executesql @sql

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
FY.fileacccode as FileType, --'BOS' as FileType,  --get in sync  5 characters max
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
'T' as TimeInc,
'Y' as AllowDisp,
'Y' as AllowBill,
'N' as AllowInterest
-- added 18/4/2011
--'TEST' as TimeTypeCode, --optional
--'TEST' as MattCatCode   --optional

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
	C.clextID is not null and 
	len(filestatus) <=5 and 
	f.created >'2002-01-01'
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO









/*
-- Author: D Baldwin
-- Date Created: 8/12/2004
-- Purpose:  export time record to CMS
-- Modified By: 
-- Changes: 
*/
CREATE     PROCEDURE [dbo].[sprCMSExportTime] AS

/* -- this fix was for Chadwicks 
update [SalesDemo].dbo.tbm_matter
set
	task_plan_code = '',
	task_set_code = ''
where
	task_plan_code = 'std' or 
	task_set_code = 'std'
*/

SELECT  
	T.ID,
	A.actacccode as timeactivitycode, --actioncode
	T.timeDesc, -- NarrativeText
	T.timeActualMins, 
	T.timeCharge,
	T.timeMins,
	'Y' as CalculateWip, 
	F.fileExtLinkID, -- matteruno
	'N' as Printed, 
	F.filecurISOCode, --CurrencyCode
	U.usrextid,  --TimekeeperUNO
	T.timeRecorded, --TransactionDate
	T.created, --not used now use timeRecorded
	usrid
	-- PhaseTaskUno --optional
	-- ActivityCode --optional
FROM 
	dbTimeLedger T join dbFile F on T.fileID = F.fileID
join 
	dbUser U on U.usrid = T.feeusrid
join 
	dbActivities A on a.actcode = T.timeactivitycode
inner join 
	[SalesDemo].dbo.hbm_matter CMSMAT on CMSMAT.matter_uno = F.fileextlinkid
WHERE 
	timeTransferred = 0 and  
	F.fileExtLinkID is not null and 
	U.usrextid is not null and
	len(actacccode) <=6 and 
	f.filestatus = 'LIVE' and 
	CMSMAT.inactive ='N'
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO







/*
-- Author: D Baldwin
-- Date Created: 22/11/2004
-- Purpose: 	To identify all users within the CMS system
-- Modified By: 
-- Changes: 
*/
CREATE   Procedure sprCMSExportUsers
AS

select
U.usrID,
U.usrInits,
U.usrFullName

from dbuser U where (usrExtID is null or usrextid = 0) and usrID > 0








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO







/*
-- Author: D Baldwin
-- Date Created: 17/11/2004
-- Purpose:  To enable update of CMS clients
-- Modified By: 
-- Changes: 
*/

CREATE   procedure sprCMSUpdateClients
AS

SELECT 
CL.clID,
CL.clNo,
CL.clextid,
left((case when coalesce(CL.clName,'') = '' then 'NoName' else CL.clName end),40) as clName, --name is required
(select top 1 contEmail from dbContactEmails where contid = CL.clDefaultContact order by contorder) as InternetAddr,
left((select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'TELEPHONE' order by contorder),20) as PhoneNumber,
left((select top 1 contNumber from dbContactnumbers where contid = CL.clDefaultContact and contCode = 'FAX' order by contorder),20) as FaxNumber,
(case when C.contTypecode = 'ORGANISATION' then 'O' else 'P' end) as NameType,
(case when coalesce(CI.contTitle,'') = '' then ' ' else CI.contTitle end) as contTitle,
--CI.contInitials,
left((case when Coalesce(CI.contChristianNames,'') = '' then ' ' else CI.contChristianNames end),20)  as contChristianNames, -- CMS does not allow empty string for first name
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
coalesce(CI.contSex,'P') as contSex,
'N' as Inactive, --always set to Inactive = N
--'OPEN' as STATUS,  --optional
B.brCode,-- oCDC.Offc Need to get this value in sync 'LON' as brCode
DEPTFEE.deptAccCode as Dept, -- oCDC.Dept Need to find out what to use clients do not have departments & may not have file (maybe sub query on last file
'FIRM' as Prof, -- oCDC.Prof profit centre code need to find out which to use maybe feeearner department (maybe subqyery on last file)
'' as clGroup, --maps to ClntCatCode table HBL_CLNT_CAT - THIS IS NOT REQUIRED
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
case WHEN CL.clDefaultAddress is null THEN C2.ctryid ELSE C1.ctryid END AS ctryid, --these need to get in sync with HBL_country
'MAIN' as AddrTypeCode, 
-- 1 as DefaultAddress,  -- optional boolean value
-- 'S' as RoundUp, -- optional
getutcdate() as EffectiveDate

  
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
	and U.usrExtID is not null 
	and (clsource is null or len(clsource) <=5) --and
	--len(clTypeCode) <=5

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO








/*
-- Author: D Baldwin
-- Date Created: 17/11/2004
-- Purpose:  To enable update of CMS clients
-- Modified By: 
-- Changes: 
*/

CREATE      procedure sprCMSUpdateMatters
as

SELECT
F.fileID,
F.fileextlinkid,
F.fileNo,
C.clextID,
left(F.fileDesc,40) as MatterName,  --cannnot be over 40 characters
F.fileDesc,
F.fileExternalNotes,
f.filestatus  as StatusCode, --'OPEN' as StatusCode, --get in sync 5 characters max
FY.fileacccode as FileType, --'BOS' as FileType,  --get in sync  5 characters max
UO.usrExtID as OpenEmplUno,
case 
	when F.filesourcecontact is not null then 'CONTA' 
	when F.filesourceuser is not null then 'USER'
	else F.fileSource  --left(F.fileSource, 5) 
END as fileSource, -- maps to ClassCode table HBL_CLNT_CLASS, -- get in sync  5 characters max
F.fileEstimate,
F.fileAccCode,    -- FY.fileAccCode     
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
--F.fileClosed as CloseDate, --changed DMB/NK 6/5/2005 matters closed in CMS based on file status
null as CloseDate,
UC.usrextID as CloseEmplUno,
f.updated as Updated
-- added 18/4/2011
--'TEST' as TimeTypeCode, --optional
--'TEST' as MattCatCode   --optional

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

WHERE F.fileNeedExport = 1 and F.fileExtLinkID is not null
and f.filestatus = 'LIVE'




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


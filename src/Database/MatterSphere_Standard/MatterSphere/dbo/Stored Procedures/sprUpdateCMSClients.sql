

CREATE PROCEDURE [dbo].[sprUpdateCMSClients]
as

SELECT 
CL.clID,
CL.clNo, -- must be a minimum of 6 characters
CL.clName,
'LON' as brCode,-- B.brCode, oCDC.Offc Need to get this value in sync
'COM' as Dept, -- oCDC.Dept Need to find out what to use clients do not have departments & may not have file
'ADM' as Prof, -- oCDC.Prof profit centre code need to find out which to use maybe feeearner department ???
--CL.Created,
U.usrFullName as RespEmplUno,
U.usrFullName as BillEmplUno,
U.usrFullName as CloseEmplUno,
/*
'O' as NameType,
dateadd(YEAR,1,getdate()) as PurgeRevwDate,
getdate() as ConfCheckDate,
'N' as HasSubs,
dateadd(YEAR,1,getdate()) as CloseDate,
'D' as AutoOadAloc,
'D' as AutoOafAloc,
'N' as TimeJurOvride,
'Y' as UseUnits,
'N' as AutoRaClient,
'N' as AutoRaMatter,
'N' as AutoRaPayor,
'N' as AutoUnapAloc,
'M' as BillFreqCode,
'N' as ConsolidateBls,
'Y' as CreditHoldOk,
'N' as DisbJurOvride,
'N' as GenDisbFlag,
'N' as PayDepositInt,
'N' as DisbClass,
'N' as TimeClass,
'H' as TimeInc,
dateadd(YEAR,1,getdate()) as RetnrEndDate,
'N' as AutoRetAloc,
1 as PrevRateLevel,
1 as RateLevel,
getdate() as RateLevelDate,
*/
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline1 ELSE CLIADD.addLine1 END AS addline1,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline2 ELSE CLIADD.addLine2 END AS addline2,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline3 ELSE CLIADD.addLine3 END AS addline3,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline4 ELSE CLIADD.addLine4 END AS addline4,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addline5 ELSE CLIADD.addLine5 END AS addline5,
case WHEN CL.clDefaultAddress is null THEN CONTADD.addpostcode ELSE CLIADD.addpostcode END AS addpostcode,
--case WHEN CL.clDefaultAddress is null THEN C2.ctryCode ELSE C1.ctryCode END AS ctryCode,
'UK' AS ctryCode, -- need to get these in sync
'BILL' as AddrTypeCode, -- MAIN, REMIT, MKTG, TA
getdate() as EffectiveDate,
(select top 1 contEmail from dbContactEmails where contid = CL.clDefaultContact and contOrder = 0) as InternetAddr
--() as PhoneNumber,
--() as FaxNumber


FROM         
dbo.dbClient CL left JOIN dbo.dbAddress CLIADD ON CL.clDefaultAddress = CLIADD.addID
left join dbcountry C1 on CLIADD.addCountry = C1.ctryID  
join dbcontact C on C.contid = CL.clDefaultContact
join dbAddress CONTADD on CONTADD.addid = C.contDefaultAddress
left join dbcountry C2 on CONTADD.addCountry = C2.ctryID
join dbBranch B on B.brID = CL.brid
join dbuser U on U.usrID = CL.feeusrID

WHERE CL.clneedexport = 1 and CL.clextID is not null

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprUpdateCMSClients] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprUpdateCMSClients] TO [OMSAdminRole]
    AS [dbo];


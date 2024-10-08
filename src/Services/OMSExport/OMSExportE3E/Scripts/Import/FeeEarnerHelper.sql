-- CM: 2013-05-03

-- USER/FEE EARNER IMPORT - (HELPER SCRIPT)

-- IMPORTS TIMEKEEPERS FROM ELITE 3E INTO MATTERSPHERE (AS USERS & FEE EARNERS):
--		* RUN THIS ON THE MATTERSPHERE DATABASE
--		* ASSUMES THE NAME OF THE ELITE 3E SERVER IS "UK-3E-DEMO01" (!)
--		* ASSUMES THE NAME OF THE ELITE 3E DATABASE IS "TE_3E_SUPPORT" (!)



-- 1. Import Users
INSERT [dbo].[dbUser]
(
	[usrActive] 
	,[usrInits] 
	,[usrAlias] 
	,[usrFullName]
	,[usrADID]
	,[usrSQLID]
	,[usrWorksFor]
	,[usrExtID]
	,[usrEmail]
	,[usrNeedsExport]
	,[Created]
	,[CreatedBy]
)
SELECT 
	bu.IsActive as [usrActive]
	,SUBSTRING(fu.NetworkAlias,CHARINDEX('\',fu.NetworkAlias,1) + 1,LEN(fu.NetworkAlias)) as [usrInits]
	,SUBSTRING(fu.NetworkAlias,CHARINDEX('\',fu.NetworkAlias,1) + 1,LEN(fu.NetworkAlias)) as [usrAlias]
	,bu.BaseUserName as [usrFullName]
	,fu.NetworkAlias as [usrADID]
	,SUBSTRING(fu.NetworkAlias,CHARINDEX('\',fu.NetworkAlias,1) + 1,LEN(fu.NetworkAlias)) as [usrSQLID]
	,-200 as [usrWorksFor]	-- will get updated afterwards based on UTK.TimeKeeper > usrExtID 
	,U.Entity as [usrExtID]
	,fu.EmailAddr as [usrEmail]
	,0 as [usrNeedsExport]
	,GETUTCDATE() as Created
	,-101 as CreatedBy	-- Import User (DO NOT CHANGE!)
FROM 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.nxUser U
	INNER JOIN [UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.nxbaseuser bu ON bu.nxbaseuserID=U.nxUserID
	INNER JOIN [UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.nxfwkuser fu ON fu.nxfwkuserID=U.nxUserID
	INNER JOIN [UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.nxusertimekeeper UTK ON UTK.userID=U.nxUserID and isDefault=1


-- 2. Import Fee Earners
INSERT [dbo].[dbFeeEarner]
(
	[feeusrID]
	,[feeResponsibleTo]
	,[feeResponsible]
	,[feecurISOCode]
	,[feeSignOff]
	,[FeeExtID]
	,[FeeNeedsExport]
	,[FeeActive]
)
SELECT
	u.[usrID] as [feeusrID]
	,- 1 as [feeResponsibleTo]
	,1 as [feeResponsible]
	,'GBP' as [feecurISOCode]	-- Change to match correct code (i.e. AUD)
	,tk.[BillName] as [feeSignOff]
	,tkprIndex as [FeeExtID]
	,0 as [FeeNeedsExport]
	,CASE WHEN TkprStatus = 'ACT' THEN 1 ELSE 0 END as [FeeActive] -- Check this
FROM
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.timekeeper tk
	INNER JOIN dbUser u ON tk.BillName = u.usrFullName 
	--AND tk.Entity = u.usrExtID (check - unremming brings back no fee earners?)



/*
-- 3. Update user works for (needed?)
UPDATE u
SET usrWorksFor = fe.FeeUsrID
FROM [UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.nxUser nu
INNER JOIN [UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.nxbaseuser bu ON bu.nxbaseuserID=nu.nxUserID
INNER JOIN [UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.nxusertimekeeper UTK ON UTK.userID=nu.nxUserID
INNER JOIN dbUser u ON bu.BaseUserName = u.usrFullName AND nu.Entity = u.usrExtID
INNER JOIN dbFeeEarner fe ON UTK.Timekeeper = fe.feeExtID


-- 4. Update user responsible (needed?)
UPDATE fe
SET feeResponsibleTo = sup.feeusrID
FROM dbUser u
INNER JOIN dbFeeEarner fe ON u.UsrID = fe.FeeusrID
INNER JOIN [UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.timekeeper tk ON tk.BillName = u.usrFullName AND tk.Entity = u.usrExtID
INNER JOIN [UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.TkprDate ON TimeKeeperLkUp = TkprIndex AND NxStartDate<=GETDATE() AND NxEndDate>GetDate()
INNER JOIN dbFeeEarner sup ON SupTkpr = sup.feeExtID

*/ 


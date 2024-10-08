/****** Object:  StoredProcedure [dbo].[udSprE3EImportClients]    Script Date: 05/08/2013 16:57:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprE3EImportClients]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[udSprE3EImportClients]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprE3EImportClients]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[udSprE3EImportClients]

AS

-- IMPORTS CLIENTS FROM ELITE 3E INTO THE OMS IMPORT TABLE
--	* LIMITED TO 1 CLIENT AT A TIME BY DEFAULT
--  * ASSUMES THE NAME OF THE ELITE 3E DATABASE IS ???
-- PLEASE AMEND ACCORDINGLY!

-- * Unrem the Insert sections to perfom insert into OMSImport
-- * Select statements limited to one row (for safety) > REMOVE LINE
-- * Amend WHERE clause if necessary to restrict Clients to import


-- Drop temporary table
if exists 
(
    select  
		* 
	from 
		tempdb.dbo.sysobjects o
    where 
		o.xtype in (''U'') 
		and o.id = object_id(N''tempdb..#Client'')
)
begin
	DROP table #Client
end


-- Create temporary table (?)
SELECT 
	c.*
INTO 
	#Client
FROM 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.Client c 
	INNER JOIN 
	(
		SELECT 
			Entity
			,MAX(Number) Number 
		FROM 
			[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.Client 
		GROUP BY Entity
	) DistCli 
	ON c.Number = DistCli.Number



--Individuals

/*
INSERT INTO [OMSImport].[dbo].[ClientDetails]
(
	[extContID]
	,[clNo]
	,[addLine1]
	,[addLine2]
	,[addLine3]
	,[addLine4]
	,[addLine5]
	,[addPostCode]
	,[addCountry]
	,[addDXCode]
	,[contType]
	,[contSalut]
	,[contTitle]
	,[contFirstNames]
	,[contSurname]
	,[contSex]
	,[contNotes]
	,[contCreated]
	,[contEmail]
	,[contTelHome]
	,[contTelWork]
	,[contTelMob]
	,[contFAX]
	,[clName]
	,[clType]
	,ClientIndex
	,EntIndex
	,feeUsrID
	,brID
)
*/


SELECT
	TOP 1	-- REMOVE THIS > HERE TO LIMIT INSERTS INTO THE TABLE (!)
	c.ClientIndex as extContID
	,c.Number as clNo
	,LEFT(a.Street,64) as addLine1
	,LEFT(a.Additional1,64) as addLine2
	,LEFT(a.Additional2,64) as addLine3
	,LEFT(a.City,64) as addLine4
	,LEFT(a.[State],64) as addLine5
	,LEFT(a.ZipCode,20) as addPostCode
	,LEFT(ISNULL(a.Country, ''GB''),50) as adCountry	-- Check that this is mapped. Default to ''GB'' if it is NULL
	,NULL as addDXCode
	,CASE WHEN e.ArchetypeCode = ''EntityOrg'' THEN ''ORGANISATION'' ELSE ''INDIVIDUAL'' END as contType
	,LEFT(p.GoesBy,50) as contSalut
	,LEFT(p.Prefix,10) as contTitle
	,LEFT(p.FirstName,50) as contFirstNames
	,LEFT(p.LastName,50) as contSurname
	,CASE LEFT(p.Gender,1) WHEN ''M'' THEN ''M'' WHEN ''F'' THEN ''F'' ELSE NULL END as contSex
	,LEFT(e.Comment,4000) as contNotes
	,c.OpenDate as contCreated
	,LEFT(se.EmailAddr,200) as contEmail
	,NULL as contTelHome
	,LEFT(ph.PhoneString,30) as contTelWork
	,NULL as contTelMob
	,NULL as contFax
	,LEFT(c.DisplayName,128) as clName
	,CASE WHEN e.ArchetypeCode = ''EntityOrg'' THEN ''2'' ELSE ''1'' END as clType --1 Person, 2 Corporate
	,c.ClientIndex as ClientIndex
	,e.EntIndex as EntIndex
	,FEEOPEN.feeusrID as feeUsrID
	,BR.brID as brID		-- If branch is not set, will use ''CLBRANCH'' value from the OMSImport defaults table
FROM
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.Entity e
INNER JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.EntityPerson p ON e.EntityID = p.EntityPersonID
	--Make client Distinct due to contacts being created by extid
INNER JOIN 
	#Client c ON e.EntIndex = c.Entity
INNER JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.cliDate tk ON c.ClientIndex = tk.ClientLkUp and GETUTCDATE () BETWEEN NXSTARTDATE AND NXENDDATE
INNER JOIN
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.Relate r ON e.EntIndex = r.sbjEntity AND r.IsDefault = 1
-- Left Joins from here, not all clients appear to have a site
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[Site] s ON r.relIndex = s.Relate AND s.IsDefault = 1
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[Address] a ON s.[Address] = a.[AddrIndex]
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[SiteEmail] se on s.SiteIndex = se.[Site] AND se.IsDefault = 1
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[SitePhone] sp on s.SiteIndex = sp.[Site] AND sp.IsPrimary = 1
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[Phone] ph on ph.PhoneIndex = sp.Phone 
LEFT OUTER JOIN 
	[OMSImport].[dbo].[ClientDetails] as CD ON e.EntIndex = CD.extContID
INNER JOIN	-- The 3E TimeKeeper must be a mapped Fee Earner in MS
	dbFeeEarner FEEOPEN on FEEOPEN.feeExtID = c.OpenTkpr 
LEFT JOIN	-- The 3E Client Office
	dbBranch BR on BR.brCode = tk.Office
WHERE 
	CD.extContID IS NULL -- Not in OMSImport already
	AND CD.ClientIndex IS NULL
	AND CD.EntIndex IS NULL
	AND e.EntIndex NOT IN (SELECT ISNULL(CONTEXTID, -100) FROM [UK-3E-DEMO01].[MCELITE].[DBO].DBCONTACT) -- Not in Mattersphere already
	AND c.ClientIndex NOT IN (SELECT ISNULL(CLEXTID, -100) FROM [UK-3E-DEMO01].[MCELITE].[DBO].DBCLIENT)
ORDER BY
	[EntIndex]


--Organisations
/*
INSERT INTO [OMSImport].[dbo].[ClientDetails]
(
	[extContID]
   ,[clNo]
   ,[addLine1]
   ,[addLine2]
   ,[addLine3]
   ,[addLine4]
   ,[addLine5]
   ,[addPostCode]
   ,[addCountry]
   ,[addDXCode]
   ,[contType]
   ,[contSalut]
   ,[contTitle]
   ,[contFirstNames]
   ,[contSurname]
   ,[contSex]
   ,[contNotes]
   ,[contCreated]
   ,[contEmail]
   ,[contTelHome]
   ,[contTelWork]
   ,[contTelMob]
   ,[contFAX]
   ,[clName]
   ,[clType]
   ,ClientIndex
   ,EntIndex
   ,feeUsrID
   ,brID
)
*/


SELECT 
	TOP 1	-- REMOVE THIS > HERE TO LIMIT INSERTS INTO THE TABLE (!)
	c.ClientIndex as extContID
	,c.Number as clNo
	,LEFT(a.Street,64) as addLine1
	,LEFT(a.Additional1,64) as addLine2
	,LEFT(a.Additional2,64) as addLine3
	,LEFT(a.City,64) as addLine4
	,LEFT(a.[State],64) as addLine5
	,LEFT(a.ZipCode,20) as addPostCode
	,LEFT(ISNULL(a.Country, ''GB''),50) as adCountry	-- Check that this is mapped. Default to ''GB'' if it is NULL
	,NULL as addDXCode
	,CASE WHEN e.ArchetypeCode = ''EntityOrg'' THEN ''ORGANISATION'' ELSE ''INDIVIDUAL'' END as ContType
	,LEFT(c.DisplayName,50) as contSalut
	,NULL as contTitle
	,NULL as contFirstNames
	,LEFT(o.OrgName,50) as contSurname
	,NULL as contSex
	,LEFT(e.Comment,4000) as contNotes
	,c.OpenDate as contCreated
	,LEFT(se.EmailAddr,200) as contEmail
	,NULL as contTelHome
	,LEFT(ph.PhoneString,30) as contTelWork
	,NULL as contTelMob
	,NULL as contFax
	,LEFT(c.DisplayName,128) as clName
	,CASE WHEN e.ArchetypeCode = ''EntityOrg'' THEN ''2'' ELSE ''1'' END as clType --1 Person, 2 Corporate
	,c.ClientIndex as ClientIndex
	,e.EntIndex as EntIndex
	,FEEOPEN.feeusrID as feeUsrID	-- 3E Open Time Keeper will imported as the Client Firm Contact
	,BR.brID as brID		-- If branch is not set, will use ''CLBRANCH'' value from the OMSImport defaults table
FROM 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.Entity e
INNER JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.EntityOrg o ON e.EntityID = o.EntityOrgID
INNER JOIN -- Make client Distinct due to contacts being created by extid
	#Client c ON e.EntIndex = c.Entity
INNER JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.cliDate tk ON c.ClientIndex = tk.ClientLkUp and GETUTCDATE () BETWEEN NXSTARTDATE AND NXENDDATE
INNER JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.Relate r ON e.EntIndex = r.sbjEntity AND r.IsDefault = 1
LEFT JOIN -- Left Joins from here, not all clients appear to have a site
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[Site] s ON r.relIndex = s.Relate AND s.IsDefault = 1
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[Address] a ON s.[Address] = a.[AddrIndex]
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[SiteEmail] se on s.SiteIndex = se.[Site] AND se.IsDefault = 1
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[SitePhone] sp on s.SiteIndex = sp.[Site] AND sp.IsPrimary = 1
LEFT JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.[Phone] ph on ph.PhoneIndex = sp.Phone
LEFT OUTER JOIN 
	[OMSImport].[dbo].[ClientDetails] as CD ON e.EntIndex = CD.extContID
INNER JOIN	-- The 3E TimeKeeper must be a mapped Fee Earner in MS 
	dbFeeEarner FEEOPEN on FEEOPEN.feeExtID = c.OpenTkpr
LEFT JOIN	-- The 3E Client Office
	dbBranch BR on BR.brCode = tk.Office
WHERE 
	CD.extContID IS NULL -- Not in OMSImport already
	AND CD.ClientIndex IS NULL
	AND CD.EntIndex IS NULL
	AND e.EntIndex NOT IN (SELECT ISNULL(CONTEXTID, -100) FROM [UK-3E-DEMO01].[MCELITE].[DBO].DBCONTACT) -- Not in Mattersphere already
	AND c.ClientIndex NOT IN (SELECT ISNULL(CLEXTID, -100) FROM [UK-3E-DEMO01].[MCELITE].[DBO].DBCLIENT)
ORDER BY 
	[EntIndex]



' 
END
GO

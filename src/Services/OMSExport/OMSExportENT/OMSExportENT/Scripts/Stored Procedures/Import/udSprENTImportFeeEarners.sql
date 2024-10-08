-- CM: 2012-07-27

-- IMPORTS TIMEKEEPERS FROM ENTERPRISE INTO MATTERSPHERE (AS USERS & FEE EARNERS):
--		* ASSUMES THE NAME OF THE ENTERPRISE DATABASE IS "SON_DB"
--		* ASSUMES THE NAME OF THE MATTERSPHERE DATABASE IS "MCENTERPRISE"
--	NOTES:
--		* ENTERPRISE 'FEE EARNER' PRIMARY KEY IS TKINITS ( NVARCHAR (8))
--		* FOLLOWING EXECUTION OF THIS STORED PROC, RUN THE FOLLOWING PROC TO ADD
--		  ENTRIES TO THE INTEGRATION TABLE 'UDSPRINTEGRATIONMAPPERFEEEARNERS' 
--	CUSTOMISE EXCEPTIONS:
--		! DO NOT CHANGE VALUE OF USREXTID --> USED AS A FLAG FOR IMPORT MAPPING (PENDING -900, MAPPED -1000)
--		! DO NOT CHANGE VALUE OF CREATEDBY --> SPECIFICALLY SET AS 'IMPORT' USER




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprENTImportFeeEarners]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[udSprENTImportFeeEarners]
GO


CREATE PROCEDURE [dbo].[udSprENTImportFeeEarners]

AS


-- Create user records from TIMEKEEP table
INSERT INTO MCEnterprise.DBO.dbUser
(
	usrInits
	, usrAlias
	, usrFullName
	, brID
	, usrType
	, usrRole
	, usrLoggedIn
	, usrFailed
	, usrActive
	, usrWelcomeWizard
	, usrRTL
	, usrNeedsExport
	, usrWorksFor
	, usrMRIListCount
	, usrExchangeSync
	, usrExtID
	, usrEmail
	, Created
	, CreatedBy
)
SELECT
	tkinit as initials	-- there is also a billing inital column which is used on templates
	, 'ENT_Import' + convert ( nvarchar ( 10 ) , ( ROW_NUMBER () over ( order by tkinit ) ) ) as usrAlias -- needs to change, this field is unique
	
	, coalesce((son_db.dbo.timekeep.tkfirst + ' ' + son_db.dbo.timekeep.tklast), 'No Name') as usrFullName -- check and amend how name is imported (see line below)
	--, son_db.dbo.timekeep.tkfirst + ' ' + son_db.dbo.timekeep.tklast as usrFullName
	
	, (select brID from dbBranch where brCode = tkloc) as brID	-- Allow nulls for this? Should this ever be null?
	, 'STANDARD' as usrType
	, 'SYSTEM' as usrRole
	, 0 as usrLoggedIn
	, 0 as usrFailed
	, 1 as usrActive
	, 1 as usrWelcomeWizard
	, 0 as usrRTL
	, 1 as usrNeedsExport
	, -1 as usrWorksFor
	, 10 as usrMRIListCount
	, 0 as usrExchangeSync
	, -900 as usrExtID		-- flag : imported but not yet mapped  (DO NOT CHANGE!)
	, tkemail as usrEmail	
	, GETUTCDATE() as created
	, -101 as createdby		-- Import User (DO NOT CHANGE!)
FROM
	son_db.dbo.timekeep 
WHERE
	tklast not like '%PROPERTY%'
	and tklast not like '%BANK%'
	and tklast not like '%INSOLVENCY%'
	and tklast not like '%COMPANY%'
	and tklast not like '%CONSTRUCTION%'
	and tklast not like '%LONDON%'



--create FeeEarner records from the users from TIMEKEEP
INSERT INTO MCEnterprise.DBO.dbFeeEarner
(
	feeusrid
	, feeType
	, feeResponsible
	, feecurISOCode
	, feeCost
	, feeRateBand1
	, feeRateBand2
	, feeRateBand3
	, feeRateBand4
	, feeRateBand5
	, feeTargetHoursWeek
	, feeTargetHoursDay
	, feeActive
	, createdBy
	, feeResponsibleTo
	, feeSignOff
	, updatedBy
)
SELECT 
	MCEnterprise.DBO.dbUser.usrid
	, 'STANDARD' as feeType
	, 0 as feeResponsible
	, 'GBP' as feecurISOCode
	, 0 as feeCost
	, 0 as feeRateBand1		-- These bands would be determined by ENT
	, 0 as feeRateBand2
	, 0 as feeRateBand3
	, 0 as feeRateBand4
	, 0 as feeRateBand5
	, 40 as feeTargetHoursWeek
	, 8 as feeTargetHoursDay
	, 1 as feeActive
	, -101 as createdBy		-- Import User (DO NOT CHANGE !)
	, -1 as feeResponsibleTo
	, MCEnterprise.DBO.dbUser.usrFullName
	, -1 as updatedBy
FROM
	MCEnterprise.DBO.dbUser
WHERE
	MCEnterprise.DBO.dbUser.usrextid = -900 
	and MCEnterprise.DBO.dbUser.usrAlias like 'ENT_Import%'
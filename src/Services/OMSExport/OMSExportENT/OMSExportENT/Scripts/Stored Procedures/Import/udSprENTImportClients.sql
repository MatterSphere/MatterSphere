-- CM: 2012-07-27

-- IMPORTS CLIENTS FROM ENTERPRISE INTO THE OMS IMPORT TABLE
--	* LIMITED TO 1 CLIENT AT A TIME BY DEFAULT
--  * ASSUMES THE NAME OF THE ENTERPRISE DATABASE IS SON_DB
-- PLEASE AMEND ACCORDINGLY!


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprENTImportClients]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[udSprENTImportClients]
GO


CREATE PROCEDURE [dbo].[udSprENTImportClients]

AS

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
)
SELECT 
	TOP 1	--as a test, limit to only the top one in the list
	CL.clnum as [extContID]
	,CL.clnum as [clNo]
	,left(CL.claddr2, 50) as addLine1									--need to either tidy up addresses or establish a new validation by Elite on creation
	,left(CL.claddr3, 50) as addLine2
	,CL.claddr4 as addLine3
	,CL.claddr5 as addLine4
	,null as [addLine5]
	,CL.clzip as [addPostCode]
	,CL.clcountry as [addCountry]	-- What is the best way to get the Country ID
	,null as [addDXCode]
	,	CASE CL.cbustype 
			WHEN 'P' THEN 'INDIVIDUAL' 
			ELSE 'ORGANISATION' 
		END as [contType] -- No contact entity in Enterprise, contact type always links to client type?
	,null as [contSalut]
	, null as contTitle -- Title removed as cannot easily identify different name(s)
	,null as [contFirstNames] -- No first name in Enterprise
	,'' as [contSurname]	-- No surname in Enterprise
	,	CASE
			WHEN LEFT (cl.clcontact, 3) = 'Mrs' THEN 'F'
			WHEN LEFT (cl.clcontact, 2) = 'Mr' THEN 'M'
			WHEN LEFT (cl.clcontact, 4) = 'Miss' THEN 'F'
		END as contSex
	,null as [contNotes]
	,CL.clopendt as [contCreated]
	,null as [contEmail]
	,CL.clphone as [contTelHome]
	,null as [contTelWork]
	,null as [contTelMob]
	,CL.clfax as [contFAX]
	,ISNULL(CL.clname1,'') + ISNULL(CL.clname2,'') as [clName]
	,	CASE CL.cbustype 
			WHEN 'P' THEN 1 
			ELSE 2 
		END as [clType]
FROM 
	[son_db].[dbo].[client] CL
LEFT JOIN  -- Left Join with a check
	dbo.[fdGetExternalID] ('ENTERPRISE' , 'CLIENT' ) FILE_CLIENT_MAPPING ON FILE_CLIENT_MAPPING.ExternalID = CL.clnum
WHERE
(
	FILE_CLIENT_MAPPING.InternalID IS NULL
	OR FILE_CLIENT_MAPPING.ExternalID IS NULL

)


GO



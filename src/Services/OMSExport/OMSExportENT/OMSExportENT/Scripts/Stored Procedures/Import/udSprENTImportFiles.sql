-- CM: 2012-07-27

-- IMPORTS FILES FROM ENTERPRISE INTO THE OMS IMPORT TABLE
--	* LIMITED TO 1 FILE AT A TIME BY DEFAULT
--  * ASSUMES THE NAME OF THE ENTERPRISE DATABASE IS SON_DB
-- PLEASE AMEND ACCORDINGLY!


/****** Object:  StoredProcedure [dbo].[udSprENTImportFiles]    Script Date: 07/27/2012 14:39:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprENTImportFiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[udSprENTImportFiles]
GO


CREATE PROCEDURE [dbo].[udSprENTImportFiles]

AS


INSERT INTO [OMSImport].[dbo].[FileDetails]
           ([clNo]
           ,[fileNo]
           ,[extFileID]
           ,[fileDesc]
           ,[fileResponsibleID]
           ,[filePrincipleID]
           ,[fileDept]
           ,[fileType]
           ,[fileStatus]
           ,[fileCreated]
           ,[fileUpdated]
           ,[fileClosed]
           ,[brID]
           ,[fileManagerID]
           )	-- Customise if needed - Currency code and Fund Type taken from OMS Import Defaults table
SELECT 
	TOP 1	-- By default, only 1 at a time
	m.mclient as clNo,
	RIGHT (m.mmatter, 6) as fileNo, -- Customise if needed
	null as extFileID,
	coalesce(m.mdesc1,'') + ' ' + coalesce(m.mdesc2,'') + ' ' + coalesce(m.mdesc3,'') as fileDesc,
	FILE_RESPONSIBLE_MAPPING.INTERNALID as fileResponsibleID,	
	FILE_PRINCIPLE_MAPPING.INTERNALID as filePrincipleID,
	DEPT.deptcode as fileDept,
	FILE_TYPE.typeCode as fileType,
	CASE m.mstatus
		WHEN 'CL' THEN 'DEAD'
		WHEN 'OP' THEN 'LIVE'
		ELSE 'LIVE'		
	END as fileStatus,	-- Customise if needed
	m.mopendt as created,
	m.mmoddate as updated,
	m.mclosedt as closed,
	BRANCH.brID,
	FILE_MANAGER_MAPPING.INTERNALID as fileManagerID
FROM 
	[son_db].[dbo].[matter] m
INNER JOIN	--Inner join to only import files where timekeeper exists
	dbo.[fdGetExternalID] ('ENTERPRISE' , 'FEE EARNER' ) FILE_PRINCIPLE_MAPPING ON FILE_PRINCIPLE_MAPPING.ExternalID = m.mbillaty
INNER JOIN  --Inner join to only import files where timekeeper exists
	dbo.[fdGetExternalID] ('ENTERPRISE' , 'FEE EARNER' ) FILE_RESPONSIBLE_MAPPING ON FILE_RESPONSIBLE_MAPPING.ExternalID = m.morgaty
INNER JOIN  --Inner join to only import files where timekeeper exists
	dbo.[fdGetExternalID] ('ENTERPRISE' , 'FEE EARNER' ) FILE_MANAGER_MAPPING ON FILE_MANAGER_MAPPING.ExternalID = m.msupaty
INNER JOIN
	dbBranch BRANCH on brCode = m.mloc	
INNER JOIN
	dbDepartment DEPT on DEPT.deptAccCode = m.mdept
INNER JOIN
	dbFileType FILE_TYPE on FILE_TYPE.fileAccCode = convert(nvarchar(30), m.mprac)
INNER JOIN		-- Only import files that have a mapped Client in Mattersphere
	dbo.[fdGetExternalID] ('ENTERPRISE' , 'CLIENT' ) FILE_CLIENT_MAPPING ON FILE_CLIENT_MAPPING.ExternalID = m.mclient
LEFT JOIN
	dbo.[fdGetExternalID] ('ENTERPRISE' , 'FILE' ) FILE_MAPPING ON FILE_MAPPING.ExternalID = convert(nvarchar(7), m.mclient) + '.' + RIGHT (m.mmatter, 6)
WHERE
(
	FILE_MAPPING.InternalID is null
	OR FILE_MAPPING.ExternalID is null
)

GO

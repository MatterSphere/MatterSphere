/****** Object:  StoredProcedure [dbo].[udSprE3EImportFiles]    Script Date: 05/08/2013 16:57:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprE3EImportFiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[udSprE3EImportFiles]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprE3EImportFiles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[udSprE3EImportFiles]

AS


-- IMPORTS FILES FROM ELITE3E INTO THE OMS IMPORT TABLE
--	* LIMITED TO 1 FILE AT A TIME BY DEFAULT
--  * ASSUMES THE NAME OF THE ELITE3E DATABASE IS ????
-- PLEASE AMEND ACCORDINGLY!


-- FILE IMPORT
-- * Unrem the Insert sections to perfom insert into OMSImport
-- * Select statements limited to one row (for safety) > REMOVE LINE
-- * Amend WHERE clause if necessary to restrict Clients to import



/*
INSERT INTO [OMSImport].[dbo].[FileDetails]
(
	[clNo]
	,[fileNo]
	,[extFileID]
	,[fileDesc]
	,[fileResponsibleID]
	,[filePrincipleID]
	,[fileDept]
	,[fileType]
	,[fileFundCode]
	,[fileCurISOCode]
	,[fileStatus]
	,[fileCreated]
	,[fileUpdated]
	,[fileClosed]
	,[fileSource]
	,MattIndex
)
*/

SELECT 
	TOP 1	-- REMOVE THIS > HERE TO LIMIT INSERTS INTO THE TABLE (!)
	c.Number as clNo
	,m.Number as fileNo
	,m.MattIndex as extFileID
	,LEFT(m.DisplayName,255) as fileDesc
	,FEESPV.feeusrID as fileResponsibleID
	,FEEBILL.feeusrID as filePrincipleID
	,DEPT.deptCode as fileDept
	,FILE_TYPE.typeCode as fileType
	,NULL as fileFundCode
	,NULL as fileCurISOCode

	,CASE m.MattStatus
		WHEN ''CLOSED'' THEN ''DEAD''
		WHEN ''DECLINED'' THEN ''DEAD''	
		WHEN ''INACTIVE'' THEN ''LIVEPREINST''
		WHEN ''OPEN'' THEN ''LIVE''
		ELSE ''LIVE''		
	END as fileStatus	-- Customise if needed
	
	,m.OpenDate as fileCreated
	,m.timestamp as fileUpdated
	,m.CloseDate as fileClosed
	,NULL as fileSource
	,m.MattIndex as MattIndex
FROM 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.Matter m
INNER JOIN 
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.Client c ON Client = ClientIndex 
INNER JOIN
	[UK-3E-DEMO01].[TE_3E_SUPPORT].dbo.MattDate MD ON MD.MatterLkUp = m.MattIndex AND GETUTCDATE () BETWEEN NXSTARTDATE AND NXENDDATE
LEFT OUTER JOIN 
	[OMSImport].[dbo].[FileDetails] as f ON m.MattIndex = f.[extFileID]
INNER JOIN	
	dbBranch BRANCH on brCode = md.Office	
INNER JOIN	
	dbDepartment DEPT on DEPT.deptAccCode = md.Department
INNER JOIN	-- IF matter type is NULL in 3E create in MS under the Default Template (?)
	dbFileType FILE_TYPE on FILE_TYPE.fileAccCode = convert(nvarchar(30), ISNULL(m.MattType, ''GENERAL'')) 
INNER JOIN  
	dbClient CLIENT on CLIENT.clExtID = m.Client -- Client is mapped
INNER JOIN -- The 3E TimeKeeper must be a mapped Fee Earner in MS
	dbFeeEarner FEESPV on FEESPV.feeExtID = md.SpvTkpr 
INNER JOIN 
	dbFeeEarner FEEBILL on FEEBILL.feeExtID = md.BillTkpr 
WHERE
	f.[extFileID] IS NULL
	AND	m.MattIndex NOT IN (Select ISNULL(fileextlinkid, -100) FROM dbfile) -- File does not exist in MS
ORDER BY
	m.MattIndex
' 
END
GO

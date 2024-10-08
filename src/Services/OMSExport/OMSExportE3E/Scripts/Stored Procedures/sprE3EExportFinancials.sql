
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprE3EExportFinancials]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sprE3EExportFinancials]
GO

CREATE PROCEDURE [dbo].[sprE3EExportFinancials] 
AS
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprE3EExportFinancials_CUSTOM]') AND type in (N'P', N'PC'))
	EXEC [dbo].[sprE3EExportFinancials_CUSTOM]
ELSE
SELECT
	fl.FinLogID AS ID
	, DBO.GetE3EDisbExportXML ( fl.FinLogID ) AS COSTCARD
FROM 
	dbFinancialLedger FL
INNER JOIN 
	DBFILE ON DBFILE.FILEID = FL.FILEID
WHERE 
	FinExtID is null AND
	FL.finNeedExport = 1  AND					
	DBFILE.FILEEXTLINKID > 0					--WE HAVE A VALID MATTINDEX STORED FOR THE MATTER


GO


CREATE PROCEDURE [dbo].[srepFilWipDis]
(
	@UI uUICultureInfo='{default}',
	@FILETYPE nvarchar(50) = null,
	@DEPARTMENT nvarchar(50) = null,
	@BRANCH nvarchar(50)= null,
	@FUNDTYPE nvarchar(50) = null,
	@LACAT nvarchar(50) = null,
	@FEEEARNER bigint = null,
	@FILESTATUS nvarchar (50) = null
)

AS 

DECLARE @SQL nvarchar(max)

--- Select Statement for the base Query
SET @SQL = N'
SELECT    
	dbo.GetFileRef(CL.clNo, F.fileNo) AS Ref, 
	CL.clName,
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc,  
	F.fileCreditLimit,
	SUM(T.timeUnits) AS Units, 
	SUM(T.timeMins) AS Mins,
	SUM(T.timeCost) AS Cost,
	SUM(T.timeCharge) AS WIP,
	U.usrFullName,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileFundCode, '''') + ''~'') AS FundingType
FROM         
	dbo.dbFile F
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID 
INNER JOIN
	dbo.dbTimeLedger T ON F.fileID = T.fileID
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''FUNDTYPE'' , @UI ) CL1 ON CL1.cdCode =  F.fileFundCode
WHERE
	T.timeBilled = 0 AND
	F.fileType = COALESCE(@FILETYPE, F.fileType) AND
	F.fileDepartment = COALESCE(@DEPARTMENT, F.fileDepartment) AND
	F.brID = COALESCE(@BRANCH, F.brID) AND
	COALESCE(F.fileFundCode, '''') = COALESCE(@FUNDTYPE, F.fileFundCode, '''') AND
	COALESCE(F.fileLACategory, '''') = COALESCE(@LACAT, F.fileLACategory, '''') AND
	F.filePrincipleID = COALESCE(@FEEEARNER, F.filePrincipleID) AND
	F.fileStatus = COALESCE(@FILESTATUS, F.fileStatus)
GROUP BY
	CL.clNo,
	F.fileNo, 
	CL.clName,
	replace(F.fileDesc, char(13) + char(10), '', ''),  
	F.fileCreditLimit,
	U.usrFullName,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileFundCode, '''') + ''~'')
ORDER BY
	CL.clNo'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @FILETYPE nvarchar(50), @DEPARTMENT nvarchar(50), @BRANCH nvarchar(50), @FUNDTYPE nvarchar(50), @LACAT nvarchar(50), @FEEEARNER bigint, @FILESTATUS nvarchar(50)', @UI, @FILETYPE, @DEPARTMENT, @BRANCH, @FUNDTYPE, @LACAT, @FEEEARNER, @FILESTATUS

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilWipDis] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilWipDis] TO [OMSAdminRole]
    AS [dbo];


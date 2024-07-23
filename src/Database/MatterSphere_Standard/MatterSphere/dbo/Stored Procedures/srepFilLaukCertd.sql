

CREATE PROCEDURE [dbo].[srepFilLaukCertd]
(
	@UI uUICultureInfo='{default}'
	, @FEEEARNER nvarchar(50) = null
)

AS 

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(400)
DECLARE @ORDERBY nvarchar(100)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	CL.clNo, 
	F.fileNo, 
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	U.usrFullName, 
	F.fileFundRef, 
	LA.legAidDesc
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID 
INNER JOIN
	dbo.dbFundType FT ON F.fileFundCode = FT.ftCode AND F.filecurISOCOde = FT.ftcurISOCode
LEFT OUTER JOIN
	dbo.dbLegalAidCategory LA ON f.fileLaCategory = LA.legAidCategory '

-- where clause
SET @WHERE = N'
WHERE
	F.fileagreementdate IS NULL AND 
	F.filestatus LIKE ''LIVE'' AND
	FT.ftLegalAidCharged = ''1'' '

-- fee earner
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.filePrincipleID = @FEEEARNER '
END

-- order by clause
SET @ORDERBY = N' ORDER BY CL.clNo, F.fileNo '

-- build the complete query
DECLARE @SQL nvarchar(4000)
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

exec sp_executesql @SQL,N'
	@UI nvarchar(10),
	@FEEEARNER nvarchar(50)',
	@UI,
	@FEEEARNER

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilLaukCertd] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilLaukCertd] TO [OMSAdminRole]
    AS [dbo];


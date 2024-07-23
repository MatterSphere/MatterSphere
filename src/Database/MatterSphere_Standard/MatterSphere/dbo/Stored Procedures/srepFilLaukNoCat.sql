

CREATE PROCEDURE [dbo].[srepFilLaukNoCat]
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
	CL.clName, 
	F.fileNo, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	F.fileStatus,
   	U.usrFullName
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleId = U.usrID '

-- set where clause
SET @WHERE = N'
WHERE     
	(F.fileLACategory IS NULL OR F.fileLACategory = ''0'') AND 
	F.fileFundCode = ''LEGALAID'' '

-- fee earner
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.filePrincipleID = @FEEEARNER '
END


-- order by clause
SET @ORDERBY = N' ORDER BY U.usrFullName, CL.clNo, F.fileNo '

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
    ON OBJECT::[dbo].[srepFilLaukNoCat] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilLaukNoCat] TO [OMSAdminRole]
    AS [dbo];


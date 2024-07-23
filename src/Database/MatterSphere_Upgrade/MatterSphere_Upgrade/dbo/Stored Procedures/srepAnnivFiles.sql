CREATE PROCEDURE [dbo].[srepAnnivFiles]
(
	@UI uUICultureInfo = '{default}'
	, @DEPARTMENT nvarchar(15) = NULL
	, @BRANCH int = NULL
	, @FUNDTYPE nvarchar(15) = NULL
	, @LACAT nvarchar(15) = NULL
	, @FILESTATUS nvarchar(15) = NULL
	, @FEEEARNER int = NULL
	, @MONTH int = NULL
	, @AGREE bit = 0
)

AS 

DECLARE @MONTH_MIN6 int
IF(@MONTH > 6)
BEGIN
	SET @MONTH_MIN6 = @MONTH - 6
END

IF(@MONTH <= 6)
BEGIN
	SET @MONTH_MIN6 = @MONTH - 6 + 12
END

DECLARE @SELECT nvarchar(2450)
DECLARE @WHERE nvarchar(1500)
DECLARE @ORDERBY nvarchar(50)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT     
	CL.clName
	, replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc
	, F.fileType
	, U.usrInits AS FeeInits
	, F.fileNo
	, F.fileAllowExternal
	, CL.clNo, COALESCE(CL2.cdDesc, ''~'' + NULLIF(CAST(F.fileLACategory AS NVARCHAR(15)), '''') + ''~'') AS LACatDesc
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileFundCode, '''') + ''~'') AS FundingType
	, F.Created
	, F.fileFundRef
	, U.usrFullName
	, F.fileLACategory
	, F.fileAgreementDate
	, @AGREE AS Agree
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
	LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] =  F.fileType
	LEFT JOIN dbo.GetCodeLookupDescription ( ''LEGALAID'', @UI ) CL2 ON CL2.[cdCode] =  CAST(F.fileLACategory AS NVARCHAR(15))
	LEFT JOIN dbo.GetCodeLookupDescription ( ''FUNDTYPE'', @UI ) CL3 ON CL3.[cdCode] =  F.fileFundCode '

-- SET THE WHERE CLAUSE
SET @WHERE = ''

-- FEE EARNER CLAUSE
IF(@FEEEARNER IS NOT NULL)
BEGIN
	SET @WHERE = ' F.filePrincipleID = @FEEEARNER '
END

-- DEPARTMENT CLAUSE
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
	END
	ELSE
	BEGIN
		SET @WHERE = ' F.fileDepartment = @DEPARTMENT '
	END
END

-- BRANCH CLAUSE
IF(@BRANCH IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.brID = @BRANCH '
	END
	ELSE
	BEGIN
		SET @WHERE = ' F.brID = @BRANCH '
	END
END

-- FUND TYPE CLAUSE
IF(@FUNDTYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileFundCode = @FUNDTYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = ' F.fileFundCode = @FUNDTYPE '
	END
END

-- FILE STATUS CLAUSE
IF(@FILESTATUS IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileStatus =@FILESTATUS '
	END
	ELSE
	BEGIN
		SET @WHERE = ' F.fileStatus =@FILESTATUS '
	END
END

-- LEGAL AID CATEGORY CLAUSE
IF(@LACAT IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileLACategory = @LACAT '
	END
	ELSE
	BEGIN
		SET @WHERE =  ' F.fileLACategory = @LACAT '
	END
END

IF @AGREE = 0
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (MONTH(F.Created) = @MONTH OR MONTH(F.Created) = @MONTH_MIN6) '
	END
	ELSE
	BEGIN
		SET @WHERE = ' (MONTH(F.Created) = @MONTH OR MONTH(F.Created) = @MONTH_MIN6) '
	END
END
ELSE
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (MONTH(F.fileAgreementDate) = @MONTH OR MONTH(F.fileAgreementDate) = @MONTH_MIN6) '
	END
	ELSE
	BEGIN
		SET @WHERE = ' (MONTH(F.fileAgreementDate) = @MONTH OR MONTH(F.fileAgreementDate) = @MONTH_MIN6) '
	END
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- BUILD THE ORDER BY CLAUSE
SET @ORDERBY = '
ORDER BY
	F.Created'

DECLARE @SQL nvarchar(4000)
--- ADD STATEMENTS TOGETHER
SET @SQL = @SELECT + @WHERE + @ORDERBY

--- DEBUG PRINT
PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @DEPARTMENT nvarchar(15)
	, @BRANCH int
	, @FUNDTYPE nvarchar(15)
	, @LACAT nvarchar(15)
	, @FILESTATUS nvarchar(15)
	, @FEEEARNER int
	, @MONTH int
	, @MONTH_MIN6 int
	, @AGREE bit'
	, @UI
	, @DEPARTMENT
	, @BRANCH
	, @FUNDTYPE
	, @LACAT
	, @FILESTATUS
	, @FEEEARNER
	, @MONTH
	, @MONTH_MIN6
	, @AGREE

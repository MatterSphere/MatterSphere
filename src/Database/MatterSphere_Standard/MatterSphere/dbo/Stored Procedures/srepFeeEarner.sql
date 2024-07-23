

CREATE PROCEDURE [dbo].[srepFeeEarner]
(
	@UI uUICultureInfo='{default}'
	, @FESTATUS bit = NULL
	, @FEDEPARTMENT uCodeLookup = NULL
)

AS 
SET NOCOUNT ON
DECLARE @SELECT nvarchar(1900)
DECLARE @WHERE nvarchar(2000)
DECLARE @ORDERBY nvarchar(100)

--- BUILD THE SELECT CLAUSE
SET @SELECT = N' 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT
	USR.usrFullName AS [User Full Name]
	, USR.usrInits AS [User Initials]
	, RESPU.usrFullName AS [Responsible To]
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(FE.feeDepartment, '''') + ''~'') AS [Fee Earner Department Description]
	, USR.usrExtension
	, USR.usrDDI
	, USR.usrDDIFax
	, USR.usrEmail
	, FE.feeSignOff AS [Fee Earner Sign Off]
	, FE.feeAddSignOff AS [Fee Earner Additional Sign Off]
	, FE.feeAddRef
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(FE.feeFileType, '''') + ''~'') AS [Fee Earner File Type Description]
	, FE.feeResponsible
	, FE.feeCost
	, FE.feeRateBand1
	, FE.feeRateBand2
	, FE.feeRateBand3
	, FE.feeRateBand4
	, FE.feeRateBand5
	, FE.feeActive
	, FE.feeLARef
	, FE.feeCDSStartNum
	, FE.feeLAGrade
FROM
	dbFeeEarner FE
INNER JOIN
	dbUser USR ON USR.usrID = FE.feeUsrID
INNER JOIN
	dbUser RESPU ON RESPU.usrID = FE.feeResponsibleTo
LEFT JOIN dbo.GetCodeLookupDescription ( ''DEPT'', @UI ) CL1 ON CL1.[cdCode] = FE.feeDepartment
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL2 ON CL2.[cdCode] = FE.feeFileType '

--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- FEE EARNER STATUS CLAUSE
IF(@FESTATUS IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' FE.feeActive = @FESTATUS '
END

--- FEE EARNER DEPARTMENT CLAUSE
IF(@FEDEPARTMENT IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND FE.feeDepartment = @FEDEPARTMENT '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' FE.feeDepartment = @FEDEPARTMENT '
	END
END

--- BUILD WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- SET ORDER BY CLAUSE
SET @ORDERBY = N' 
ORDER BY 
	USR.usrFullName '

DECLARE @SQL nvarchar(4000)
--- ADD CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE + @ORDERBY

--- DEBUG PRINT
PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @FESTATUS bit
	, @FEDEPARTMENT uCodeLookup'
	, @UI
	, @FESTATUS
	, @FEDEPARTMENT

--- exec srepfeeearner @festatus = 1

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFeeEarner] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFeeEarner] TO [OMSAdminRole]
    AS [dbo];


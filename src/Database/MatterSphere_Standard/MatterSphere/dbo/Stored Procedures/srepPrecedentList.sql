

CREATE PROCEDURE [dbo].[srepPrecedentList]
(
	@UI uUICultureInfo='{default}'
	, @PRECCAT nvarchar(15) = NULL
	, @PRECSUBCAT nvarchar(15) = NULL
	, @PRECADDRESSEE nvarchar(15) = NULL
	, @PRECLIB nvarchar(15) = NULL
	, @PRECTEXT bit = NULL
)

AS 

DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1300)
DECLARE @ORDERBY nvarchar(200)

--- SET THE SELECT CLAUSE
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT  
	P.precTitle
	, P.precDesc
	, P.precType
	, P.precCategory
	, P.precSubCategory
	, P.precAddressee
	, P.precText
	, P.precMultiPrec
FROM         
	dbo.dbPrecedents P '

--- SET THE WHERE CLAUSE
SET @WHERE = ' WHERE P.precDeleted = 0 '

--- PRECEDENT CATEGORY CLAUSE
IF(@PRECCAT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND P.precCategory = @PRECCAT '
END

--- PRECEDENT SUB CATEGORY CLAUSE
IF(@PRECSUBCAT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND P.precSubCategory = @PRECSUBCAT '
END

--- PRECEDENT ADDRESSEE CLAUSE
IF(@PRECADDRESSEE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND P.precAddressee = @PRECADDRESSEE '
END

--- PRECEDENT LIBRARY CLAUSE
IF(@PRECLIB IS NOT NULL)
BEGIN
	IF(@PRECLIB = 'ALLLIVE')
	BEGIN
		SET @WHERE = @WHERE + ' AND (P.precLibrary <> ''ARCHIVE'' OR P.precLibrary IS NULL) ' 
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND P.precLibrary = @PRECLIB '
	END
END

--- PRECEDENT TEXT CLAUSE
IF(@PRECTEXT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND P.precText = @PRECTEXT '
END

--- SET THE ORDER BY CLAUSE
SET @ORDERBY = '
ORDER BY
	P.precTitle ASC '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE + @ORDERBY

--- DEBUG PRINT
PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @PRECCAT nvarchar(15)
	, @PRECSUBCAT nvarchar(15)
	, @PRECADDRESSEE nvarchar(15)
	, @PRECLIB nvarchar(15)
	, @PRECTEXT bit'
	, @UI
	, @PRECCAT
	, @PRECSUBCAT
	, @PRECADDRESSEE
	, @PRECLIB
	, @PRECTEXT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPrecedentList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPrecedentList] TO [OMSAdminRole]
    AS [dbo];


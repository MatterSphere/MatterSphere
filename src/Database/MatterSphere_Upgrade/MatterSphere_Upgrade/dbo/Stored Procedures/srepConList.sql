

CREATE PROCEDURE [dbo].[srepConList]
(
	@UI uUICultureInfo = '{default}'
	, @CREATEDBY int = NULL
	, @CONTTYPE nvarchar(15) = NULL
	, @SUBCONTTYPE nvarchar(15) = NULL
	, @LOCATION nvarchar(64) = NULL
	, @POSTCODE nvarchar(20) = NULL
	, @STARTDATE DateTime = NULL
	, @ENDDATE DateTime = NULL
	, @APPROVEDTRUE bit = 0
	, @APPROVEDFALSE bit = 0
	, @APPROVEDALL bit = 1
	, @XMASTRUE bit = 0
	, @XMASFALSE bit = 0
	, @XMASALL bit = 1
)

AS 

DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1400)
DECLARE @ORDERBY nvarchar(100)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET CONCAT_NULL_YIELDS_NULL OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	  C.contName
	, C.CreatedBy
	, C.Created
	, A.addline1
	, A.addLine2 
	, A.addLine3 
	, A.addLine4 
    , A.addLine5 
	, A.addPostcode 
	, C.contXMASCard 
	, C.contApproved 
	, C.contGrade
	, C.contAddFilter
	, C.contTypeCode
	, X.cdDesc AS contType
FROM
		dbo.dbContact C 
	INNER JOIN
		dbo.dbAddress A ON C.contDefaultAddress = A.addID
	LEFT OUTER JOIN
		dbo.GetCodeLookupDescription ( ''CONTTYPE'' , @UI ) X ON X.cdCode = C.contTypeCode '

--- SET THE WHERE STATEMENT
SET @WHERE = ''

--- CREATED BY CLAUSE
IF(@CREATEDBY IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' C.CreatedBy = @CREATEDBY '
END

--- CONTACT TYPE CLAUSE
IF(@CONTTYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND C.contTypeCode = @CONTTYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' C.contTypeCode = @CONTTYPE '
	END
END

--- SUB CONTACT TYPE CLAUSE
IF(@SUBCONTTYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND C.contAddFilter = @SUBCONTTYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' C.contAddFilter = @SUBCONTTYPE '
	END
END

--- LOCATION CLAUSE
IF(@LOCATION IS NOT NULL AND @LOCATION <> '')
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND CHARINDEX ( @LOCATION , A.addLine1 + A.addLine2 + A.addLine3 + A.addLine4 + A.addLine5 ) > 0'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' CHARINDEX ( @LOCATION , A.addLine1 + A.addLine2 + A.addLine3 + A.addLine4 + A.addLine5 ) > 0'
	END
END

--- POST CODE CLAUSE
IF(@POSTCODE IS NOT NULL AND @POSTCODE <> '')
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND A.addPostCode LIKE ''%'' + @POSTCODE + ''%'' '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' A.addPostCode LIKE ''%'' + @POSTCODE + ''%'' '
	END
END

--- CONTACT CREATED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (C.Created >= @STARTDATE AND C.Created < @ENDDATE) '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (C.Created >= @STARTDATE AND C.Created < @ENDDATE) '
	END
END

--- APPROVAL STATUS CLAUSE
IF @APPROVEDTRUE = 1
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND C.contApproved = 1 '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' C.contApproved = 1 '
	END
END
ELSE
IF @APPROVEDFALSE = 1
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND C.contApproved = 0 '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' C.contApproved = 0 '
	END
END
-- ELSE

--- XMAS CARD CLAUSE
IF @XMASTRUE = 1
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND C.contXMASCard = 1 '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' C.contXMASCard = 1 '
	END
END
ELSE
IF @XMASFALSE = 1
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND C.contXMASCard = 0 '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' C.contXMASCard = 0 '
	END
END


--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- SET THE ORDER BY STATEMENT
SET @ORDERBY = ' 
ORDER BY
	C.contName'

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--- DEBUG PRINT
 PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @CREATEDBY int
	, @CONTTYPE nvarchar(15)
	, @SUBCONTTYPE nvarchar(15)
	, @LOCATION nvarchar(64)
	, @POSTCODE nvarchar(20)
	, @STARTDATE DateTime
	, @ENDDATE DateTime
	, @APPROVEDTRUE bit
	, @APPROVEDFALSE bit
	, @APPROVEDALL bit
	, @XMASTRUE bit
	, @XMASFALSE bit
	, @XMASALL bit'
	, @UI
	, @CREATEDBY
	, @CONTTYPE
	, @SUBCONTTYPE
	, @LOCATION
	, @POSTCODE
	, @STARTDATE
	, @ENDDATE
	, @APPROVEDTRUE
	, @APPROVEDFALSE
	, @APPROVEDALL
	, @XMASTRUE
	, @XMASFALSE
	, @XMASALL

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConList] TO [OMSAdminRole]
    AS [dbo];


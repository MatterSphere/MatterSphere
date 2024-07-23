CREATE PROCEDURE dbo.schSearchAddress(
	@MAX_RECORDS INT = 50
	, @POSTCODE NVARCHAR(20) = NULL
	, @ADDRESS1 NVARCHAR(64) = NULL
	, @ADDRESS2 NVARCHAR(64) = NULL
	, @ADDRESS3 NVARCHAR(64) = NULL
	, @ADDRESS4 NVARCHAR(64) = NULL 
	, @ADDRESS5 NVARCHAR(64) = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
	, @UI uUICultureInfo = '{default}'
	) 
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Adr AS(
SELECT 
	addid
	, addline1
	, addline2
	, addline3
	, addline4
	, addline5
	, addpostcode 
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(c.ctryCode, '''') + ''~'') AS addCountryDesc 
FROM dbo.dbAddress a
	LEFT OUTER JOIN dbo.dbCountry c ON c.ctryID = a.addCountry
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''COUNTRIES'', @UI) CL ON CL.cdCode = c.ctryCode
' 
IF @ADDRESS1 IS NOT NULL
	SET @Select = @Select + N'WHERE addLine1 Like @ADDRESS1 + ''%'' OR addLine2 Like @ADDRESS1 + ''%'' OR addLine3 Like @ADDRESS1 + ''%'' 
	OR addLine4 Like @ADDRESS1 + ''%'' OR addLine5 Like @ADDRESS1 + ''%'' OR addPostCode Like @ADDRESS1 + ''%'''

SET @Select = @Select + N'
)'

IF @MAX_RECORDS > 0
	SET @Select =  @Select + N'
SELECT TOP (@MAX_RECORDS) 
	addid
	, addline1
	, addline2
	, addline3
	, addline4
	, addline5
	, addpostcode 
	, addCountryDesc
FROM Adr
'
ELSE
	SET @Select =  @Select + N'
SELECT 
	addid
	, addline1
	, addline2
	, addline3
	, addline4
	, addline5
	, addpostcode 
	, addCountryDesc
FROM Adr
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY addid'
ELSE 
	IF @ORDERBY NOT LIKE '%archID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', addid'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @Select, N'@POSTCODE NVARCHAR(20), @ADDRESS1 NVARCHAR(64), @ADDRESS2 NVARCHAR(64), @ADDRESS3 NVARCHAR(64), @ADDRESS4 NVARCHAR(64), @ADDRESS5 NVARCHAR(64), @MAX_RECORDS INT, @UI uUICultureInfo', 
		@POSTCODE, @ADDRESS1, @ADDRESS2, @ADDRESS3, @ADDRESS4, @ADDRESS5, @MAX_RECORDS, @UI
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchAddress] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchAddress] TO [OMSAdminRole]
    AS [dbo];


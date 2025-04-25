CREATE PROCEDURE dbo.schSearchClient (
	@MAX_RECORDS INT = 50
	, @SEARCH NVARCHAR(128) = ''
	, @ADDRESS NVARCHAR(150) = ''
	, @CLTYPE uCodeLookup = NULL
	, @DEBUG BIT = 0
	, @FEEUSRID BIGINT = NULL
	, @ENHANCED BIT = 0 
	, @SOUNDEX BIT = 0
	, @ORDERBY NVARCHAR(MAX) = NULL)  
AS

SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @search1 NVARCHAR(128);
DECLARE @search2 NVARCHAR(128);
DECLARE @search3 NVARCHAR(128);
DECLARE @search4 NVARCHAR(128);
DECLARE @search5 NVARCHAR(128);

DECLARE @Select NVARCHAR(MAX)
	, @Where NVARCHAR(MAX) = N'WHERE 1 = 1'

SET @Select = N'
WITH Num AS(
	SELECT clid
		, COUNT(*) AS NumAllFiles
		, COUNT(DISTINCT CASE WHEN fileStatus LIKE ''LIVE%'' THEN fileID END) AS NumLiveFiles
	FROM dbo.dbfile  
	GROUP BY clid
)
, Client AS(
SELECT CL.clID
	, CL.clNo
	, CL.clSearch1
	, CL.clSearch2
	, CL.clSearch3
	, CL.clSearch4
	, CL.clSearch5
	, CL.clName
	, ADR.addLine1
	, ADR.addLine2
	, ADR.addLine3
	, ADR.addLine4
	, ADR.addLine5
	, ADR.addPostcode
	, CONT.contName
	, U.usrInits
	, ISNULL(Num.NumAllFiles, 0) AS NumAllFiles
	, ISNULL(Num.NumLiveFiles, 0) AS NumLiveFiles
	, CT.typeGlyph
	, REPLACE(REPLACE(COALESCE(ADR.addLine1, '''') + '', '' + COALESCE(ADR.addLine2, '''') + '', '' + COALESCE(ADR.addLine3, '''') + '', '' + COALESCE(ADR.addLine4, '''') + '', '' + COALESCE(ADR.addLine5, '''') + '', '' + COALESCE(ADR.addPostCode, '''') , '', , '',  '', '') , '', , '',  '', '') AS ConcatAddress
	, CL.Created
	FROM dbo.dbclient CL
		INNER JOIN dbo.dbContact CONT ON CL.clDefaultContact = CONT.contID
		LEFT OUTER JOIN dbo.dbAddress ADR ON cont.contDefaultAddress = ADR.addID 
		INNER JOIN dbo.dbClientType CT ON CT.typeCode = CL.clTypeCode
  		LEFT OUTER JOIN dbo.dbUser U ON U.usrid = CL.feeusrid
		LEFT OUTER JOIN Num ON Num.clID = CL.clID
'
IF @CLTYPE IS NOT NULL
	SET @Where = @Where + N' 
	AND CL.clTypeCode = @CLTYPE'

IF @FEEUSRID IS NOT NULL
	SET @Where = @Where + N' 
	AND CL.feeUsrID = @FEEUSRID'

IF @ADDRESS <> '' AND @ADDRESS IS NOT NULL
	SET @Where = @where + N' 
	AND (ADR.addLine1 LIKE ''%'' + @ADDRESS + ''%''
		OR ADR.addLine2 LIKE ''%'' + @ADDRESS + ''%''
		OR ADR.addLine3 LIKE ''%'' + @ADDRESS + ''%''
		OR ADR.addLine4 LIKE ''%'' + @ADDRESS + ''%''
		OR ADR.addPostCode LIKE ''%'' + @ADDRESS + ''%'')'

IF @SEARCH <> '' AND @SEARCH IS NOT NULL
BEGIN
	IF @SOUNDEX = 1 
		SET @Where = @Where + 
		N'
	AND (SOUNDEX (CL.clSearch1) = SOUNDEX(@SEARCH) 
		OR SOUNDEX (CL.clSearch2) = SOUNDEX(@SEARCH)
		OR SOUNDEX (CL.clSearch3) = SOUNDEX(@SEARCH)
		OR SOUNDEX (CL.clSearch4) = SOUNDEX(@SEARCH)
		OR SOUNDEX (CL.clSearch5) = SOUNDEX(@SEARCH))'
	ELSE
	BEGIN
		DECLARE @searchPhrases TABLE([rowNum] [int] IDENTITY(1,1) NOT NULL, phrase NVARCHAR(128));
		DECLARE @cmptlevel int = 0;
		SELECT @cmptlevel= cmptlevel FROM master.dbo.sysdatabases WHERE name = db_name();

		IF @cmptlevel < 130 
		BEGIN
			INSERT INTO @searchPhrases
			SELECT items
			FROM SplitStringToTable(TRIM(@SEARCH), ' ');
		END
		ELSE
		BEGIN
			INSERT INTO @searchPhrases
			SELECT value FROM STRING_SPLIT(TRIM(@SEARCH), ' ')
			WHERE RTRIM(value) <> '';
		END

		DECLARE @phrasesCount INT;
		SET @phrasesCount = (SELECT COUNT(*) FROM @searchPhrases);

		IF @phrasesCount > 1
		BEGIN
			DECLARE @searchParamName NVARCHAR(128);

			SELECT 
				@search1 = (CASE WHEN rowNum = 1 THEN phrase ELSE @search1 end),
				@search2 = (CASE WHEN rowNum = 2 THEN phrase ELSE @search2 end),
				@search3 = (CASE WHEN rowNum = 3 THEN phrase ELSE @search3 end),
				@search4 = (CASE WHEN rowNum = 4 THEN phrase ELSE @search4 end),
				@search5 = (CASE WHEN rowNum = 5 THEN phrase ELSE @search5 end)
			FROM @searchPhrases

			DECLARE @i INT = 0;
			WHILE @i < @phrasesCount
			BEGIN
				SET @i = @i + 1;
				SET @searchParamName = '@search' + cast(@i as nvarchar);

				SET @Where = @Where + N'
		AND (CL.clSearch1 =  ' + @searchParamName + '
			OR CL.clSearch2 = ' + @searchParamName + '
			OR CL.clSearch3 = ' + @searchParamName + '
			OR CL.clSearch4 = ' + @searchParamName + '
			OR CL.clSearch5 = ' + @searchParamName + ')';
			END
		END
		ELSE
			SET @Where = @Where +N'
		AND (CL.clSearch1 = @SEARCH 
			OR CL.clSearch2 = @SEARCH
			OR CL.clSearch3 = @SEARCH
			OR CL.clSearch4 = @SEARCH
			OR CL.clSearch5 = @SEARCH)'
	END

	IF @ENHANCED = 1
		-- Added 19.01.09: If 'Enhanced Search' is checked, then search using a 'Like' clause but still take into account 
		-- any supplied Client Types.
		SET @Where = @Where + ' OR CL.CLNAME LIKE ''%'' + @SEARCH + ''%'' '

END

SET @Select =  @Select + @Where + N'
)
'

IF @MAX_RECORDS > 0
	SET @Select =  @Select + N'
SELECT TOP (@MAX_RECORDS) *
FROM Client
'
ELSE
	SET @Select =  @Select + N'
SELECT *
FROM Client
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY clID'
ELSE 
	IF @ORDERBY NOT LIKE '%clID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', clID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

IF @DEBUG = 1 PRINT @Select
 
EXEC sp_executesql @Select,  N'
	@SEARCH NVARCHAR(128), @ADDRESS NVARCHAR(150), @CLTYPE uCodeLookup, @FEEUSRID BIGINT, @MAX_RECORDS INT, 
	@search1 NVARCHAR(128), @search2 NVARCHAR(128), @search3 NVARCHAR(128), @search4 NVARCHAR(128), @search5 NVARCHAR(128)', 
	@SEARCH, @ADDRESS, @CLTYPE, @FEEUSRID, @MAX_RECORDS,
	@search1, @search2, @search3, @search4, @search5;

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchClient] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchClient] TO [OMSAdminRole]
    AS [dbo];


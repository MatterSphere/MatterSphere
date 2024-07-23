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
		DECLARE @searchPhrases TABLE(rowNum INT, phrase NVARCHAR(128));

		INSERT INTO @searchPhrases
		SELECT ROW_NUMBER() OVER(ORDER BY tmpTable.items ASC) AS Row#, tmpTable.items
		FROM SplitStringToTable(TRIM(@SEARCH), ' ') AS tmpTable;

		DECLARE @phrasesCount INT;
		SET @phrasesCount = (SELECT COUNT(*) FROM @searchPhrases);

		IF @phrasesCount > 1
		BEGIN
			DECLARE @phrase NVARCHAR(128);
			DECLARE @whereStr NVARCHAR(MAX) = N' 1 = 1 ';

			DECLARE @i INT = 0;
			WHILE @i < @phrasesCount
			BEGIN
				SET @i = @i + 1;
				SET @phrase = (SELECT phrase FROM @searchPhrases WHERE rowNum = @i);

				SET @Where = @Where + N'
		AND (CL.clSearch1 = ' + '''' + @phrase + '''' + '
			OR CL.clSearch2 = ' + '''' + @phrase + '''' + '
			OR CL.clSearch3 = ' + '''' + @phrase + ''''  + ' 
			OR CL.clSearch4 = ' + '''' + @phrase + '''' + '
			OR CL.clSearch5 = ' + '''' + @phrase + ''')'
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
 
EXEC sp_executesql @Select,  N'@SEARCH NVARCHAR(128), @ADDRESS NVARCHAR(150), @CLTYPE uCodeLookup, @FEEUSRID BIGINT, @MAX_RECORDS INT', @SEARCH, @ADDRESS, @CLTYPE, @FEEUSRID, @MAX_RECORDS

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchClient] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchClient] TO [OMSAdminRole]
    AS [dbo];


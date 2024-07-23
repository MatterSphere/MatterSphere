CREATE PROCEDURE search.ESAddStructureField (
	@ESIndexTableId SMALLINT
	, @TableFieldName NVARCHAR(128)
	, @FieldName NVARCHAR(128)
	, @ESFieldType NVARCHAR(50)
	, @searchable BIT
	, @facetable BIT
	, @FacetOrder TINYINT = NULL
	, @Analyzer NVARCHAR(50)
	, @ExtTable NVARCHAR(128) 
	, @Suggestable BIT
	, @cdCode NVARCHAR(15) = NULL
	, @FieldCodeLookupGroup NVARCHAR(15) = NULL
)
AS

SET NOCOUNT ON
DECLARE @ESFieldTypeE NVARCHAR(50)
	, @searchableE BIT
	, @facetableE BIT
	, @AnalyzerE NVARCHAR(50)
	, @ErrMsg NVARCHAR(MAX) = N''
	, @ObjectTypeT NVARCHAR(100)
	, @ESFieldTypeT NVARCHAR(50)
	, @searchableT NVARCHAR(MAX)
	, @facetableT NVARCHAR(MAX)
	, @AnalyzerT NVARCHAR(50)


IF EXISTS(SELECT * FROM search.ESIndexStructure WHERE ESIndexTableId = @ESIndexTableId AND FieldName = @FieldName)
BEGIN
	RAISERROR(N'Field with the name ''%s'' already exists' , 16, 1, @FieldName)
	RETURN
END

SELECT @ESFieldTypeE = eis.ESFieldType
	, @searchableE = eis.searchable
	, @facetableE = eis.facetable
	, @AnalyzerE = eis.Analyzer 
	, @ObjectTypeT = eit.ObjectType
FROM search.ESIndexStructure eis
	INNER JOIN search.ESIndexTable eit ON eit.ESIndexTableId = eis.ESIndexTableId
WHERE eis.FieldName = @FieldName 
ORDER BY eit.IsDefault, eis.IsDefault

IF @ObjectTypeT IS NOT NULL
BEGIN
	IF @ESFieldTypeE <> @ESFieldType OR @searchableE <> @searchable OR @facetableE <> @facetable OR ISNULL(@AnalyzerE, '') <> ISNULL(@Analyzer, '')
	BEGIN
		SET @ErrMsg = N'Field with the name ''%s'' already exists for ''%s'' with attributes ''field type'' = ''%s'' ''searchable'' = ''%s'' ''facetable'' = ''%s'' '' Analyzer'' = ''%s''. Please use the same values or rename the field.'
		SELECT @ESFieldTypeT = ISNULL(@ESFieldTypeE, '')
			, @searchableT = CAST(@searchableE AS NVARCHAR(MAX))
			, @facetableT = CAST(@facetableE AS NVARCHAR(MAX))
			, @AnalyzerT = ISNULL(@AnalyzerE, '')
		RAISERROR(@ErrMsg , 16, 1, @FieldName, @ObjectTypeT, @ESFieldTypeT, @searchableT, @facetableT, @AnalyzerT)
		RETURN
	END
END

BEGIN TRAN

EXEC search.ESUpdateGetDataProcedure @ESIndexTableId, @TableFieldName, @ExtTable, @FieldName, @FieldCodeLookupGroup
IF @@ERROR > 0
BEGIN
	ROLLBACK 
	RETURN
END

IF @facetable = 1 AND @cdCode IS NOT NULL
	IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = @cdCode)
		INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , @cdCode , @cdCode )

INSERT INTO  search.ESIndexStructure(ESIndexTableId, FieldName, ESFieldType, searchable, facetable, FacetOrder, Analyzer, IsDefault, ExtTable, Suggestable, cdCode, fieldCodeLookupGroup)
VALUES (@ESIndexTableId, @FieldName, @ESFieldType, @searchable, @facetable, @FacetOrder, @Analyzer, 0, @ExtTable, @Suggestable, @cdCode, @FieldCodeLookupGroup)
IF @@ERROR > 0
BEGIN
	ROLLBACK 
	RETURN
END

COMMIT TRAN
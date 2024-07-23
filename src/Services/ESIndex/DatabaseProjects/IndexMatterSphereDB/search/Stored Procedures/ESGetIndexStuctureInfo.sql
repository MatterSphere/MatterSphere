CREATE PROCEDURE search.ESGetIndexStuctureInfo (@ESIndexId SMALLINT, @UI uUICultureInfo = '{default}')
AS
SET NOCOUNT ON
SET TRAN ISOLATION LEVEL READ UNCOMMITTED

SELECT eist.ESIndexId
	, eis.ESIndexTableId
	, eis.FieldName
	, eis.ESFieldType
	, eis.searchable
	, eis.facetable
	, eis.FacetOrder
	, eis.Suggestable
	, eis.Analyzer
	, eis.IsDefault
	, eist.IndexingEnabled
	, CASE eis.facetable WHEN 1 THEN COALESCE(cl1.cdDesc, '~' + eis.cdCode + '~') END AS FieldTitle
	, eis.cdCode
FROM search.ESIndexStructure eis
	INNER JOIN search.ESIndexTable eist ON eist.ESIndexTableId = eis.ESIndexTableId
	LEFT JOIN [dbo].[GetCodeLookupDescription]('SEARCH', @UI) cl1 ON cl1.cdCode = eis.cdCode
WHERE eist.ESIndexId = @ESIndexId
UNION
SELECT
	ESIndexId
	, NULL AS ESIndexTableId
	, FieldName
	, ESFieldType
	, searchable
	, facetable
	, eis.FacetOrder
	, CAST(0 AS BIT) AS Suggestable
	, Analyzer
	, CAST(1 AS BIT) AS IsDefault
	, CAST(1 AS BIT) AS IndexingEnabled
	, CASE eis.facetable WHEN 1 THEN COALESCE(cl1.cdDesc, '~' + eis.cdCode + '~') END AS FieldTitle
	, eis.cdCode
FROM search.ESIndexCommonStructure eis
	LEFT JOIN [dbo].[GetCodeLookupDescription]('SEARCH', @UI) cl1 ON cl1.cdCode = eis.cdCode
WHERE ESIndexId = @ESIndexId

CREATE PROCEDURE search.ESGetIndexTableInfo (@ESIndexId SMALLINT)
AS
SET NOCOUNT ON
SET TRAN ISOLATION LEVEL READ UNCOMMITTED

SELECT eist.ESIndexTableId
	, eist.ESIndexId
	, eist.ObjectType
	, eist.tablename
	, eist.pkFieldName
	, eist.IndexingEnabled
	, eist.IsDefault
	, eist.summaryTemplate
FROM search.ESIndexTable eist 
WHERE eist.ESIndexId = @ESIndexId

CREATE PROCEDURE search.ESGetIndexStuctureInfoByObjectType(@ESIndexTableId SMALLINT)
AS
SET NOCOUNT ON
SET TRAN ISOLATION LEVEL READ UNCOMMITTED

SELECT eist.ESIndexId
	, eis.*
FROM search.ESIndexStructure eis
	INNER JOIN search.ESIndexTable eist ON eist.ESIndexTableId = eis.ESIndexTableId
WHERE eist.ESIndexTableId = @ESIndexTableId
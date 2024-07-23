CREATE PROCEDURE [search].[GetSummaryTemplate]
	@objectType NVARCHAR(100)
AS
SELECT summaryTemplate FROM search.ESIndexTable WHERE [ObjectType] = @objectType


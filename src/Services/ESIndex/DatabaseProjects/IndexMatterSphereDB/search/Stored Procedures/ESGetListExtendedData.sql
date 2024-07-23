CREATE PROCEDURE search.ESGetListExtendedData (@ESIndexTableId SMALLINT)
AS
SET NOCOUNT ON

DECLARE @pkFieldName NVARCHAR(128) = (SELECT pkFieldName FROM search.ESIndexTable WHERE ESIndexTableId = @ESIndexTableId)

SELECT extCode
	, extCall
	, extSourceLink
FROM dbo.dbExtendedData 
WHERE extSourceLink = @pkFieldName
	AND extModes = 15
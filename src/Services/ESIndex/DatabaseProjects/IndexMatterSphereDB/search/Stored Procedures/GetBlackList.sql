CREATE PROCEDURE search.GetBlackList
AS
SET NOCOUNT ON;

SELECT Extension
	, [Contains]
	, EncodingType
	, MaxSize
FROM search.BlackList;
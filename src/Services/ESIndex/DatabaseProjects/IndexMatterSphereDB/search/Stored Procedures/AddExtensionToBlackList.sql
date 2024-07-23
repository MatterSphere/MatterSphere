CREATE PROCEDURE search.AddExtensionToBlackList
	@Extension NVARCHAR(15)
	, @Contains NVARCHAR(1000) = ''
	, @EncodingType VARCHAR(30) = ''
	, @MaxSize BIGINT = NULL
AS
SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM search.BlackList WHERE Extension = @Extension AND [Contains] = ISNULL(@Contains, '') AND EncodingType = ISNULL(@EncodingType, ''))
	INSERT INTO search.BlackList(Extension, [Contains], EncodingType, MaxSize)
	SELECT @Extension
		, ISNULL(@Contains, '')
		, ISNULL(@EncodingType, '')
		, @MaxSize
ELSE 
	UPDATE search.BlackList 
	SET MaxSize = @MaxSize
	WHERE Extension = @Extension 
		AND [Contains] = ISNULL(@Contains, '') 
		AND EncodingType = ISNULL(@EncodingType, '');

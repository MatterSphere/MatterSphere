CREATE PROCEDURE search.DeleteExtensionFromBlackList
	@Extension NVARCHAR(15)
	, @Contains NVARCHAR(1000) = ''
	, @EncodingType VARCHAR(30) = ''
	, @FullExtension BIT = 0
AS
SET NOCOUNT ON;

DELETE search.BlackList
WHERE (Extension = @Extension AND [Contains] = ISNULL(@Contains, '') AND EncodingType = ISNULL(@EncodingType, ''))
	OR (Extension = @Extension AND @FullExtension = 1);


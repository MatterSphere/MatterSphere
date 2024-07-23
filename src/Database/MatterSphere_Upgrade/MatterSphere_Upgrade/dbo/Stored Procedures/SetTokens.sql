CREATE PROCEDURE [dbo].[SetTokens]
(
	@siteCode CHAR(2),
	@userId INT,
	@accessToken VARCHAR(2000),
	@accessTokenExpiresAt DATETIME,
	@refreshToken VARCHAR(200) = NULL,
	@refreshTokenExpiresAt DATETIME = NULL
)
AS
	MERGE INTO [dbo].[dbTokens] AS Target
	USING (VALUES(@siteCode, @userId, @accessToken, @accessTokenExpiresAt, @refreshToken, @refreshTokenExpiresAt))
	AS Source (siteCode, userId, accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt)
	ON Target.siteCode = Source.siteCode AND ((Target.userId IS NULL AND Source.userId IS NULL) OR (Target.userId = Source.userId))
	WHEN MATCHED THEN
	UPDATE SET accessToken = Source.accessToken
	, accessTokenExpiresAt = Source.accessTokenExpiresAt
	, refreshToken = Source.refreshToken
	, refreshTokenExpiresAt = Source.refreshTokenExpiresAt
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (siteCode, userId, accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt)
	VALUES (Source.siteCode, Source.userId, Source.accessToken, Source.accessTokenExpiresAt, Source.refreshToken, Source.refreshTokenExpiresAt);

GO
GRANT EXECUTE
	ON OBJECT::[dbo].[SetTokens] TO [OMSRole]
	AS [dbo];

GO
GRANT EXECUTE
	ON OBJECT::[dbo].[SetTokens] TO [OMSAdminRole]
	AS [dbo];
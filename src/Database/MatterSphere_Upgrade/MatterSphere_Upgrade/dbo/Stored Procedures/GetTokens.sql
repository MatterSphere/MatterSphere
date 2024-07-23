CREATE PROCEDURE [dbo].[GetTokens]
(
	@siteCode CHAR(2),
	@userId INT
)
AS
	SELECT accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt
	FROM dbo.dbTokens
	WHERE siteCode = @siteCode AND ((userId IS NULL AND @userId IS NULL) OR (userId = @userId))

GO
GRANT EXECUTE
	ON OBJECT::[dbo].[GetTokens] TO [OMSRole]
	AS [dbo];

GO
GRANT EXECUTE
	ON OBJECT::[dbo].[GetTokens] TO [OMSAdminRole]
	AS [dbo];

CREATE PROCEDURE [dbo].[srepCreateNotification]
(
	@rpdNotfAppID nvarchar(100)
	,@rpdNotfFormattedDate datetime
	,@rpdNotfDateTime datetime
	,@rpdNotfGuid nvarchar(100)
	,@rpdNotfMessage nvarchar(1000)
	,@rpdNotfClaimXml xml = null
)
AS 
BEGIN
	SET NOCOUNT ON
	--SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	--SELECT 0,0

	IF (@rpdNotfAppID IS NOT NULL)
	BEGIN -- 1
		IF NOT EXISTS (
						SELECT * 
						FROM fdRaPIdNotificationList 
						WHERE 
							rpdNotfAppID = @rpdNotfAppID
							AND rpdNotfFormattedDate = @rpdNotfFormattedDate
							AND rpdNotfDateTime = @rpdNotfDateTime
							AND rpdNotfGuid = @rpdNotfGuid
							AND rpdNotfMessage = @rpdNotfMessage
						)
			BEGIN
				INSERT INTO fdRaPIdNotificationList (
					rpdNotfAppID
					,rpdNotfFormattedDate
					,rpdNotfDateTime
					,rpdNotfGuid
					,rpdNotfMessage
					,rpdNotfClaimXml
					)
					VALUES (
					@rpdNotfAppID
					,@rpdNotfFormattedDate
					,@rpdNotfDateTime
					,@rpdNotfGuid
					,@rpdNotfMessage
					,@rpdNotfClaimXml
					)

					SELECT 1, SCOPE_IDENTITY()
			END
			ELSE
			BEGIN
				SELECT 0,0
			END
	END -- 1
	ELSE
	BEGIN
		SELECT 0,0
	END
END
GO

GRANT EXECUTE ON [dbo].[srepCreateNotification] TO PUBLIC
GO

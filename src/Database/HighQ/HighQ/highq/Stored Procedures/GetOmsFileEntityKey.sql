CREATE PROCEDURE [highq].[GetOmsFileEntityKey]
	@clientId BIGINT
AS
	SELECT omsFileEntityId FROM highq.dbClient WHERE clID = @clientId

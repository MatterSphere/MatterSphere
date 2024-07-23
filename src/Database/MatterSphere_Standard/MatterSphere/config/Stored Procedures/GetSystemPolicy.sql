

CREATE PROCEDURE [config].[GetSystemPolicy]
	@policyID uniqueidentifier = NULL ,
	@groupID uniqueidentifier = NULL ,
	@userID int = NULL

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF @policyID IS NOT NULL
BEGIN
	SELECT [ID] , [Type] , [AllowMask] , [DenyMask] , [Name] FROM [config].[SystemPolicy] WHERE [ID] = @policyID
	RETURN
END

IF @userID IS NOT NULL
BEGIN
	SELECT SP.[ID] , SP.[Type] , SP.[AllowMask] , SP.[DenyMask] , SP.[Name] FROM [config].[SystemPolicy] SP
	JOIN [item].[User] IU ON IU.PolicyID = SP.[ID] 
	JOIN [dbo].[dbUser] U ON U.usrADID = IU.NTLogin WHERE U.[usrID] = @userID
	RETURN
END

IF @groupID IS NOT NULL
BEGIN
	SELECT SP.[ID] , SP.[Type] , SP.[AllowMask] , SP.[DenyMask] , SP.[Name] FROM [config].[SystemPolicy] SP
	JOIN [item].[Group] IG ON IG.PolicyID = SP.[ID] WHERE IG.[ID] = @groupID
	RETURN
END



GO
GRANT EXECUTE
    ON OBJECT::[config].[GetSystemPolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetSystemPolicy] TO [OMSAdminRole]
    AS [dbo];





CREATE PROCEDURE [config].[GetFilePermissions]
	@objectID bigint , 
	@children bit,
	@status bit = 0 ,
	@User nvarchar(200)

AS
SET NOCOUNT ON 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

declare @securableType uCodeLookup
set @securableType = 'FILE'

DECLARE @RemotePolicy bit
IF (SELECT AccessType FROM dbUser where usrADID = @USER) = 'INTERNAL'
	SET @RemotePolicy = 0
ELSE
	SET @RemotePolicy = 1

IF ( SELECT [config].[IsAdministrator] (@USER) ) = 1
BEGIN
	IF @status = 0
	BEGIN
		SELECT TOP 0
			[SecurableType] , [Permission] , 1 as [Allow] , 0 as[Deny] , [NodeLevel]
		FROM
			[config].[ObjectPolicyConfig] 
		RETURN
	END
	ELSE
	BEGIN
		SELECT
			[SecurableType] , [Permission] , 1 as [Allow] , 0 as[Deny] , [NodeLevel]
		FROM
			[config].[ObjectPolicyConfig] 
		WHERE
			SecurableType = @securableType AND NOT Permission IS NULL
		RETURN
	END

END

IF @status = 0
BEGIN

	SELECT A.[SecurableType] , A.[Permission] , @objectID as ObjectID, 1 as [Deny],  0 as [Allow] FROM 
	(
	SELECT [SecurableType] , [Permission] ,  1 as [Allow] , 0 as [Deny] , [NodeLevel] FROM [config].[ObjectPolicyConfig] X
	CROSS JOIN
	[relationship].[UserGroup_File] Y
	JOIN [config].[ObjectPolicy] OP ON OP.ID = Y.PolicyID AND COALESCE (OP.IsRemote,0) = @RemotePolicy
	WHERE X.[SecurableType] IS NOT NULL AND X.[Permission] IS NOT NULL AND Y.[FileID] = @objectID
	AND CASE WHEN @children = 1 THEN X.[SecurableType] ELSE @securableType END = X.[SecurableType]
	EXCEPT
	SELECT 
		MTP.[SecurableType] ,
		MTP.[PermissionCode] as [Permission],
		CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 0 ELSE 1 END as [Allow] ,
		CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 1 ELSE 0 END as [Deny] ,
		MTP.[NodeLevel]
	FROM 
		[config].[ObjectPolicy] OP 
	JOIN
		[relationship].[UserGroup_File] UG ON UG.[PolicyID] = OP.[ID] CROSS APPLY [config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MTP
	JOIN
		[config].[GetUserAndGroupMembershipNT] (@USER) GM ON GM.[ID] = UG.[UserGroupID]
	WHERE
		UG.[FileID] = @objectID 
	AND
		CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
	GROUP BY
		MTP.[PermissionCode] , MTP.[SecurableType] , UG.[FileID] , MTP.[NodeLevel]
	) A
	ORDER BY
		A.[NodeLevel]
	RETURN

END

IF @status = 1
BEGIN

	SELECT A.[SecurableType] , A.[Permission] , @objectID as ObjectID, 0 as [Deny], 1 as [Allow] FROM 
	(
	SELECT [SecurableType] , [Permission] ,  1 as [Allow] , 0 as [Deny] , [NodeLevel] FROM [config].[ObjectPolicyConfig] X
	CROSS JOIN
	[relationship].[UserGroup_File] Y
	JOIN [config].[ObjectPolicy] OP ON OP.ID = Y.PolicyID AND COALESCE (OP.IsRemote,0) = @RemotePolicy
	WHERE X.[SecurableType] IS NOT NULL AND X.[Permission] IS NOT NULL AND Y.[FileID] = @objectID
	AND CASE WHEN @children = 1 THEN X.[SecurableType] ELSE @securableType END = X.[SecurableType]
	INTERSECT
	SELECT 
		MTP.[SecurableType] ,
		MTP.[PermissionCode] as [Permission],
		CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 0 ELSE 1 END as [Allow] ,
		CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 1 ELSE 0 END as [Deny] ,
		MTP.[NodeLevel]
	FROM 
		[config].[ObjectPolicy] OP 
	JOIN
		[relationship].[UserGroup_File] UG ON UG.[PolicyID] = OP.[ID] CROSS APPLY [config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MTP
	JOIN
		[config].[GetUserAndGroupMembershipNT] (@USER) GM ON GM.[ID] = UG.[UserGroupID]
	WHERE
		UG.[FileID] = @objectID 
	AND
		CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
	GROUP BY
		MTP.[PermissionCode] , MTP.[SecurableType] , UG.[FileID] , MTP.[NodeLevel]
	) A
	ORDER BY
		A.[NodeLevel]
	RETURN

END




	SELECT A.[SecurableType] , A.[Permission] , @objectID as ObjectID, A.[Deny], A.[Allow] FROM 
	(
	
	SELECT 
		MTP.[SecurableType] ,
		MTP.[PermissionCode] as [Permission],
		CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 0 ELSE 1 END as [Allow] ,
		CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 1 ELSE 0 END as [Deny] ,
		MTP.[NodeLevel]
	FROM 
		[config].[ObjectPolicy] OP 
	JOIN
		[relationship].[UserGroup_File] UG ON UG.[PolicyID] = OP.[ID] CROSS APPLY [config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MTP
	JOIN
		[config].[GetUserAndGroupMembershipNT] (@USER) GM ON GM.[ID] = UG.[UserGroupID]
	WHERE
		UG.[FileID] = @objectID 
	AND
		CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
	GROUP BY
		MTP.[PermissionCode] , MTP.[SecurableType] , UG.[FileID] , MTP.[NodeLevel]
	
	) A
	ORDER BY
		A.[NodeLevel]
	RETURN



GO
GRANT EXECUTE
    ON OBJECT::[config].[GetFilePermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetFilePermissions] TO [OMSAdminRole]
    AS [dbo];


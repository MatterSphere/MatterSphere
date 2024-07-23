

CREATE PROCEDURE [config].[GetDocumentPermissions]

	@objectID bigint , 
	@children bit,
	@status bit = 0 ,
	@User nvarchar(200)

AS
SET NOCOUNT ON 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

declare @securableType uCodeLookup
set @securableType = 'DOCUMENT'

DECLARE @T Table (
[SecurableType] nvarchar(200), 
[Permission] nvarchar(200), 
[ObjectId] bigint,
[Allow] bit, 
[Deny] bit) 

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
		INSERT into @T
		SELECT A.[SecurableType] , A.[Permission] , @objectID as ObjectID, 1 as [Deny], 0 as [Allow] FROM 
		(
		SELECT [SecurableType] , [Permission] ,  1 as [Allow] , 0 as [Deny] , [NodeLevel] FROM [config].[ObjectPolicyConfig] X
		CROSS JOIN
		[relationship].[UserGroup_Document] Y
		JOIN [config].[ObjectPolicy] OP ON OP.ID = Y.PolicyID AND COALESCE (OP.IsRemote,0) = @RemotePolicy
		WHERE X.[SecurableType] IS NOT NULL AND X.[Permission] IS NOT NULL AND Y.[DocumentID] = @objectID
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
			[relationship].[UserGroup_Document] UG ON UG.[PolicyID] = OP.[ID] CROSS APPLY [config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MTP
		JOIN
			[config].[GetUserAndGroupMembershipNT] (@USER) GM ON GM.[ID] = UG.[UserGroupID]
		WHERE
			UG.[DocumentID] = @objectID 
		AND
			CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
		GROUP BY
			MTP.[PermissionCode] , MTP.[SecurableType] , UG.[DocumentID] , MTP.[NodeLevel]
		) A
		ORDER BY
			A.[NodeLevel]

		IF EXISTS (SELECT 1 from dbreginfo WHERE regBlockInheritence = 1)
			BEGIN
				INSERT INTO @T
				SELECT A.[SecurableType] , A.[Permission] , @objectID as ObjectID, 1 as [Deny], 0 as [Allow] FROM 
				(
				SELECT [SecurableType] , [Permission] ,  1 as [Allow] , 0 as [Deny] , [NodeLevel] FROM [config].[ObjectPolicyConfig] X
				CROSS JOIN
				[relationship].[UserGroup_File] Y
				left JOIN [config].[ObjectPolicy] OP ON OP.ID = Y.PolicyID AND COALESCE (OP.IsRemote,0) = @RemotePolicy
				WHERE X.[SecurableType] IS NOT NULL 
				AND X.[Permission] IS NOT NULL 
				AND Y.[FileID] = (select fileid from config.dbdocument where docid = @objectID )
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
					UG.[FileID] = (select fileid from config.dbdocument where docid = @objectID )
				AND
					CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
				GROUP BY
					MTP.[PermissionCode] , MTP.[SecurableType] , UG.[FileID] , MTP.[NodeLevel]
				) A
				ORDER BY
					A.[NodeLevel]
			End

		SELECT * from @T
		RETURN

END

IF @status = 1
BEGIN
		INSERT into @T
		SELECT A.[SecurableType] , A.[Permission] , @objectID as ObjectID, 0 as [Deny], 1 as [Allow] FROM 
		(
		SELECT [SecurableType] , [Permission] ,  1 as [Allow] , 0 as [Deny] , [NodeLevel] FROM [config].[ObjectPolicyConfig] X
		CROSS JOIN
		[relationship].[UserGroup_Document] Y
		JOIN [config].[ObjectPolicy] OP ON OP.ID = Y.PolicyID AND COALESCE (OP.IsRemote,0) = @RemotePolicy
		WHERE X.[SecurableType] IS NOT NULL AND X.[Permission] IS NOT NULL AND Y.[DocumentID] = @objectID
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
			[relationship].[UserGroup_Document] UG ON UG.[PolicyID] = OP.[ID] CROSS APPLY [config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MTP
		JOIN
			[config].[GetUserAndGroupMembershipNT] (@USER) GM ON GM.[ID] = UG.[UserGroupID]
		WHERE
			UG.[DocumentID] = @objectID 
		AND
			CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
		GROUP BY
			MTP.[PermissionCode] , MTP.[SecurableType] , UG.[DocumentID] , MTP.[NodeLevel]
		) A
		ORDER BY
			A.[NodeLevel]

		IF EXISTS (SELECT 1 from dbreginfo WHERE regBlockInheritence = 1)
			BEGIN
				INSERT INTO @T
				SELECT A.[SecurableType] , A.[Permission] , @objectID as ObjectID, 1 as [Deny], 0 as [Allow] FROM 
				(
				SELECT [SecurableType] , [Permission] ,  1 as [Allow] , 0 as [Deny] , [NodeLevel] FROM [config].[ObjectPolicyConfig] X
				CROSS JOIN
				[relationship].[UserGroup_File] Y
				left JOIN [config].[ObjectPolicy] OP ON OP.ID = Y.PolicyID AND COALESCE (OP.IsRemote,0) = @RemotePolicy
				WHERE X.[SecurableType] IS NOT NULL 
				AND X.[Permission] IS NOT NULL 
				AND Y.[FileID] = (select fileid from config.dbdocument where docid = @objectID )
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
					UG.[FileID] = (select fileid from config.dbdocument where docid = @objectID )
				AND
					CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
				GROUP BY
					MTP.[PermissionCode] , MTP.[SecurableType] , UG.[FileID] , MTP.[NodeLevel]
				) A
				ORDER BY
					A.[NodeLevel]
			End

		SELECT * from @T
		RETURN

END

		INSERT INTO @T
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
			[relationship].[UserGroup_Document] UG ON UG.[PolicyID] = OP.[ID] CROSS APPLY [config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MTP
		JOIN
			[config].[GetUserAndGroupMembershipNT] (@USER) GM ON GM.[ID] = UG.[UserGroupID]
		WHERE
			UG.[DocumentID] = @objectID 
		AND
			CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
		GROUP BY
			MTP.[PermissionCode] , MTP.[SecurableType] , UG.[DocumentID] , MTP.[NodeLevel]
		) A
		ORDER BY
			A.[NodeLevel]
	IF EXISTS (SELECT 1 from dbreginfo WHERE regBlockInheritence = 1)
		BEGIN
			INSERT INTO @T
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
				UG.[FileID] = (select fileid from config.dbdocument where docid = @objectID )
			AND
				CASE WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
			GROUP BY
				MTP.[PermissionCode] , MTP.[SecurableType] , UG.[FileID] , MTP.[NodeLevel]
			) A
			ORDER BY
				A.[NodeLevel]			
		END

	SELECT * from @T
	RETURN





GO
GRANT EXECUTE
    ON OBJECT::[config].[GetDocumentPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetDocumentPermissions] TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [config].[GetSystemPermissions]
	@children bit,
	@status bit = 0 ,
	@User nvarchar(200)

AS
SET NOCOUNT ON 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

declare @securableType uCodeLookup
set @securableType = 'SYSTEM'

	SELECT 
		MTP.[SecurableType] ,
		MTP.[PermissionCode] as [Permission] ,
		NULL as ObjectID,
		(CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 1 ELSE 0 END) AS [Deny],
		(CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 0 ELSE 1 END) AS [Allow]
	FROM 
		[config].[SystemPolicy] SP 
	JOIN
		[config].[GetUserAndGroupMembershipNT] (@USER) GM ON GM.[PolicyID] = SP.[ID] CROSS APPLY [config].[SystemMaskToPermissions] ( SP.[AllowMask] , SP.[DenyMask] ) MTP
	WHERE
		CASE
			WHEN @children = 1 THEN MTP.[SecurableType] ELSE @securableType END = MTP.[SecurableType] 
--		AND
--			MTP.[Allow] = 0 
	GROUP BY 
		MTP.[PermissionCode] ,  MTP.[SecurableType] 

	HAVING
		@status IS NULL
		OR
		(NOT @status IS NULL AND
		 (CASE WHEN Sum([Deny]) > 0 OR Sum([Allow]) = 0 THEN 0 ELSE 1 END) = @status)
		 	
		-- This is done to bring back all not set system permissions along with the explicitly denied (System Roles Only)
	RETURN


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetSystemPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetSystemPermissions] TO [OMSAdminRole]
    AS [dbo];


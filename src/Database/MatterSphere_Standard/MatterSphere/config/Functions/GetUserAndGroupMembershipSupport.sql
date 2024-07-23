

CREATE FUNCTION [config].[GetUserAndGroupMembershipSupport] ( @ntLogin nvarchar(200) ) 
RETURNS table

RETURN
(
	SELECT
		ID , [Name] , [PolicyID]
	FROM
		[item].[user] U
	WHERE
		[NTlogin] = @ntLogin
	)
	UNION ALL
	(
	SELECT
		G.ID , G.[Name] , G.[PolicyID]
	FROM
		[item].[User] U
	JOIN
		[relationship].[Group_User] GU ON GU.UserID = U.[ID]
	JOIN
		[item].[Group] G ON G.[ID] = GU.[GroupID]
	WHERE 
		U.[NTlogin] = @ntLogin
	)
	UNION ALL
	(	
	SELECT
		ID , [Name] , [PolicyID]
	FROM
		[item].[Group]
	WHERE
		[Name] = 'Everyone'
)



GO
GRANT UPDATE
    ON OBJECT::[config].[GetUserAndGroupMembershipSupport] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[GetUserAndGroupMembershipSupport] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[GetUserAndGroupMembershipSupport] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[GetUserAndGroupMembershipSupport] TO [OMSApplicationRole]
    AS [dbo];


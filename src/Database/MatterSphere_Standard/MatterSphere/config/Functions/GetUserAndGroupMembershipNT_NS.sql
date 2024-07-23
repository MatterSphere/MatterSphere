

CREATE FUNCTION [config].[GetUserAndGroupMembershipNT_NS] ()
RETURNS table

RETURN
(
	SELECT
		ID , [Name] , [PolicyID]
	FROM
		[item].[User] U
	WHERE
		[NTLogin] = config.GetUserLogin()  
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
		U.[NTLogin] = config.GetUserLogin() AND G.[Active] = 1
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
    ON OBJECT::[config].[GetUserAndGroupMembershipNT_NS] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[GetUserAndGroupMembershipNT_NS] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[GetUserAndGroupMembershipNT_NS] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[GetUserAndGroupMembershipNT_NS] TO [OMSApplicationRole]
    AS [dbo];


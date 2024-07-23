

CREATE FUNCTION [config].[GetUserAndGroupMembershipNT] ( @USER nvarchar(200) )
RETURNS table
RETURN
(
	SELECT
		ID , [Name] , [PolicyID] , 0 as Gtype
	FROM
		[item].[User] U
	WHERE
		[NTLogin] = @USER
	)
	UNION ALL
	(
	SELECT
		G.ID , G.[Description] , G.[PolicyID] , 1 as Gtype
	FROM
		[item].[User] U
	JOIN
		[relationship].[Group_User] GU ON GU.UserID = U.[ID]
	JOIN
		[item].[Group] G ON G.[ID] = GU.[GroupID]
	WHERE 
		U.[NTLogin] = @USER AND G.[Active] = 1
	)
	UNION ALL
	(	
	SELECT
		ID , [Name] , [PolicyID] , 2 as Gtype
	FROM
		[item].[Group]
	WHERE
		[Name] = 'Everyone'
)


GO
GRANT UPDATE
    ON OBJECT::[config].[GetUserAndGroupMembershipNT] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[GetUserAndGroupMembershipNT] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[GetUserAndGroupMembershipNT] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[GetUserAndGroupMembershipNT] TO [OMSApplicationRole]
    AS [dbo];


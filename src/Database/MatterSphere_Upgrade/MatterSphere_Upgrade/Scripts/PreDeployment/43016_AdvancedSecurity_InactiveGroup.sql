SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [config].[GetUserAndGroupMembershipNT] ( @USER nvarchar(200) )
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


ALTER FUNCTION [config].[GetUserAndGroupMembershipNT_NS] ()
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
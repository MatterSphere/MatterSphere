

CREATE FUNCTION [config].[GetUserAndGroupMembership] ()
RETURNS table

RETURN
(
SELECT
	U.ID , R.usrFullName as [Name]
FROM
	[item].[user] U
JOIN 
	dbo.dbUser R ON R.usrADID = U.NTLogin
--WHERE
--	[NTlogin] = Suser_sname() 
)
UNION ALL
(
SELECT
	G.ID , G.[Name]
FROM
	[item].[User] U
JOIN
	[relationship].[Group_User] GU ON GU.UserID = U.[ID]
JOIN
	[item].[Group] G ON G.[ID] = GU.[GroupID]
GROUP BY G.ID , G.[Name]
--WHERE 
--	U.[NTlogin] = Suser_sname() 
)
UNION ALL
(	
SELECT
	ID , [Name]
FROM
	[item].[Group]
WHERE
	[Name] = 'Everyone'
)



GO
GRANT UPDATE
    ON OBJECT::[config].[GetUserAndGroupMembership] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[GetUserAndGroupMembership] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[GetUserAndGroupMembership] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[GetUserAndGroupMembership] TO [OMSApplicationRole]
    AS [dbo];


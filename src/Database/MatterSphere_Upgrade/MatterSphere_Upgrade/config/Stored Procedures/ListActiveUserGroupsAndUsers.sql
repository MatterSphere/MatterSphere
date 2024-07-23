
CREATE PROCEDURE [config].[ListActiveUserGroupsAndUsers]
	@ui uCodeLookup = '{default}'
	
AS
SET NOCOUNT ON

SELECT 
	CONVERT(NVARCHAR(200), ID) + ';' + Name + ';51' AS ID, COALESCE(CL.cdDesc, '~' + NULLIF([Group].Name, '') + '~') AS Name, 51 AS GroupImageIndex, 'INTERNAL' as AccessType
FROM
	item.[Group]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'SECGROUPS', @ui ) CL ON CL.[cdCode] = [Group].Name
WHERE
    (Active = 1) AND (Name <> 'EVERYONE')
	AND ID != 'DF08633D-9262-489F-B7F1-9A4FC56B41BC'
UNION ALL
SELECT        
	CONVERT(NVARCHAR(200), ID) + ';' + R.usrFullName + ';52' AS ID, R.usrFullName, 52 AS GroupImageIndex, R.AccessType as AccessType
FROM            
	item.[User] U JOIN dbo.dbUser R ON R.usrADID = U.NTLogin
WHERE			
	(Active = 1) AND R.AccessType = 'INTERNAL'



GO
GRANT EXECUTE
    ON OBJECT::[config].[ListActiveUserGroupsAndUsers] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListActiveUserGroupsAndUsers] TO [OMSAdminRole]
    AS [dbo];


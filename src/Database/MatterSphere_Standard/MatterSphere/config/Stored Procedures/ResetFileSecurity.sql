

CREATE PROCEDURE [config].[ResetFileSecurity]
	@securableID bigint
	,@incInternal bit = 0

AS
SET NOCOUNT ON 
--DECLARE @fileID bigint
--SET @fileID = ( SELECT TOP 1 FileID FROM [relationship].[UserGroup_File] WHERE UserGroupID = @userGroup )
DELETE [relationship].[UserGroup_File] 
FROM [relationship].[UserGroup_File] UG
LEFT JOIN [item].[user] IU on IU.ID = UG.UserGroupID
LEFT JOIN [dbo].[dbUser] U on U.usrADID = IU.NTLogin
WHERE UG.FileID = @securableID
AND 
(
	(@incInternal = 0 AND (U.AccessType <> 'EXTERNAL' OR U.AccessType is NULL)) OR (@incInternal = 1) 
)

-- delete document records
DELETE [relationship].[UserGroup_Document] 
FROM [relationship].[UserGroup_Document] UG
LEFT JOIN [item].[user] IU on IU.ID = UG.UserGroupID
LEFT JOIN [dbo].[dbUser] U on U.usrADID = IU.NTLogin
WHERE 
	UG.DocumentID IN (SELECT docID FROM [dbo].[dbDocument] WHERE [fileID] = @securableID)
AND 
(
	(@incInternal = 0 AND (U.AccessType <> 'EXTERNAL' OR U.AccessType is NULL)) OR (@incInternal = 1) 
)



GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetFileSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetFileSecurity] TO [OMSAdminRole]
    AS [dbo];


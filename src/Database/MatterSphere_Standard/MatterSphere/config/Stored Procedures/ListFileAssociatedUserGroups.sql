

CREATE PROCEDURE [config].[ListFileAssociatedUserGroups]
	@fileID bigint ,
	@ui uCodeLookup = '{default}'
	,@AccessType nvarchar(15) ='INTERNAL'
	

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


IF EXISTS 
	(
	SELECT 1
	FROM
		[relationship].[UserGroup_File] UGF
	JOIN
		[item].[Group] G ON G.[ID] = UGF.[UserGroupID]
	WHERE 
		UGF.[FileID] = @fileID
	UNION	ALL
	SELECT 1
	FROM
		[relationship].[UserGroup_File] UGF
	JOIN
		[item].[User] U ON U.[ID] = UGF.[UserGroupID]
	JOIN
		[dbo].[dbUser] dbU on U.[NTLogin] = dbU.[usrADID]			
	WHERE 
		UGF.[FileID] = @fileID
		--AND dbU.AccessType = @AccessType
	)
BEGIN
	SELECT DISTINCT
		51 as [ImageIndex],
		CASE OP.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGF.[PolicyID] END as [Policy] ,
		G.[ID] as [GroupID],
		G.[Name] as [GroupName] ,
		case  UGF.inherited 
		when 'C' then 'Inherited from Client - ' else ''
		 end + COALESCE(CL.cdDesc, '~' + NULLIF(G.[Name], '') + '~') as [GroupNameDesc]
		 ,'INTERNAL' as [AccessType]
		 , null as [SelectedMattersOnly]
		 , UGF.inherited as [Inherited]
		 , G.Active as [Active]
	FROM
		[relationship].[UserGroup_File] UGF
	JOIN
		[item].[Group] G ON G.[ID] = UGF.[UserGroupID]
	JOIN
		[config].[ObjectPolicy] OP ON OP.[ID] = UGF.[PolicyID]
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'SECGROUPS', @ui ) CL ON CL.[cdCode] = G.[Name]
	WHERE 
		UGF.[FileID] = @fileID
	UNION	ALL
	SELECT DISTINCT
		--52 as [ImageIndex],
		CASE R.AccessType WHEN 'EXTERNAL' THEN 94 ELSE 52 END AS [ImageIndex] ,
		CASE OP.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGF.[PolicyID] END as [Policy] ,
		U.[ID] as [GroupID],
		R.[usrFullName] as [GroupName],
		case  UGF.inherited 
		when 'C' then 'Inherited from Client - ' else ''
		 end + R.[usrFullName] as [GroupNameDesc]
		 , R.AccessType
		 , null as [SelectedMattersOnly]
		 , UGF.inherited as [Inherited]
		 , U.Active as [Active]
	FROM
		[relationship].[UserGroup_File] UGF
	JOIN
		[item].[User] U ON U.[ID] = UGF.[UserGroupID]
	JOIN
		[dbo].[dbUser] R ON R.usrADID = U.NTLogin
	JOIN
		[config].[ObjectPolicy] OP ON OP.[ID] = UGF.[PolicyID]
	WHERE 
		UGF.[FileID] = @fileID
		--AND R.AccessType = @AccessType
END
ELSE
SELECT
	51 as [ImageIndex] ,
	( SELECT [ID] FROM [config].[ObjectPolicy] WHERE [Type] = 'GLOBALOBJDEF' ) as [Policy],
	newid() as [GroupID], dbo.GetCodeLookupDesc('SECGROUPS' , 'EVERYONE' , @ui )   as [GroupName], 'Internal Users' as [GroupNameDesc], '' as [AccessType]
	, null as [SelectedMattersOnly]
	, null as [Inherited]
	, null as [Active]

GO
GRANT EXECUTE
    ON OBJECT::[config].[ListFileAssociatedUserGroups] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListFileAssociatedUserGroups] TO [OMSAdminRole]
    AS [dbo];


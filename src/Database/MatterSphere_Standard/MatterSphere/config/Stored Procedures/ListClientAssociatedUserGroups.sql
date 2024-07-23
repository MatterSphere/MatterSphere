
CREATE PROCEDURE [config].[ListClientAssociatedUserGroups]
	@ClID bigint ,
	@ui uCodeLookup = '{default}'
	,@AccessType nvarchar(15) ='INTERNAL'

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF EXISTS 
	(
	SELECT 1
	FROM
		[relationship].[UserGroup_Client] UGC
	JOIN
		[item].[Group] G ON G.[ID] = UGC.[UserGroupID]
	WHERE 
		UGC.[ClientID] = @ClID
	UNION	ALL
	SELECT 1
	FROM
		[relationship].[UserGroup_Client] UGC
	JOIN
		[item].[User] U ON U.[ID] = UGC.[UserGroupID]
	JOIN
		[dbo].[dbUser] dbU on U.[NTLogin] = dbU.[usrADID]		
	WHERE 
		UGC.[ClientID] = @ClID
		--AND dbU.AccessType = @AccessType
	)
BEGIN
	SELECT DISTINCT
		51 as [ImageIndex],
		CASE OP.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGC.[PolicyID] END as [Policy] ,
		G.[ID] as [GroupID],
		G.[Name] as [GroupName] ,
		case  when UGC.block_inheritance = 1 
		 then 'Selected Matters Only - ' else ''
		 end 
		+ COALESCE(CL.cdDesc, '~' + NULLIF(G.[Name], '') + '~') as [GroupNameDesc]
		, 'INTERNAL' as [AccessType]
		, UGC.block_inheritance as [SelectedMattersOnly]
		, null as [Inherited]
		, G.Active as [Active]
	FROM
		[relationship].[UserGroup_Client] UGC
	JOIN
		[item].[Group] G ON G.[ID] = UGC.[UserGroupID]
	JOIN
		[config].[ObjectPolicy] OP ON OP.[ID] = UGC.[PolicyID]
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'SECGROUPS', @ui ) CL ON CL.[cdCode] = G.[Name]
	WHERE 
		UGC.[ClientID] = @ClID
	UNION	ALL
	SELECT DISTINCT
		--52 as [ImageIndex],
		CASE R.AccessType WHEN 'EXTERNAL' THEN 94 ELSE 52 END AS [ImageIndex] ,
		CASE OP.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGC.[PolicyID] END as [Policy] ,
		U.[ID] as [GroupID],
		R.[usrFullName] as [GroupNme],
		case  when UGC.block_inheritance = 1 
		 then 'Selected Matters Only - ' else ''
		 end 
		+ R.[usrFullName] as [GroupNameDesc]
		, R.AccessType as [AccessType]
		, UGC.block_inheritance as [SelectedMattersOnly]
		, null as [Inherited]
		, U.Active as [Active]
	FROM
		[relationship].[UserGroup_Client] UGC
	JOIN
		[item].[User] U ON U.[ID] = UGC.[UserGroupID]
	JOIN
		[dbo].[dbUser] R ON R.usrADID = U.NTLogin
	JOIN
		[config].[ObjectPolicy] OP ON OP.[ID] = UGC.[PolicyID]
	WHERE 
		UGC.[ClientID] = @ClID
		--AND R.AccessType = @AccessType
END
ELSE
SELECT
	51 as [ImageIndex] ,
	( SELECT [ID] FROM [config].[ObjectPolicy] WHERE [Type] = 'SYSDEFAULT' ) as [Policy],
	newid() as [GroupID], dbo.GetCodeLookupDesc('SECGROUPS ', 'EVERYONE' , @ui )   as [GroupName], 'Internal Users' as [GroupNameDesc], '' as [AccessType]
	, null as [SelectedMattersOnly]
	, null as [Inherited]
	, null as [Active]

GO
GRANT EXECUTE
    ON OBJECT::[config].[ListClientAssociatedUserGroups] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListClientAssociatedUserGroups] TO [OMSAdminRole]
    AS [dbo];


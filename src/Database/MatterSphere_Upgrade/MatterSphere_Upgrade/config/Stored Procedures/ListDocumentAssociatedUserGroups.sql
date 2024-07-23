

CREATE PROCEDURE [config].[ListDocumentAssociatedUserGroups] 
	@documentID bigint ,
	@ui uCodeLookup = '{default}'
	,@AccessType nvarchar(15) ='INTERNAL'
	
AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF EXISTS 
	(
	SELECT 1
	FROM
		[relationship].[UserGroup_Document] UGD
	JOIN
		[item].[Group] G ON G.[ID] = UGD.[UserGroupID]
	WHERE 
		UGD.[DocumentID] = @documentID
	UNION	ALL
	SELECT 1
	FROM
		[relationship].[UserGroup_Document] UGD
	JOIN
		[item].[User] U ON U.[ID] = UGD.[UserGroupID]
	JOIN
		[dbo].[dbUser] dbU on U.[NTLogin] = dbU.[usrADID]			
	WHERE 
		UGD.[DocumentID] = @documentID
		--AND dbU.AccessType = @AccessType
	)
BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[dbRegInfo] WHERE [regBlockInheritence] = 0)
	BEGIN
		SELECT DISTINCT
			51 as [ImageIndex],
			CASE OP.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGD.[PolicyID] END as [Policy] ,
			G.[ID] as [GroupID],
			G.[name] as [GroupName] ,
			case  UGD.inherited 
			when 'F' then 'Inherited from Matter - '
			when 'C' then 'Inherited from Client - ' else ''
			 end + COALESCE(CL.cdDesc, '~' + NULLIF(G.[Name], '') + '~') as [GroupNameDesc]
			 ,'INTERNAL' as [AccessType]
			 , null as [SelectedMattersOnly]
			 , UGD.inherited as [Inherited]
			 , G.Active as [Active]
		FROM
			[relationship].[UserGroup_Document] UGD
		JOIN
			[item].[Group] G ON G.[ID] = UGD.[UserGroupID]
		JOIN
			[config].[ObjectPolicy] OP ON OP.[ID] = UGD.[PolicyID]
		LEFT JOIN
			[dbo].[GetCodeLookupDescription] ( 'SECGROUPS', @ui ) CL ON CL.[cdCode] = G.[Name]
		WHERE 
			UGD.[DocumentID] = @documentID
		UNION	ALL
		SELECT DISTINCT
			--52 as [ImageIndex],
			CASE R.AccessType WHEN 'EXTERNAL' THEN 94 ELSE 52 END AS [ImageIndex] ,
			CASE OP.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGD.[PolicyID] END as [Policy] ,
			U.[ID] as [GroupID],
			R.[usrFullName] as [GroupName],
			case  UGD.inherited 
			when 'F' then 'Inherited from Matter - '
			when 'C' then 'Inherited from Client - ' else ''
			 end  +R.[usrFullName] as [GroupNameDesc]
			 ,R.AccessType as [AccessType]
			 , null as [SelectedMattersOnly]
			 , UGD.inherited as [Inherited]
			 , U.Active as [Active]
		FROM
			[relationship].[UserGroup_Document] UGD
		JOIN
			[item].[User] U ON U.[ID] = UGD.[UserGroupID]
		JOIN
			[dbo].[dbUser] R ON R.usrADID = U.NTLogin
		JOIN
			[config].[ObjectPolicy] OP ON OP.[ID] = UGD.[PolicyID]
		WHERE 
			UGD.[DocumentID] = @documentID
		--AND R.AccessType = @AccessType
	END
	ELSE
	BEGIN
		SELECT DISTINCT
			51 as [ImageIndex],
			CASE OP.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGD.[PolicyID] END as [Policy] ,
			G.[ID] as [GroupID],
			G.[name] as [GroupName] ,
			COALESCE(CL.cdDesc, '~' + NULLIF(G.[Name], '') + '~') as [GroupNameDesc]
			,'INTERNAL' as [AccessType]
			, null as [SelectedMattersOnly]
			, null as [Inherited]
			, G.Active as [Active]
		FROM
			[relationship].[UserGroup_Document] UGD
		JOIN
			[item].[Group] G ON G.[ID] = UGD.[UserGroupID]
		JOIN
			[config].[ObjectPolicy] OP ON OP.[ID] = UGD.[PolicyID]
		LEFT JOIN
			[dbo].[GetCodeLookupDescription] ( 'SECGROUPS', @ui ) CL ON CL.[cdCode] = G.[Name]
		WHERE 
			UGD.[DocumentID] = @documentID
		AND UGD.inherited is null
		UNION	ALL
		SELECT DISTINCT
			--52 as [ImageIndex],
			CASE R.AccessType WHEN 'EXTERNAL' THEN 49 ELSE 52 END AS [ImageIndex] ,
			CASE OP.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGD.[PolicyID] END as [Policy] ,
			U.[ID] as [GroupID],
			R.[usrFullName] as [GroupName],
			R.[usrFullName] as [GroupNameDesc]
			, R.AccessType as [AccessType]
			, null as [SelectedMattersOnly]
			, UGD.inherited as [Inherited]
			, U.Active as [Active]
		FROM
			[relationship].[UserGroup_Document] UGD
		JOIN
			[item].[User] U ON U.[ID] = UGD.[UserGroupID]
		JOIN
			[dbo].[dbUser] R ON R.usrADID = U.NTLogin
		JOIN
			[config].[ObjectPolicy] OP ON OP.[ID] = UGD.[PolicyID]
		WHERE 
			UGD.[DocumentID] = @documentID
		AND UGD.inherited is null
		UNION	
		SELECT
		59 as [ImageIndex] ,
		( SELECT [ID] FROM [config].[ObjectPolicy] WHERE [Type] = 'GLOBALOBJDEF' ) as [Policy],
		newid() as [GroupID], 
		dbo.GetCodeLookupDesc('SECGROUPS' , 'EVERYONE' , @ui )  as [GroupName], 
		'Implied Security from Matter' as [GroupNameDesc]
		, '' as [AccessType]
		, null as [SelectedMattersOnly]
		, null as [Inherited]
		, null as [Active]
		WHERE (SELECT  top 1 [regBlockInheritence] FROM [dbo].[dbRegInfo]) = 1 AND EXISTS
						(select 1 from relationship.UserGroup_File f 
								join config.dbDocument d on d.fileID = f.FileID 
								where d.docID =  @documentID) 
	END
END
ELSE IF (SELECT  top 1 [regBlockInheritence] FROM [dbo].[dbRegInfo]) = 1 AND EXISTS
	(select 1 from relationship.UserGroup_File f 
	join config.dbDocument d on d.fileID = f.FileID 
	where d.docID =  @documentID)
	begin
	SELECT
		59 as [ImageIndex] ,
		( SELECT [ID] FROM [config].[ObjectPolicy] WHERE [Type] = 'GLOBALOBJDEF' ) as [Policy],
		newid() as [GroupID], dbo.GetCodeLookupDesc('SECGROUPS' , 'EVERYONE' , @ui )  as [GroupName], 'Implied Security from Matter' as [GroupNameDesc], '' as [AccessType]
		, null as [SelectedMattersOnly]
		, null as [Inherited]
		, null as [Active]
	end
ELSE
SELECT
	51 as [ImageIndex] ,
	( SELECT [ID] FROM [config].[ObjectPolicy] WHERE [Type] = 'GLOBALOBJDEF' ) as [Policy],
	newid() as [GroupID], dbo.GetCodeLookupDesc('SECGROUPS' , 'EVERYONE' , @ui )  as [GroupName], 'Internal Users' as [GroupNameDesc], '' as [AccessType]
	, null as [SelectedMattersOnly]
	, null as [Inherited]
	, null as [Active]

GO
GRANT EXECUTE
    ON OBJECT::[config].[ListDocumentAssociatedUserGroups] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListDocumentAssociatedUserGroups] TO [OMSAdminRole]
    AS [dbo];


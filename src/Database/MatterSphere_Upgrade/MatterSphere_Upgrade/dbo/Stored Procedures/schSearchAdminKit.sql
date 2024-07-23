

CREATE PROCEDURE [dbo].[schSearchAdminKit] 
(
	@UI nvarchar(15) = '{default}'
	, @DESC nvarchar(50) = null
	, @TYPE nvarchar(15) = null
)

AS
BEGIN
WITH  MenuH ([admnuParent], TadmnuID, [admnuID], [admnuCode], NodeFunctionCode, level, TLabel, NodeDescription,[admnuSearchListCode],Roles) AS
(
    SELECT
		[admnuParent]
		, [admnuID] as TadmnuID
		, [admnuID], [admnuCode]
		, [admnuSearchListCode] as NodeFunctionCode
		, 0
		, dbo.ReplaceEnvironmentalVariableWithText(CAST(COALESCE(CL.cdDesc, '~' + NULLIF([dbAdminMenu].[admnuCode], '') + '~') as VARCHAR(MAX))) as TLabel 
		, dbo.ReplaceEnvironmentalVariableWithText(CAST(COALESCE(CL.cdDesc, '~' + NULLIF([dbAdminMenu].[admnuCode], '') + '~') as VARCHAR(MAX))) as NodeDescription
		,[admnuSearchListCode]
		,[admnuRoles] as Roles
    FROM [dbAdminMenu]
		LEFT JOIN [dbo].[GetCodeLookupDescription] ( 'AdminMenu', @UI) CL ON CL.[cdCode] = [dbAdminMenu].[admnuCode]
    WHERE [admnuParent] = 1 and
		  [admnuID] <> 1
    UNION ALL
    SELECT 
		m.[admnuParent]
		,TadmnuID, 
		m.[admnuID]
		, m.[admnuCode]
		, m.admnuSearchListCode as NodeFunctionCode
		, level + 1
		,h.TLabel
		, dbo.ReplaceEnvironmentalVariableWithText(CAST([dbo].[GetCodeLookupDesc] ('AdminMenu',m.[admnuCode],@UI) as VARCHAR(MAX))) as NodeDescription
		,m.[admnuSearchListCode]
		,m.admnuRoles as Roles
    FROM
		[dbAdminMenu] m JOIN MenuH h ON m.[admnuParent] = h.[admnuID]
    WHERE
		m.[admnuID] <> 1
)


SELECT 
	NodeDescription
	, NodeFunctionCode
	, mh.[admnuID] As NodeID
	, TLabel As ContainerDescription
	, am.admnuSearchListCode as ContainerCode
	, TadmnuID As ContainerID
	, mh.Roles
FROM
	MenuH mh join dbAdminMenu am on mh.TadmnuID = am.admnuID
WHERE
	NodeDescription like '%' + @DESC + '%' AND
	am.admnuName = @TYPE
END



SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchAdminKit] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchAdminKit] TO [OMSAdminRole]
    AS [dbo];


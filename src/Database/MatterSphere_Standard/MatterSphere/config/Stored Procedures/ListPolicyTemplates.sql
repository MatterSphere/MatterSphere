

CREATE PROCEDURE [config].[ListPolicyTemplates]
	@UI [dbo].uUICultureInfo = '{default}'

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


SELECT 	P.[ID], 
		COALESCE(CL.cdDesc, '~' + NULLIF(P.[Type], '') + '~') AS [type],
		P.[Name] ,
		P.[Type] 
FROM (
	SELECT [ID], 
		[Name],
		[Type] 
	FROM [config].[ObjectPolicy]
	WHERE [Type] <> 'EXPLICITOBJ'
	UNION ALL
	SELECT [ID], 
		[Name] ,
		[Type]
	FROM [config].[SystemPolicy]
	WHERE [Type] <> 'EXPLICITSYS'
	) P
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'POLICY', @UI ) CL ON CL.[cdCode] = P.[Type]
ORDER BY P.[Name];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListPolicyTemplates] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListPolicyTemplates] TO [OMSAdminRole]
    AS [dbo];


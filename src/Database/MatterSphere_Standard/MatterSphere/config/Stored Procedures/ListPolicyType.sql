

CREATE PROCEDURE [config].[ListPolicyType]
	@uI uUICultureInfo ,
	@isSystem bit,
	@includeexp bit = 0


AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
IF (@includeexp = 1)
BEGIN
	SELECT
		CASE @isSystem WHEN 0 THEN 'EXPLICITOBJ' ELSE 'EXPLICITSYS' END as [PolicyTypeCode] ,
		CASE @isSystem WHEN 0 THEN [dbo].[GetCodeLookupDesc] ( 'POLICY' , 'EXPLICITOBJ' , @uI )
		 ELSE [dbo].[GetCodeLookupDesc] ( 'POLICY' , 'EXPLICITSYS' , @uI ) END as [PolicyDescription] ,
		NULL as [PolicyID] , 0 as IsRemote
		, 0 as IsDefault
		, NULL as [IsDefaultImage]
	UNION ALL	
	SELECT 
		[PolicyTypeCode] , 
		COALESCE(CL.cdDesc, '~' +  NULLIF(PT.[PolicyTypeCode], '') + '~') as [PolicyDescription] ,
		CASE @isSystem WHEN 1 THEN SP.[ID] ELSE OP.[ID] END as [PolicyID], OP.[IsRemote] as IsRemote
		, SP.[isDefault] as IsDefault
		, CASE ISNULL(SP.[IsDefault], 0) WHEN 0 THEN NULL ELSE 66 END as [IsDefaultImage]
	FROM
		[config].[PolicyType] PT
	LEFT JOIN
		[config].[ObjectPolicy] OP ON OP.[Type] = PT.[PolicyTypeCode]
	LEFT JOIN
		[config].[SystemPolicy] SP ON SP.[Type] = PT.[PolicyTypeCode]
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'POLICY', @UI ) CL ON CL.[cdCode] = PT.[PolicyTypeCode]
	WHERE
		[IncludeInFilters]  = 1
	AND
		CASE @isSystem
			WHEN 1 THEN 1 ELSE 0 END = IsSystemPolicy
	AND
		(OP.[ID] IS NOT NULL OR SP.[ID] IS NOT NULL)
	AND
		[PolicyTypeCode] NOT IN ( 'EXPLICITSYS' , 'EXPLICITOBJ' )

END
ELSE
BEGIN
	SELECT
		[PolicyTypeCode] , 
		COALESCE(CL.cdDesc, '~' + NULLIF(PT.[PolicyTypeCode], '') + '~') as [PolicyDescription] ,
		CASE @isSystem WHEN 1 THEN SP.[ID] ELSE OP.[ID] END as [PolicyID], OP.[IsRemote] as IsRemote
		, SP.[isDefault] as IsDefault
		, CASE ISNULL(SP.[IsDefault], 0) WHEN 0 THEN NULL ELSE 66 END as [IsDefaultImage]
	FROM
		[config].[PolicyType] PT
	LEFT JOIN
		[config].[ObjectPolicy] OP ON OP.[Type] = PT.[PolicyTypeCode]
	LEFT JOIN
		[config].[SystemPolicy] SP ON SP.[Type] = PT.[PolicyTypeCode]
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'POLICY', @UI ) CL ON CL.[cdCode] = PT.[PolicyTypeCode]
	WHERE
		[IncludeInFilters]  = 1
	AND
		CASE @isSystem
			WHEN 1 THEN 1 ELSE 0 END = IsSystemPolicy
	AND
		(OP.[ID] IS NOT NULL OR SP.[ID] IS NOT NULL)
	AND
		[PolicyTypeCode] NOT IN ( 'EXPLICITSYS' , 'EXPLICITOBJ' )
END


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListPolicyType] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListPolicyType] TO [OMSAdminRole]
    AS [dbo];


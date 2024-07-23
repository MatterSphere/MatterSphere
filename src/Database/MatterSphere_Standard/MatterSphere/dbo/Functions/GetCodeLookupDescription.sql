

CREATE FUNCTION [dbo].[GetCodeLookupDescription] ( @type uCodeLookup , @ui uUICultureInfo )
RETURNS Table
RETURN
(
	WITH c AS(
	SELECT cdCode 
		, MAX(CASE cdUICultureInfo WHEN @ui THEN [cdDesc] END) AS base
		, MAX(CASE WHEN cdUICultureInfo = '{default}' THEN [cdDesc] END) AS [default]
	FROM [dbo].[dbCodeLookup] 
	WHERE cdType = @type AND cdUICultureInfo IN (@ui, '{default}')
	GROUP BY cdCode
	)
	SELECT @type as cdType
		, [cdCode] 
		, CASE WHEN base IS NOT NULL THEN base WHEN base IS NULL AND [default] IS NOT NULL THEN [default] ELSE '~' +[cdCode] + '~' END as cdDesc
	FROM c
)


GO
GRANT UPDATE
    ON OBJECT::[dbo].[GetCodeLookupDescription] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GetCodeLookupDescription] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[GetCodeLookupDescription] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[GetCodeLookupDescription] TO [OMSApplicationRole]
    AS [dbo];


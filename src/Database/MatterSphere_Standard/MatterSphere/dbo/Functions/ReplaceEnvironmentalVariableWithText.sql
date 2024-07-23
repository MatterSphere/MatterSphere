


CREATE FUNCTION [dbo].[ReplaceEnvironmentalVariableWithText] (@originaltext nvarchar(max))
RETURNS nvarchar (max)
AS
BEGIN
	-- Declare the return variable here
	declare @result as nvarchar (max)
	set @result = replace(@originaltext, '%CLIENT%', dbo.GetCodeLookupDesc('EN','%CLIENT%',''))
	set @result = replace(@result, '%FILE%', dbo.GetCodeLookupDesc('EN','%FILE%',''))
	set @result = replace(@result, '%ASSOCIATE%', dbo.GetCodeLookupDesc('EN','%ASSOCIATE%',''))
	set @result = replace(@result, '%PRECEDENT%', dbo.GetCodeLookupDesc('EN','%PRECEDENT%',''))
	set @result = replace(@result, '%FEEEARNER%', dbo.GetCodeLookupDesc('EN','%FEEEARNER%',''))
	set @result = replace(@result, '%RESPONSIBLE%', dbo.GetCodeLookupDesc('EN','%RESPONSIBLE%',''))
	set @result = replace(@result, '%APPNAME%', dbo.GetCodeLookupDesc('EN','%APPNAME%',''))
	set @result = replace(@result, '%DEPT%', dbo.GetCodeLookupDesc('EN','%DEPT%',''))
	set @result = replace(@result, '%CLIENTS%', dbo.GetCodeLookupDesc('EN','%CLIENTS%',''))
	set @result = replace(@result, '%FILES%', dbo.GetCodeLookupDesc('EN','%FILES%',''))
	set @result = replace(@result, '%ASSOCIATES%', dbo.GetCodeLookupDesc('EN','%ASSOCIATES%',''))
	set @result = replace(@result, '%PRECEDENTS%', dbo.GetCodeLookupDesc('EN','%PRECEDENTS%',''))
	set @result = replace(@result, '%FEEEARNER%S', dbo.GetCodeLookupDesc('EN','%FEEEARNERS%',''))
	set @result = replace(@result, '%RESPONSIBLES%', dbo.GetCodeLookupDesc('EN','%RESPONSIBLES%',''))
	set @result = replace(@result, '%APPNAMES%', dbo.GetCodeLookupDesc('EN','%APPNAMES%',''))
	set @result = replace(@result, '%DEPTS%', dbo.GetCodeLookupDesc('EN','%DEPTS%',''))
	return @result
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReplaceEnvironmentalVariableWithText] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReplaceEnvironmentalVariableWithText] TO [OMSAdminRole]
    AS [dbo];


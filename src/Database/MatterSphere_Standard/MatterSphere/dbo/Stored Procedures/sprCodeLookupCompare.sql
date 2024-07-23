

CREATE PROCEDURE [dbo].[sprCodeLookupCompare]  (@Type uCodeLookup = null, @UI_S uUICultureInfo = '{default}', @UI_D uUICultureInfo = '{default}' ) 
AS
SELECT dbCodeLookup.cdCode AS Code, dbCodeLookup.cdAddLink AS AddLink, dbo.GetCodeLookupDesc(@Type, dbCodeLookup.cdCode, 
					  @UI_S) AS S_Desc, dbo.GetCodeLookupCulture(@Type, dbCodeLookup.cdCode, @UI_S) AS S_Cult, dbo.GetCodeLookupHelpNoResult(@Type, dbCodeLookup.cdCode, @UI_S) AS S_Help, dbo.GetCodeLookupDescNoResult(@Type, 
					  dbCodeLookup.cdCode, @UI_D) AS D_Desc, dbo.GetCodeLookupCultureNoResult(@Type, dbCodeLookup.cdCode, @UI_D) AS D_Cult, dbo.GetCodeLookupHelpNoResult(@Type, dbCodeLookup.cdCode, @UI_D) AS D_Help, cdGroup, cdAddLink
FROM         dbCodeLookup 
WHERE     (dbCodeLookup.cdType = @Type)
GROUP BY dbCodeLookup.cdCode, dbCodeLookup.cdAddLink, dbCodeLookup.cdGroup 
ORDER BY dbCodeLookup.cdCode


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupCompare] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupCompare] TO [OMSAdminRole]
    AS [dbo];


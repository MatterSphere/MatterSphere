

CREATE PROCEDURE [dbo].[sprGetCultures] (@SupportedOnly bit = 1, @IncludeDefault bit = 1, @CodeLookupDefault bit = 1, @Type uCodeLookup ='', @Code uCodeLookup = '') AS
declare @bound uUICultureInfo
declare @display nvarchar(100)
if @CodeLookupDefault = 1
begin
	set @bound = '{default}'
	set @display = '{default}'
end
else
begin
	set @bound = null
	set @display = ''
end
if @Type <> '' and @Code <> ''
	SELECT     dbCodeLookup.cdUICultureInfo AS langcode, dbLanguage.langDesc + N' - ' + dbCodeLookup.cdUICultureInfo AS langdesc
	FROM         dbCodeLookup LEFT OUTER JOIN
	                      dbLanguage ON dbCodeLookup.cdUICultureInfo = dbLanguage.langCode
	WHERE     (dbCodeLookup.cdType = @Type) AND (dbCodeLookup.cdCode = @Code) 
else if @SupportedOnly = 1 and @IncludeDefault = 1
	select @bound as langcode, @display langdesc union select langcode, langdesc from dblanguage where langsupported = 1 order by langcode
else if @SupportedOnly = 1 and @IncludeDefault = 0
	select langcode, langdesc from dblanguage  where langsupported = 1 order by langcode
else if @SupportedOnly = 0 and @IncludeDefault = 1
	select @bound as langcode, @display langdesc union select langcode, langdesc from dblanguage order by langcode
else if @SupportedOnly = 0 and @IncludeDefault = 0
	select langcode, langdesc from dblanguage order by langcode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetCultures] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetCultures] TO [OMSAdminRole]
    AS [dbo];


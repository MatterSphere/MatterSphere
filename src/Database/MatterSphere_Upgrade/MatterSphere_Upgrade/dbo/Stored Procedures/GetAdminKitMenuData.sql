

CREATE PROCEDURE [dbo].[GetAdminKitMenuData]    
(
	@UI nvarchar(15)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
select distinct
	dbo.ReplaceEnvironmentalVariableWithText(CAST(COALESCE(CL1.cdDesc, '~' + NULLIF(am.[admnuCode], '') + '~') as VARCHAR(MAX))) as [cddesc]
    , am.* from dbAdminMenu am
    inner join dbcodelookup cl
    on cl.cdcode = am.admnuCode
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'AdminMenu', @ui ) CL1 ON CL1.[cdCode] = am.[admnuCode]
    Order by [cddesc] asc
END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdminKitMenuData] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdminKitMenuData] TO [OMSAdminRole]
    AS [dbo];


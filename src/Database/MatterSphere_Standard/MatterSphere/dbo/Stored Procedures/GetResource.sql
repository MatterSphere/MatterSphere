

CREATE PROCEDURE [dbo].[GetResource](
	@Type nvarchar(15),
	@Code nvarchar(15)
	)
AS
	SELECT cdCode as Code, cdDesc as Description FROM dbCodeLookup WHERE cdType = @Type AND cdCode = @Code AND cdUICultureInfo = '{default}'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetResource] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetResource] TO [OMSAdminRole]
    AS [dbo];


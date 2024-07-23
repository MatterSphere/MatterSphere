CREATE PROCEDURE dbo.sprUpdateUserSearchListColumns
	@Code uCodeLookup
	, @listview dbo.uXML
AS
SET NOCOUNT ON

DECLARE @USER NVARCHAR(200) = config.GetUserLogin()

IF EXISTS(SELECT 1 FROM dbo.dbUserSearchListColumns WHERE NTLogin = @USER AND schcode = @Code)
	IF ISNULL(@listview, '') = ''
		DELETE dbo.dbUserSearchListColumns WHERE NTLogin = @USER AND schcode = @Code
	ELSE 
		UPDATE dbo.dbUserSearchListColumns SET schListView = @listview WHERE NTLogin = @USER AND schcode = @Code

ELSE
	IF ISNULL(@listview, '') <> ''
		INSERT INTO dbo.dbUserSearchListColumns(NTLogin, schcode, schListView)
		VALUES(@USER, @Code, @listview)

	
GO
GRANT EXECUTE
    ON OBJECT::dbo.sprUpdateUserSearchListColumns TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::dbo.sprUpdateUserSearchListColumns TO [OMSAdminRole]
    AS [dbo];




CREATE PROCEDURE [dbo].[fdCheckFileSecurity]
(
	@FileId AS BIGINT
)
AS
	SET NOCOUNT ON;
	DECLARE @c INT;
	SET @c = (SELECT COUNT(*) FROM sys.tables t WHERE t.name = 'UserGroup_File' and t.schema_id = SCHEMA_ID('relationship'));
	IF(@c > 0)
	BEGIN
		SELECT * FROM relationship.UserGroup_Document WHERE fileid = @FileId
	END
	ELSE
	BEGIN
		SELECT * FROM sys.tables t WHERE t.name = 'UserGroup_File' and t.schema_id = SCHEMA_ID('relationship')
	END

	GO
	GRANT EXECUTE
		ON OBJECT::[dbo].[fdCheckFileSecurity] TO [OMSRole]
		AS [dbo];

	GO
	GRANT EXECUTE
		ON OBJECT::[dbo].[fdCheckFileSecurity] TO [OMSAdminRole]
		AS [dbo];


CREATE PROCEDURE [dbo].[SetUserContext] (
	@USERID nvarchar(128))

AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE  @context varbinary(128)

	SET @context = (SELECT Convert(varbinary(128) , @USERID))

	SET CONTEXT_INFO @context

END



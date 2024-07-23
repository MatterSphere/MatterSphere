CREATE PROCEDURE search.SendESIndexMessage
  @text XML
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @handle UNIQUEIDENTIFIER;

  BEGIN TRANSACTION
    BEGIN DIALOG @handle
      FROM SERVICE [//msindex/MessageService]
      TO SERVICE '//msindex/MessageService'
      ON CONTRACT [//msindex/MessageContract]
      WITH ENCRYPTION = OFF;

    SEND ON CONVERSATION @handle MESSAGE TYPE [//msindex/Message] ( @text );
	END CONVERSATION @handle WITH CLEANUP;

  COMMIT TRANSACTION
END

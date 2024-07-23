CREATE PROCEDURE search.SendESIndexMessage
  @text XML
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @handle UNIQUEIDENTIFIER;

  BEGIN TRANSACTION
    BEGIN DIALOG @handle
      FROM SERVICE [//esindex/MessageService]
      TO SERVICE '//esindex/MessageService'
      ON CONTRACT [//esindex/MessageContract]
      WITH ENCRYPTION = OFF;

    SEND ON CONVERSATION @handle MESSAGE TYPE [//esindex/Message] ( @text );
	END CONVERSATION @handle WITH CLEANUP;

  COMMIT TRANSACTION
END

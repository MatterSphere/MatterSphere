CREATE PROCEDURE dbo.DeleteDocWithEmptyPathById 
	@DocId BIGINT
AS
DECLARE @as BIT = 0
DECLARE @SubId BIGINT
DECLARE @SubDocs TABLE(docID BIGINT NOT NULL)

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbDocument')
	SET @as = 1

IF EXISTS(SELECT * FROM dbDocument WHERE docID = @DocId AND ISNULL(docFileName, '') = '')
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DELETE dbo.dbDocumentLog WHERE docID = @DocId
			DELETE dbo.dbDocumentVersion WHERE docID = @DocId
			DELETE dbo.dbDocumentEmail WHERE docID = @DocId
			DELETE dbo.dbTimeLedger WHERE docID = @DocId
			IF @as =1 
			BEGIN
				INSERT INTO @SubDocs SELECT docID FROM config.dbDocument WHERE docParent = @DocId
				WHILE (1=1)
				BEGIN
					SET @SubId = (SELECT TOP 1 docID FROM @SubDocs)
					IF @SubId IS NULL BREAK
					EXEC dbo.DeleteDocWithEmptyPathById @SubId
					DELETE FROM @SubDocs WHERE docID = @SubId
				END
				DELETE config.dbDocumentPreview WHERE docID = @DocId
				DELETE config.dbDocument WHERE docID = @DocId
			END
			ELSE
			BEGIN
				INSERT INTO @SubDocs SELECT docID FROM dbo.dbDocument WHERE docParent = @DocId
				WHILE (1=1)
				BEGIN
					SET @SubId = (SELECT TOP 1 docID FROM @SubDocs)
					IF @SubId IS NULL BREAK
					EXEC dbo.DeleteDocWithEmptyPathById @SubId
					DELETE FROM @SubDocs WHERE docID = @SubId
				END
				DELETE dbo.dbDocumentPreview WHERE docID = @DocId
				DELETE dbo.dbDocument WHERE docID = @DocId
			END
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT <> 0
			ROLLBACK TRANSACTION
		DECLARE @er NVARCHAR(MAX)
		SET @er = ERROR_MESSAGE()
		RAISERROR ( @er , 16 ,1 )
	END CATCH
END

GO
GRANT EXECUTE
    ON OBJECT::dbo.DeleteDocWithEmptyPathById TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::dbo.DeleteDocWithEmptyPathById TO [OMSAdminRole]
    AS [dbo];

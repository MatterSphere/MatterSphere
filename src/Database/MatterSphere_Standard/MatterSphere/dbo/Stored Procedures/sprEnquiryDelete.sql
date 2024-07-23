

CREATE PROCEDURE [dbo].[sprEnquiryDelete]
(@Code uCodeLookup, @Overwrite bit) 
AS
DECLARE @enqID int
DECLARE @enqSys bit
SELECT @enqID = enqID, @enqSys = enqSystem FROM dbEnquiry WHERE enqCode = @Code
if (@enqSys = 0)
BEGIN
	IF (not @enqID is null)
	BEGIN
		BEGIN TRANSACTION
		DELETE FROM dbEnquiryPage WHERE enqID = @enqID
		if @@ERROR <> 0
		begin
			ROLLBACK TRANSACTION
			return
		end
		DELETE FROM dbEnquiryQuestion WHERE enqID = @enqID
		if @@ERROR <> 0
		begin
			ROLLBACK TRANSACTION
			return
		end
		DELETE FROM dbEnquiryMethod WHERE enqID = @enqID
		if @@ERROR <> 0
		begin
			ROLLBACK TRANSACTION
			return
		end
		DELETE FROM dbEnquiryDataSource WHERE enqID = @enqID
		if @@ERROR <> 0
		begin
			ROLLBACK TRANSACTION
			return
		end
		DELETE FROM dbEnquiry WHERE enqID = @enqID
		if @@ERROR <> 0
		begin
			ROLLBACK TRANSACTION
			return
		end
		if (@Overwrite = 0)
		BEGIN
			DELETE FROM dbCodeLookup WHERE cdCode = @Code AND cdType = 'ENQHEADER'
			if @@ERROR <> 0
			begin
				ROLLBACK TRANSACTION
				return
			end
		END	
		COMMIT TRANSACTION
	END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprEnquiryDelete] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprEnquiryDelete] TO [OMSAdminRole]
    AS [dbo];


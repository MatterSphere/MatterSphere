-- =============================================
-- Author:		Mosmar - Pratik Mehta
-- Create date: 25.05.2017
-- Description:	This SP manages the address change requests from the Client Portal
-- =============================================
CREATE PROCEDURE dbo.sprManageAddressChangeReq
	@changeGUID uniqueidentifier = null,
	@userID int,
	@approve bit = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @changeGUID IS NULL OR NOT EXISTS (SELECT * FROM dbo.dbContactNewAddressRequest where rowguid = @changeGUID)
	BEGIN
		SELECT 'FAILED' AS col1, 'Invalid address change request' AS col2
		RETURN
	END

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @MakeDefault bit
		DECLARE @ContactID bigint
		DECLARE @ContactAddID bigint
		DECLARE @AddressTypeCode nvarchar(15)

		SELECT   @MakeDefault = contMakeDefault
				,@ContactID = contID
				,@ContactAddID = contaddID
				,@AddressTypeCode = contCode
		FROM dbo.dbContactNewAddressRequest WHERE rowguid = @changeGUID

		IF @approve = 1
		BEGIN
			DECLARE @AddressID bigint
			DECLARE @ContAddOrder tinyint

			-- Create Address
			INSERT INTO dbAddress (addLine1, addLine2, addLine3, addLine4, addLine5, addPostcode, addCountry, Created, CreatedBy, Updated, UpdatedBy)
			SELECT addLine1, addLine2, addLine3, addLine4, addLine5, addPostcode, addCountry, GETUTCDATE(), @userID, GETUTCDATE(), @userID
			FROM dbo.dbNewAddressRequest WHERE addID = @ContactAddID
			SET @AddressID = SCOPE_IDENTITY()

			--Check if it is the default contact address
			IF @MakeDefault = 1
			BEGIN
				UPDATE config.dbContact SET contDefaultAddress = @AddressID WHERE contID = @ContactID
			END

			SET @ContAddOrder = 0

			IF EXISTS (SELECT * FROM dbo.dbContactAddresses WHERE contID = @ContactID)
			BEGIN
				SELECT @ContAddOrder = MAX(contOrder) FROM dbo.dbContactAddresses WHERE contID = @ContactID
			END

			--Link the address with the contact
			INSERT INTO dbo.dbContactAddresses (contaddID, contID, contActive, contOrder, contCode)
			VALUES (@AddressID, @ContactID, 1, @ContAddOrder, @AddressTypeCode);

			--Create audit record if needed
			--NOT IMPLEMENTED

			--Remove the original entries from the Client Portal
			DELETE FROM dbo.dbContactNewAddressRequest where rowguid = @changeGUID
			DELETE FROM dbo.dbNewAddressRequest where addID = @ContactAddID
		END
		ELSE
		BEGIN
			--Create audit record if needed
			--NOT IMPLEMENTED

			--Remove the original entries from the Client Portal
			DELETE FROM dbo.dbContactNewAddressRequest where rowguid = @changeGUID
			DELETE FROM dbo.dbNewAddressRequest where addID = @ContactAddID
		END

		SELECT 'SUCCESS' AS col1, 'Request processed successfully' AS col2

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		SELECT ERROR_LINE() AS col1, ERROR_MESSAGE() AS col2
		ROLLBACK
	END CATCH
END
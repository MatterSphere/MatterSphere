

CREATE PROCEDURE [config].[CreatePolicyType]
	@policyTypeCode uCodeLookup ,
	@canEdit bit ,
	@canDelete bit ,
	@CreatedBy int ,
	@isSystemPolicy bit ,
	@includeInFilters bit

AS
SET NOCOUNT ON
SET XACT_ABORT ON
DECLARE @getdate datetime
SET @getdate = GetUTCDate()

BEGIN TRY
BEGIN TRANSACTION
	-- Create the Policy Record
	INSERT [config].[PolicyType] ( [PolicyTypeCode] , [CanEdit] , [CanDelete] , [Created] , [CreatedBy] , [IsSystemPolicy] , [IncludeInFilters]  )
	VALUES ( @policyTypeCode , @canEdit , @canDelete , @getdate , @createdBy , @isSystemPolicy , @includeInFilters )
	-- Audit the event
	INSERT [audit].[PolicyType] ( [AuditEvent] , [PolicyTypeCode] , [CanEdit] , [CanDelete] , [Updated] , [UpdatedBy] , [IsSystemPolicy] , [IncludeInFilters] )
	VALUES ( 'Security Policy Created' , @policyTypeCode , @canEdit , @canDelete , @getdate , @createdBy , @isSystemPolicy , @includeInFilters )
COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@Trancount <> 0
		ROLLBACK TRANSACTION
	DECLARE @err nvarchar(max)
	SET @err = ERROR_MESSAGE()
	RAISERROR ( @err , 16 , 1 )
END CATCH



GO
GRANT EXECUTE
    ON OBJECT::[config].[CreatePolicyType] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[CreatePolicyType] TO [OMSAdminRole]
    AS [dbo];


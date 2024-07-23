


CREATE PROCEDURE [dbo].[wfNewToken]
	@userId AS INT,
	@workflowToken AS NVARCHAR(300),
	@workflowCode AS NVARCHAR(200),
	@workflowURI AS NVARCHAR(500),
	@workflowStatus AS INT,
	@comment AS NVARCHAR(500) = NULL,
	@contextTaskId AS BIGINT = NULL,
	@contextFileId AS BIGINT = NULL,
	@contextClientId AS BIGINT = NULL,
	@contextAssociateId AS BIGINT = NULL,
	@contextFeeEarnerId AS BIGINT = NULL,
	@contextDocumentId AS BIGINT = NULL,
	@contextDocumentVersionNo AS INT = NULL,
	@contextUserId AS INT = NULL,
	@contextBranchId AS INT = NULL,
	@contextPrecedentId AS BIGINT = NULL,
	@contextContactId AS BIGINT =NULL
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[dbWorkflowToken]
			([UserId]
			,[WorkflowToken]
			,[WorkflowCode]
			,[WorkflowURI]
			,[WorkflowStatus]
			,[Comment]
			,[ContextTaskId]
			,[ContextFileId]
			,[ContextClientId]
			,[ContextAssociateId]
			,[ContextFeeEarnerId]
			,[ContextDocumentId]
			,[ContextDocumentVersionNo]
			,[ContextUserId]
			,[ContextBranchId]
			,[ContextPrecedentId]
			,[ContextContactId])
		VALUES
           (@userId,
			@workflowToken,
			@workflowCode,
			@workflowURI,
			@workflowStatus,
			@comment,
			@contextTaskId,
			@contextFileId,
			@contextClientId,
			@contextAssociateId,
			@contextFeeEarnerId,
			@contextDocumentId,
			@contextDocumentVersionNo,
			@contextUserId,
			@contextBranchId,
			@contextPrecedentId,
			@contextContactId)
	SELECT SCOPE_IDENTITY()
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[wfNewToken] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[wfNewToken] TO [OMSAdminRole]
    AS [dbo];


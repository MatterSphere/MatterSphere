CREATE TABLE [dbo].[dbWorkflowToken] (
    [Id]                       BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]                   INT            NOT NULL,
    [WorkflowToken]            NVARCHAR (300) NOT NULL,
    [WorkflowCode]             NVARCHAR (200) NOT NULL,
    [WorkflowURI]              NVARCHAR (500) NOT NULL,
    [WorkflowStatus]           INT            NOT NULL,
    [Comment]                  NVARCHAR (500) NULL,
    [ContextTaskId]            BIGINT         NULL,
    [ContextFileId]            BIGINT         NULL,
    [ContextClientId]          BIGINT         NULL,
    [ContextAssociateId]       BIGINT         NULL,
    [ContextFeeEarnerId]       BIGINT         NULL,
    [ContextDocumentId]        BIGINT         NULL,
    [ContextDocumentVersionNo] INT            NULL,
    [ContextUserId]            INT            NULL,
    [ContextBranchId]          INT            NULL,
    [ContextPrecedentId]       BIGINT         NULL,
    [ContextContactId]         BIGINT         NULL,
    CONSTRAINT [PK_dbWorkflowToken] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbWorkflowToken]
    ON [dbo].[dbWorkflowToken]([WorkflowToken] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_dbWorkflowToken_1]
    ON [dbo].[dbWorkflowToken]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_dbWorkflowToken_2]
    ON [dbo].[dbWorkflowToken]([WorkflowStatus] ASC);


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbWorkflowToken] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbWorkflowToken] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbWorkflowToken] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbWorkflowToken] TO [OMSApplicationRole]
    AS [dbo];


CREATE TABLE [dbo].[dbWorkflowConfig] (
    [wfCode]    NVARCHAR (100)     NOT NULL,
    [wfVersion] BIGINT             CONSTRAINT [DF_dbWorkflowConfig_wfVersion] DEFAULT ((1)) NOT NULL,
    [wfConfig]  NVARCHAR (MAX)     NULL,
    [Created]   [dbo].[uCreated]   CONSTRAINT [DF_dbWorkflowConfig_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy] [dbo].[uCreatedBy] NULL,
    [Updated]   [dbo].[uCreated]   NULL,
    [UpdatedBy] [dbo].[uCreatedBy] NULL,
    [rowguid]   UNIQUEIDENTIFIER   CONSTRAINT [DF_dbWorkflowConfig_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbWorkflowConfig] PRIMARY KEY CLUSTERED ([wfCode] ASC) WITH (FILLFACTOR = 90)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbWorkflowConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbWorkflowConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbWorkflowConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbWorkflowConfig] TO [OMSApplicationRole]
    AS [dbo];


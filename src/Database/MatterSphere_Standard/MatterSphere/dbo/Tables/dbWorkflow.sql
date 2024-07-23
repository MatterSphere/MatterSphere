CREATE TABLE [dbo].[dbWorkflow] (
    [wfCode]       NVARCHAR (200)     NOT NULL,
    [wfVersion]    BIGINT             CONSTRAINT [DF_dbWorkflow_wfVersion] DEFAULT ((1)) NOT NULL,
    [wfXaml]       NVARCHAR (MAX)     NOT NULL,
    [wfAssemblies] NVARCHAR (MAX)     NULL,
    [Created]      [dbo].[uCreated]   CONSTRAINT [DF_dbWorkflow_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]    [dbo].[uCreatedBy] NULL,
    [Updated]      [dbo].[uCreated]   NULL,
    [UpdatedBy]    [dbo].[uCreatedBy] NULL,
    [rowguid]      UNIQUEIDENTIFIER   CONSTRAINT [DF_dbWorkflow_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [wfGroup]      NVARCHAR (50)      CONSTRAINT [DF_dbWorkflow_wfGroup] DEFAULT ('') NOT NULL,
    [wfNotes]      NVARCHAR (500)     CONSTRAINT [DF_dbWorkflow_wfNotes] DEFAULT ('') NOT NULL,
    [wfCodelookup] NVARCHAR (15)      NULL,
    [wfAttributes] BIGINT             CONSTRAINT [DF_dbWorkflow_wfAttributes] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbWorkflow] PRIMARY KEY CLUSTERED ([wfCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE NONCLUSTERED INDEX [IX_DBWorkflow_wfGroup]
    ON [dbo].[dbWorkflow]([wfGroup] ASC) WITH (FILLFACTOR = 85)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbWorkflow] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbWorkflow] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbWorkflow] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbWorkflow] TO [OMSApplicationRole]
    AS [dbo];


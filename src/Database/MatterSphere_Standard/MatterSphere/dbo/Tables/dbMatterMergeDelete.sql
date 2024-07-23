CREATE TABLE [dbo].[dbMatterMergeDelete] (
    [LogID]                  INT              IDENTITY (1, 1) NOT NULL,
    [OldFileID]              BIGINT           NOT NULL,
    [NewFileID]              BIGINT           NULL,
    [ClID]                   BIGINT           NOT NULL,
    [UsrID]                  INT              NOT NULL,
    [DeletedGuid]            UNIQUEIDENTIFIER NULL,
    [EntryDate]              DATETIME         NOT NULL,
    [ExecutionStartDate]     DATETIME         NULL,
    [ExecutionCompletedDate] DATETIME         NULL,
    [ExecutionXML]           XML              CONSTRAINT [DF_dbMatterMergeDelete_ExecutionLog] DEFAULT ('<log/>') NOT NULL,
    [ExecutionStatus]        TINYINT          CONSTRAINT [DF_dbMatterMergeDelete_ExecutionStatus] DEFAULT ((0)) NOT NULL,
    [ExecutionError]         NVARCHAR (MAX)   NULL,
    [rowguid]                UNIQUEIDENTIFIER CONSTRAINT [DF_dbMatterMergeDelete_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbMatterMergeDelete] PRIMARY KEY CLUSTERED ([LogID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMatterMergeDelete_rowguid]
    ON [dbo].[dbMatterMergeDelete]([LogID] ASC)
    ON [IndexGroup];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 queued, 1 executing, 2 errored, 5 complete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'dbMatterMergeDelete', @level2type = N'COLUMN', @level2name = N'ExecutionStatus';


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMatterMergeDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMatterMergeDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMatterMergeDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMatterMergeDelete] TO [OMSApplicationRole]
    AS [dbo];


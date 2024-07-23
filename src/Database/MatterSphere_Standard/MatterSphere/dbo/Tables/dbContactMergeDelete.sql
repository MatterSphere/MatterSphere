CREATE TABLE [dbo].[dbContactMergeDelete] (
    [LogID]                  INT                 IDENTITY (1, 1) NOT NULL,
    [OldContID]              BIGINT              NOT NULL,
    [ContTypeCode]           [dbo].[uCodeLookup] NOT NULL,
    [NewContID]              BIGINT              NULL,
    [UsrID]                  INT                 NOT NULL,
    [DeletedGuid]            UNIQUEIDENTIFIER    NULL,
    [EntryDate]              DATETIME            NOT NULL,
    [ExecutionStartDate]     DATETIME            NULL,
    [ExecutionCompletedDate] DATETIME            NULL,
    [ExecutionXML]           XML                 CONSTRAINT [DF_dbContactMergeDelete_ExecutionLog] DEFAULT ('<log/>') NOT NULL,
    [ExecutionStatus]        TINYINT             CONSTRAINT [DF_dbContactMergeDelete_ExecutionStatus] DEFAULT ((0)) NOT NULL,
    [ExecutionError]         NVARCHAR (MAX)      NULL,
    [rowguid]                UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactMergeDelete_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactMergeDelete] PRIMARY KEY CLUSTERED ([LogID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactMergeDelete_rowguid]
    ON [dbo].[dbContactMergeDelete]([LogID] ASC)
    ON [IndexGroup];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 queued, 1 executing, 2 errored, 5 complete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'dbContactMergeDelete', @level2type = N'COLUMN', @level2name = N'ExecutionStatus';


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactMergeDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactMergeDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactMergeDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactMergeDelete] TO [OMSApplicationRole]
    AS [dbo];


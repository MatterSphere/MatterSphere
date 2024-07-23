CREATE TABLE [dbo].[dbClientDelete] (
    [LogID]                  INT              IDENTITY (1, 1) NOT NULL,
    [ClID]                   BIGINT           NOT NULL,
    [DefaultContactID]       BIGINT           NULL,
    [UsrID]                  INT              NOT NULL,
    [DeletedGuid]            UNIQUEIDENTIFIER NULL,
    [EntryDate]              DATETIME         NOT NULL,
    [ExecutionStartDate]     DATETIME         NULL,
    [ExecutionCompletedDate] DATETIME         NULL,
    [ExecutionXML]           XML              CONSTRAINT [DF_dbClientDelete_ExecutionLog] DEFAULT ('<log/>') NOT NULL,
    [ExecutionStatus]        TINYINT          CONSTRAINT [DF_dbClientDelete_ExecutionStatus] DEFAULT ((0)) NOT NULL,
    [ExecutionError]         NVARCHAR (MAX)   NULL,
    [rowguid]                UNIQUEIDENTIFIER CONSTRAINT [DF_dbClientDelete_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbClientDelete] PRIMARY KEY CLUSTERED ([LogID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbClientDelete_rowguid]
    ON [dbo].[dbClientDelete]([LogID] ASC)
    ON [IndexGroup];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 queued, 1 executing, 2 errored, 5 complete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'dbClientDelete', @level2type = N'COLUMN', @level2name = N'ExecutionStatus';


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbClientDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbClientDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbClientDelete] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbClientDelete] TO [OMSApplicationRole]
    AS [dbo];


CREATE TABLE [dbo].[dbMilestoneConfigStage] (
    [stgID]              INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [msCode]             [dbo].[uCodeLookup] NOT NULL,
    [stgNumber]          TINYINT             CONSTRAINT [DF_dbMilestoneStage_Number] DEFAULT ((1)) NOT NULL,
    [stgName]            NVARCHAR (50)       NOT NULL,
    [stgLongDesc]        NVARCHAR (MAX)      NULL,
    [stgInteractiveDesc] NVARCHAR (MAX)      NULL,
    [stgDays]            SMALLINT            CONSTRAINT [DF_dbMilestoneStage_stgDays] DEFAULT ((0)) NOT NULL,
    [stgAction]          [dbo].[uCodeLookup] NULL,
    [stgCalcFrom]        TINYINT             CONSTRAINT [DF_dbMilestoneStage_stgCalcFrom] DEFAULT ((1)) NOT NULL,
    [rowguid]            UNIQUEIDENTIFIER    CONSTRAINT [DF_dbMilestoneConfigStage_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbMilestoneStage] PRIMARY KEY CLUSTERED ([msCode] ASC, [stgNumber] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbMilestoneConfigStage_dbMilestoneConfig] FOREIGN KEY ([msCode]) REFERENCES [dbo].[dbMilestoneConfig] ([msCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMilestoneConfigStage_rowguid]
    ON [dbo].[dbMilestoneConfigStage]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMilestoneConfigStage_stgID]
    ON [dbo].[dbMilestoneConfigStage]([stgID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMilestoneConfigStage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMilestoneConfigStage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMilestoneConfigStage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMilestoneConfigStage] TO [OMSApplicationRole]
    AS [dbo];


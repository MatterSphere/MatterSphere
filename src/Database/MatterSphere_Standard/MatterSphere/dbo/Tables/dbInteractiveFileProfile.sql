CREATE TABLE [dbo].[dbInteractiveFileProfile] (
    [ID]            BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [contID]        BIGINT           NOT NULL,
    [clid]          BIGINT           NOT NULL,
    [fileid]        BIGINT           NOT NULL,
    [proContact]    BIT              CONSTRAINT [DF_dbInteractiveFileProfile_proContact] DEFAULT ((0)) NOT NULL,
    [proMilestones] BIT              CONSTRAINT [DF_dbInteractiveFileProfile_proContact1] DEFAULT ((0)) NULL,
    [proNotes]      BIT              CONSTRAINT [DF_dbInteractiveFileProfile_proContact2] DEFAULT ((0)) NULL,
    [proActionable] BIT              CONSTRAINT [DF_dbInteractiveFileProfile_proContact3] DEFAULT ((0)) NULL,
    [proDocs]       BIT              CONSTRAINT [DF_dbInteractiveFileProfile_proContact4] DEFAULT ((0)) NULL,
    [proSMS]        BIT              CONSTRAINT [DF_dbInteractiveFileProfile_proContact5] DEFAULT ((0)) NULL,
    [proSecValue]   INT              NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_dbInteractiveFileProfile_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbInteractiveFileProfile] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbInteractiveFileProfile_rowguid]
    ON [dbo].[dbInteractiveFileProfile]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbInteractiveFileProfileIDKeys]
    ON [dbo].[dbInteractiveFileProfile]([clid] ASC, [contID] ASC, [fileid] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbInteractiveFileProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbInteractiveFileProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbInteractiveFileProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbInteractiveFileProfile] TO [OMSApplicationRole]
    AS [dbo];


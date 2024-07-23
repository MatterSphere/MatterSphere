CREATE TABLE [dbo].[dbFPEData] (
    [fpeLinkID]      BIGINT           NOT NULL,
    [fpeType]        NVARCHAR (50)    NOT NULL,
    [fpeFormID]      INT              NOT NULL,
    [fpeFormGuid]    UNIQUEIDENTIFIER NULL,
    [fpeData]        XML              NULL,
    [fpeUpdated]     DATETIME         NULL,
    [fpeUpdatedBy]   INT              NULL,
    [fpeCreated]     DATETIME         NULL,
    [fpeCreatedBy]   INT              NULL,
    [fpeOurRef]      NVARCHAR (50)    NULL,
    [fpeYourRef]     NVARCHAR (50)    NULL,
    [fpeEmail]       NVARCHAR (150)   NULL,
    [fpeDescription] NVARCHAR (250)   NULL,
    [fpeRefreshed]   DATETIME         NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_dbFPEData_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [fpeCampaignID]  INT              NULL,
    [fpeCompleted]   DATETIME         NULL,
    [fpeRequested]   DATETIME         NULL,
    CONSTRAINT [PK_dbFPEData] PRIMARY KEY CLUSTERED ([fpeLinkID] ASC, [fpeType] ASC, [fpeFormID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFPEData_rowguid]
    ON [dbo].[dbFPEData]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFPEData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFPEData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFPEData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFPEData] TO [OMSApplicationRole]
    AS [dbo];


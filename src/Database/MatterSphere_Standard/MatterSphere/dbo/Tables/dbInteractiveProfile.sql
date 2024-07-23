CREATE TABLE [dbo].[dbInteractiveProfile] (
    [contID]           BIGINT             NOT NULL,
    [proUserName]      NVARCHAR (50)      NULL,
    [proPassword]      NVARCHAR (50)      NULL,
    [proEmail]         [dbo].[uEmail]     NULL,
    [proSMSNumber]     [dbo].[uTelephone] NULL,
    [proInformEmail]   BIT                CONSTRAINT [DF_dbInteractiveProfile_proEmailInformChange] DEFAULT ((0)) NULL,
    [proInformSMS]     BIT                CONSTRAINT [DF_dbInteractiveProfile_proSMSInformChange] DEFAULT ((0)) NULL,
    [proSecValue]      INT                CONSTRAINT [DF_dbInteractiveProfile_proAccessLevel] DEFAULT ((0)) NULL,
    [proDefSecSetting] NVARCHAR (10)      NULL,
    [rowguid]          UNIQUEIDENTIFIER   CONSTRAINT [DF_dbInteractiveProfile_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbInteractiveProfile] PRIMARY KEY CLUSTERED ([contID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbInteractiveProfile_rowguid]
    ON [dbo].[dbInteractiveProfile]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbInteractiveProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbInteractiveProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbInteractiveProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbInteractiveProfile] TO [OMSApplicationRole]
    AS [dbo];


CREATE TABLE [dbo].[dbCDSClaim] (
    [UFNID]               NVARCHAR (50)       NOT NULL,
    [UFNCode]             NVARCHAR (12)       NOT NULL,
    [UFNProCosts]         MONEY               NULL,
    [UFNDisb]             MONEY               NULL,
    [UFNTravel]           MONEY               NULL,
    [UFNWaiting]          MONEY               NULL,
    [UFNCDS11Created]     [dbo].[uCreated]    CONSTRAINT [DF_dbCDSClaim_UFNCDS11Created] DEFAULT ((0)) NULL,
    [UFNCDS11CreatedBy]   NVARCHAR (12)       NULL,
    [UFNCLCode]           NVARCHAR (12)       NULL,
    [UFNOffCode]          NVARCHAR (12)       NULL,
    [UFNOutCode]          NVARCHAR (12)       NULL,
    [UFNConcluded]        DATETIME            NULL,
    [UFNSuspects]         INT                 NULL,
    [UFNAttendances]      INT                 NULL,
    [UFNIdentifier]       NVARCHAR (50)       NULL,
    [UFNDuty]             BIT                 NULL,
    [UFNYouth]            BIT                 NULL,
    [UFNClaimed]          BIT                 NULL,
    [UFNClaimedDate]      DATETIME            NULL,
    [UFNCDSRef]           NVARCHAR (50)       NULL,
    [UFNbrID]             INT                 NULL,
    [UFNCDSSupplementary] BIT                 NULL,
    [UFNSupplierNo]       NVARCHAR (12)       NULL,
    [rowguid]             UNIQUEIDENTIFIER    CONSTRAINT [DF_dbCDSClaim_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [UFNDisabilityFlag]   [dbo].[uCodeLookup] NULL,
    [UFNEthnicOrigin]     [dbo].[uCodeLookup] NULL,
    [UFNSex]              [dbo].[uCodeLookup] NULL,
    CONSTRAINT [PK_dbCDSClaim] PRIMARY KEY CLUSTERED ([UFNID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCDSClaim_rowguid]
    ON [dbo].[dbCDSClaim]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbCDSClaim_UFNCode]
    ON [dbo].[dbCDSClaim]([UFNCode] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbCDSClaim_UFNID]
    ON [dbo].[dbCDSClaim]([UFNID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCDSClaim] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCDSClaim] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCDSClaim] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCDSClaim] TO [OMSApplicationRole]
    AS [dbo];


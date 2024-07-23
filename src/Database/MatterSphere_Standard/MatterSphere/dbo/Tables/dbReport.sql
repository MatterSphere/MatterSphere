CREATE TABLE [dbo].[dbReport] (
    [rptCode]     [dbo].[uCodeLookup] NOT NULL,
    [rptVersion]  BIGINT              CONSTRAINT [DF_dbReport_rptVersion] DEFAULT ((1)) NOT NULL,
    [rptBLOB]     VARBINARY (MAX)     NULL,
    [rptXML]      VARBINARY (MAX)     NULL,
    [rptPubName]  NVARCHAR (100)      NULL,
    [rptSystem]   BIT                 CONSTRAINT [DF_dbReport_rptSystem] DEFAULT ((0)) NOT NULL,
    [rptScript]   NVARCHAR (15)       NULL,
    [rptKeywords] NVARCHAR (MAX)      NULL,
    [rptNotes]    NVARCHAR (MAX)      NULL,
    [Created]     [dbo].[uCreated]    NULL,
    [CreatedBy]   [dbo].[uCreatedBy]  NULL,
    [Updated]     [dbo].[uCreated]    NULL,
    [UpdatedBy]   [dbo].[uCreatedBy]  NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbReport_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbReport] PRIMARY KEY CLUSTERED ([rptCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbReport_rowguid]
    ON [dbo].[dbReport]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbReport] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbReport] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbReport] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbReport] TO [OMSApplicationRole]
    AS [dbo];


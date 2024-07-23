CREATE TABLE [dbo].[dbEnquiryPage] (
    [enqID]              INT                 NOT NULL,
    [pgeOrder]           SMALLINT            NOT NULL,
    [pgeCode]            [dbo].[uCodeLookup] NULL,
    [pgeShortCode]       [dbo].[uCodeLookup] NULL,
    [pgeCustom]          BIT                 CONSTRAINT [DF_dbEnquiryPages_pgeCustom] DEFAULT ((0)) NOT NULL,
    [pgeEdition]         VARCHAR (2)         NULL,
    [pgeName]            NVARCHAR (25)       NULL,
    [pgeFinishedEnabled] BIT                 CONSTRAINT [DF_dbEnquiryPage_pgeFinishedEnabled] DEFAULT ((1)) NOT NULL,
    [pgeCondition]       NVARCHAR (500)      NULL,
    [pgeRole]            NVARCHAR (200)      NULL,
    [pgeLicense]         NVARCHAR (200)      NULL,
    [rowguid]            UNIQUEIDENTIFIER    CONSTRAINT [DF_dbEnquiryPage_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [pgeSettings]        NVARCHAR (MAX)      NULL,
    CONSTRAINT [PK_dbEnquiryPages] PRIMARY KEY CLUSTERED ([enqID] ASC, [pgeOrder] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbEnquiryPages_dbEnquiry] FOREIGN KEY ([enqID]) REFERENCES [dbo].[dbEnquiry] ([enqID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiryPage_rowguid]
    ON [dbo].[dbEnquiryPage]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiryPage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiryPage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiryPage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiryPage] TO [OMSApplicationRole]
    AS [dbo];


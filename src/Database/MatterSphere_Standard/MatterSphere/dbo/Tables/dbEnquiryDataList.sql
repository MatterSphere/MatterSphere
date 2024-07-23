CREATE TABLE [dbo].[dbEnquiryDataList] (
    [enqTable]      [dbo].[uCodeLookup] NOT NULL,
    [enqSourceType] [dbo].[uCodeLookup] CONSTRAINT [DF_dbEnquiryDataList_extSourceType] DEFAULT (N'OMS') NOT NULL,
    [enqSource]     NVARCHAR (500)      NULL,
    [enqCall]       NVARCHAR (2500)     NULL,
    [enqParameters] [dbo].[uXML]        CONSTRAINT [DF_dbEnquiryDataList_extParameters] DEFAULT (N'<params></params>') NULL,
    [enqSystem]     BIT                 CONSTRAINT [DF_dbEnquiryDataList_enqSystem] DEFAULT ((0)) NOT NULL,
    [Created]       [dbo].[uCreated]    CONSTRAINT [DF_dbEnquiryDataList_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     [dbo].[uCreatedBy]  NULL,
    [Updated]       [dbo].[uCreated]    CONSTRAINT [DF_dbEnquiryDataList_Updated] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]     [dbo].[uCreatedBy]  NULL,
    [rowguid]       UNIQUEIDENTIFIER    CONSTRAINT [DF_dbEnquiryDataList_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [enqApiExclude] BIT                 DEFAULT ((1)) NOT NULL,
    [enqDLVersion]  INT                 NULL,
    CONSTRAINT [PK_dbEnquiryDataList] PRIMARY KEY CLUSTERED ([enqTable] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiryDataList_rowguid]
    ON [dbo].[dbEnquiryDataList]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiryDataList] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiryDataList] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiryDataList] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiryDataList] TO [OMSApplicationRole]
    AS [dbo];


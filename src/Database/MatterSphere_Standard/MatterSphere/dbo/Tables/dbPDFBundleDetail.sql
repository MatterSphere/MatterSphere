CREATE TABLE [dbo].[dbPDFBundleDetail] (
    [bundleDocID]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [bundleID]       BIGINT           NOT NULL,
    [docID]          BIGINT           NULL,
    [docOrder]       INT              NULL,
    [docDescription] NVARCHAR (500)   NULL,
    [bundleCategory] NVARCHAR (15)    NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_dbPDFBundleDetail_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPDFBundleDetail] PRIMARY KEY CLUSTERED ([bundleDocID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPDFBundleDetail] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPDFBundleDetail] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPDFBundleDetail] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPDFBundleDetail] TO [OMSApplicationRole]
    AS [dbo];


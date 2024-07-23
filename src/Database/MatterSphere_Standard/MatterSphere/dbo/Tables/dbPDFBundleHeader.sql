CREATE TABLE [dbo].[dbPDFBundleHeader] (
    [bundleID]                   BIGINT           IDENTITY (1, 1) NOT NULL,
    [fileID]                     BIGINT           NULL,
    [bundleDescription]          NVARCHAR (255)   NULL,
    [bundleIncludeTOCs]          BIT              NULL,
    [bundleInsertBlankPages]     BIT              NULL,
    [bundleIncludePageNumbering] BIT              NULL,
    [bundleUseFirstDocAsCover]   BIT              NULL,
    [bundleStatus]               TINYINT          NULL,
    [bundleLog]                  NVARCHAR (MAX)   NULL,
    [CreatedBy]                  INT              NULL,
    [Created]                    DATETIME         NULL,
    [UpdatedBy]                  INT              NULL,
    [Updated]                    DATETIME         NULL,
    [rowguid]                    UNIQUEIDENTIFIER CONSTRAINT [DF_dbPDFBundleHeader_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPDFBundleHeader] PRIMARY KEY CLUSTERED ([bundleID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPDFBundleHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPDFBundleHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPDFBundleHeader] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPDFBundleHeader] TO [OMSApplicationRole]
    AS [dbo];


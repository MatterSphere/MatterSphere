CREATE TABLE [dbo].[dbEnquiryVersionData] (
    [VersionID]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [Code]              NVARCHAR (15)    NOT NULL,
    [Version]           BIGINT           NULL,
    [dbEnquiry]         NVARCHAR (MAX)   NULL,
    [dbEnquiryQuestion] NVARCHAR (MAX)   NULL,
    [dbEnquiryPage]     NVARCHAR (MAX)   NULL,
    [Created]           DATETIME         NULL,
    [CreatedBy]         INT              NULL,
    [Comments]          NVARCHAR (MAX)   NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_dbEnquiryVersionData_rowguid] DEFAULT (newid()) ROWGUIDCOL NULL,
    CONSTRAINT [PK_dbEnquiryVersionData] PRIMARY KEY CLUSTERED ([VersionID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiryVersionData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiryVersionData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiryVersionData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiryVersionData] TO [OMSApplicationRole]
    AS [dbo];


CREATE TABLE [dbo].[dbEnquiryControl] (
    [ctrlID]               INT                 NOT NULL,
    [ctrlCode]             [dbo].[uCodeLookup] NOT NULL,
    [ctrlGroup]            [dbo].[uCodeLookup] NULL,
    [ctrlSystem]           BIT                 CONSTRAINT [DF_dbEnquiryControl_ctrlSystem] DEFAULT ((0)) NOT NULL,
    [ctrlWinType]          NVARCHAR (500)      NOT NULL,
    [ctrlWebType]          NVARCHAR (500)      NULL,
    [ctrlPDAType]          NVARCHAR (500)      NULL,
    [rowguid]              UNIQUEIDENTIFIER    CONSTRAINT [DF_dbEnquiryControl_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ctrlAssemblyFileName] NVARCHAR (100)      NULL,
    CONSTRAINT [PK_dbEnquiryControl] PRIMARY KEY CLUSTERED ([ctrlID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiryControl_rowguid]
    ON [dbo].[dbEnquiryControl]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiryControl] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiryControl] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiryControl] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiryControl] TO [OMSApplicationRole]
    AS [dbo];


CREATE TABLE [dbo].[dbEnquiryCommand] (
    [cmdCode]       [dbo].[uCodeLookup] NOT NULL,
    [cmdType]       NVARCHAR (100)      CONSTRAINT [DF_dbEnquiryCommand_cmdType] DEFAULT (N'FWBS.OMS.UI.Windows.Services') NOT NULL,
    [cmdMethod]     VARCHAR (100)       NOT NULL,
    [cmdParameters] [dbo].[uXML]        CONSTRAINT [DF_dbEnquiryCommand_cmdParameters] DEFAULT (N'<params>
	</params>') NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER    CONSTRAINT [DF_dbEnquiryCommand_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbEnquiryCommand] PRIMARY KEY CLUSTERED ([cmdCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiryCommand_rowguid]
    ON [dbo].[dbEnquiryCommand]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiryCommand] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiryCommand] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiryCommand] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiryCommand] TO [OMSApplicationRole]
    AS [dbo];


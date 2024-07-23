CREATE TABLE [dbo].[dbEnquiryMethod] (
    [enqID]         INT              NOT NULL,
    [enqOrder]      TINYINT          NOT NULL,
    [enqMethod]     VARCHAR (100)    NOT NULL,
    [enqParameters] [dbo].[uXML]     CONSTRAINT [DF_dbEnquiryMethod_enqParameters] DEFAULT (N'<params>
	</params>') NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_dbEnquiryMethod_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbEnquiryMethod] PRIMARY KEY CLUSTERED ([enqID] ASC, [enqOrder] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbEnquiryMethod_dbEnquiry] FOREIGN KEY ([enqID]) REFERENCES [dbo].[dbEnquiry] ([enqID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiryMethod_rowguid]
    ON [dbo].[dbEnquiryMethod]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiryMethod] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiryMethod] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiryMethod] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiryMethod] TO [OMSApplicationRole]
    AS [dbo];


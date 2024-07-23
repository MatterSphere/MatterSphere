CREATE TABLE [dbo].[dbEnquiryDataSource] (
    [enqID]    INT              NOT NULL,
    [enqOrder] TINYINT          NOT NULL,
    [enqTable] VARCHAR (50)     NOT NULL,
    [rowguid]  UNIQUEIDENTIFIER CONSTRAINT [DF_dbEnquiryDataSource_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbEnquiryDataSource] PRIMARY KEY CLUSTERED ([enqID] ASC, [enqOrder] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbEnquiryDataSource_dbEnquiry] FOREIGN KEY ([enqID]) REFERENCES [dbo].[dbEnquiry] ([enqID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiryDataSource_rowguid]
    ON [dbo].[dbEnquiryDataSource]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiryDataSource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiryDataSource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiryDataSource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiryDataSource] TO [OMSApplicationRole]
    AS [dbo];


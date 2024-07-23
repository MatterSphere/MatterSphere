CREATE TABLE [dbo].[dbDocumentEmail] (
    [docID]                BIGINT           NOT NULL,
    [docStoreID]           VARCHAR (1000)   NULL,
    [docEntryID]           VARCHAR (1000)   NULL,
    [docFrom]              NVARCHAR (255)   NULL,
    [docTo]                NVARCHAR (1000)  NULL,
    [docCC]                NVARCHAR (1000)  NULL,
    [docSent]              DATETIME         NULL,
    [docClass]             NVARCHAR (100)   NULL,
    [docAttachments]       INT              CONSTRAINT [DF_dbDocumentEmail_docAttachments] DEFAULT ((0)) NOT NULL,
    [docReceived]          DATETIME         NULL,
    [rowguid]              UNIQUEIDENTIFIER CONSTRAINT [DF_dbDocumentEmail_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [docConversationIndex] NVARCHAR (250)   NULL,
    [docConversationTopic] NVARCHAR (250)   NULL,
    CONSTRAINT [PK_dbDocumentEmail] PRIMARY KEY CLUSTERED ([docID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocumentEmail_rowguid]
    ON [dbo].[dbDocumentEmail]([rowguid] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentEmail] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentEmail] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentEmail] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentEmail] TO [OMSApplicationRole]
    AS [dbo];


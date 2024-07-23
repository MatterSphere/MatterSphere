CREATE TABLE [dbo].[dbInstantMessageAttachment] (
    [ID]          UNIQUEIDENTIFIER CONSTRAINT [DF_dbInstantMessageAttachment_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [messageID]   UNIQUEIDENTIFIER NOT NULL,
    [Description] NVARCHAR (150)   NOT NULL,
    [LinkID]      UNIQUEIDENTIFIER NULL,
    [Binary]      VARBINARY (MAX)  NULL,
    [Created]     DATETIME         CONSTRAINT [DF_dbInstantMessageAttachment_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]   INT              NOT NULL,
    CONSTRAINT [PK_dbInstantMessageAttachment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbInstantMessageAttachment_dbInstantMessage] FOREIGN KEY ([messageID]) REFERENCES [dbo].[dbInstantMessage] ([ID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbInstantMessageAttachment_dbUser] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbInstantMessageAttachment] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbInstantMessageAttachment] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbInstantMessageAttachment] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbInstantMessageAttachment] TO [OMSApplicationRole]
    AS [dbo];


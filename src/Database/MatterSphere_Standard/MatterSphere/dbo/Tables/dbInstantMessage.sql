CREATE TABLE [dbo].[dbInstantMessage] (
    [ID]        UNIQUEIDENTIFIER CONSTRAINT [DF_dbInstantMessage_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ThreadID]  UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy] INT              NOT NULL,
    [Created]   DATETIME         CONSTRAINT [DF_dbInstantMessage_Created] DEFAULT (getutcdate()) NOT NULL,
    [Message]   NVARCHAR (500)   NOT NULL,
    CONSTRAINT [PK_dbInstantMessage] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbInstantMessage_dbInstantMessageThread] FOREIGN KEY ([ThreadID]) REFERENCES [dbo].[dbInstantMessageThread] ([ID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbInstantMessage_dbUser] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbInstantMessage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbInstantMessage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbInstantMessage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbInstantMessage] TO [OMSApplicationRole]
    AS [dbo];


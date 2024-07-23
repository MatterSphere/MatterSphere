CREATE TABLE [dbo].[dbInstantMessageUserStatus] (
    [ID]        UNIQUEIDENTIFIER CONSTRAINT [DF_dbInstantMessageUserStatus_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [messageID] UNIQUEIDENTIFIER NOT NULL,
    [usrID]     INT              NOT NULL,
    [Status]    TINYINT          CONSTRAINT [DF_dbInstantMessageUserStatus_Status] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbInstantMessageUserStatus_1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbInstantMessageUserStatus_dbInstantMessage] FOREIGN KEY ([messageID]) REFERENCES [dbo].[dbInstantMessage] ([ID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbInstantMessageUserStatus_dbUser] FOREIGN KEY ([usrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbInstantMessageUserStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbInstantMessageUserStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbInstantMessageUserStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbInstantMessageUserStatus] TO [OMSApplicationRole]
    AS [dbo];

